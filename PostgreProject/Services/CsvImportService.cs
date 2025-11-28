using CsvHelper;
using CsvHelper.Configuration;
using PostgreProject.Context;
using PostgreProject.Entities;
using PostgreProject.Dtos;
using System.Globalization;

namespace PostgreProject.Services
{
	public class CsvImportService
	{
		private readonly AppDbContext _context;

		public CsvImportService(AppDbContext context)
		{
			_context = context;
		}

		public async Task<int> ImportOrdersFromCsv(string filePath)
		{
			try
			{
				if (!File.Exists(filePath))
					throw new FileNotFoundException("CSV dosyası bulunamadı!");

				var config = new CsvConfiguration(CultureInfo.InvariantCulture)
				{
					HasHeaderRecord = true,
					Delimiter = ",",
					MissingFieldFound = null,
					BadDataFound = null
				};

				using var reader = new StreamReader(filePath);
				using var csv = new CsvReader(reader, config);

				var csvRecords = csv.GetRecords<OrderCsvDto>().ToList();

				if (csvRecords.Count == 0)
					throw new Exception("CSV dosyası boş!");

				var orders = csvRecords.Select(r => new Order
				{
					FirstName = r.FirstName,
					LastName = r.LastName,
					ProductName = r.ProductName,
					Price = r.Price,
					Quantity = r.Quantity,
					TotalPrice = r.TotalPrice,
					// CSV'den gelen tarihi UTC'ye çevir
					OrderDate = DateTime.SpecifyKind(
						DateTime.Parse(r.OrderDate.ToString("yyyy-MM-dd HH:mm:ss")),
						DateTimeKind.Utc
					)
				}).ToList();


				await _context.Orders.AddRangeAsync(orders);
				var savedCount = await _context.SaveChangesAsync();

				return savedCount;
			}
			catch (Exception ex)
			{
				throw new Exception($"CSV import hatası: {ex.Message} | Inner: {ex.InnerException?.Message}");
			}
		}
	}
}