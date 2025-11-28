using Microsoft.ML;
using Microsoft.ML.Data;
using PostgreProject.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML.Transforms.TimeSeries;

namespace PostgreProject.Services
{
	public class SalesForecastService
	{
		private readonly AppDbContext _context;
		private readonly MLContext _mlContext;

		public SalesForecastService(AppDbContext context)
		{
			_context = context;
			_mlContext = new MLContext(seed: 1);
		}

		public async Task<List<DailySalesData>> PrepareDailySalesData()
		{
			var startDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			var endDate = new DateTime(2025, 9, 30, 23, 59, 59, DateTimeKind.Utc);

			var dailySales = await _context.Orders
				.Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate) 
				.GroupBy(o => o.OrderDate.Date)
				.Select(g => new DailySalesData
				{
					Date = g.Key,
					TotalSales = (float)g.Sum(o => o.TotalPrice),
					OrderCount = g.Count()
				})
				.OrderBy(d => d.Date)
				.ToListAsync();

			return dailySales;
		}


		// 3 aylık tahmin yap (Ekim, Kasım, Aralık)
		public async Task<ForecastResult> ForecastNextThreeMonths()
		{
			// Ocak-Eylül verilerini al
			var trainData = await PrepareDailySalesData();

			if (trainData.Count == 0)
				throw new Exception("Eğitim verisi bulunamadı!");

			// IDataView'e çevir
			var dataView = _mlContext.Data.LoadFromEnumerable(trainData);

			// Model pipeline oluştur
			var forecastingPipeline = _mlContext.Forecasting.ForecastBySsa(
				outputColumnName: "ForecastedSales",
				inputColumnName: nameof(DailySalesData.TotalSales),
				windowSize: 7,        // 7 günlük pencere
				seriesLength: 30,     // 30 günlük seri
				trainSize: trainData.Count,
				horizon: 90,          // 90 gün tahmin (Ekim-Kasım-Aralık)
				confidenceLevel: 0.95f,
				confidenceLowerBoundColumn: "LowerBound",
				confidenceUpperBoundColumn: "UpperBound"
			);

			// Modeli eğit
			var model = forecastingPipeline.Fit(dataView);

			// Tahmin yap
			var forecastEngine = model.CreateTimeSeriesEngine<DailySalesData, SalesForecast>(_mlContext);
			var forecast = forecastEngine.Predict();

			// Sonuçları aylık grupla
			var result = new ForecastResult
			{
				TrainingDataCount = trainData.Count,
				LastTrainingDate = trainData.Max(d => d.Date),
				MonthlyForecasts = new List<MonthlyForecast>()
			};

			// Ekim, Kasım, Aralık için aylık toplam hesapla
			int dayIndex = 0; 

			for (int month = 10; month <= 12; month++)
			{
				int daysInMonth = DateTime.DaysInMonth(2025, month);
				float monthlyTotal = 0;

				// Her ay için sırayla günleri al
				for (int day = 0; day < daysInMonth && dayIndex < forecast.ForecastedSales.Length; day++)
				{
					monthlyTotal += forecast.ForecastedSales[dayIndex];
					dayIndex++; // Index'i sürekli artır
				}

				result.MonthlyForecasts.Add(new MonthlyForecast
				{
					Month = month,
					Year = 2025,
					MonthName = new DateTime(2025, month, 1).ToString("MMMM"),
					PredictedSales = monthlyTotal
				});
			}
			return result;
		}

		// Geçmiş aylık satışları getir (karşılaştırma için)
		public async Task<List<MonthlyForecast>> GetHistoricalMonthlySales()
		{
			var startDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			var endDate = new DateTime(2025, 9, 30, 23, 59, 59, DateTimeKind.Utc);

			var monthlySales = await _context.Orders
				.Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate) 
				.GroupBy(o => new { o.OrderDate.Year, o.OrderDate.Month })
				.Select(g => new MonthlyForecast
				{
					Year = g.Key.Year,
					Month = g.Key.Month,
					MonthName = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMMM"),
					PredictedSales = (float)g.Sum(o => o.TotalPrice),
					IsActual = true
				})
				.OrderBy(m => m.Year).ThenBy(m => m.Month)
				.ToListAsync();

			return monthlySales;
		}
	}

	public class DailySalesData
	{
		public DateTime Date { get; set; }
		public float TotalSales { get; set; }
		public int OrderCount { get; set; }
	}

	public class SalesForecast
	{
		[VectorType(90)] // 90 günlük tahmin
		public float[] ForecastedSales { get; set; }

		[VectorType(90)]
		public float[] LowerBound { get; set; }

		[VectorType(90)]
		public float[] UpperBound { get; set; }
	}

	public class ForecastResult
	{
		public int TrainingDataCount { get; set; }
		public DateTime LastTrainingDate { get; set; }
		public List<MonthlyForecast> MonthlyForecasts { get; set; }
	}

	public class MonthlyForecast
	{
		public int Year { get; set; }
		public int Month { get; set; }
		public string MonthName { get; set; }
		public float PredictedSales { get; set; }
		public bool IsActual { get; set; } = false;
	}
}