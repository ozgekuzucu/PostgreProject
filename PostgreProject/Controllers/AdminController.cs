using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostgreProject.Context;
using PostgreProject.Services;

namespace PostgreProject.Controllers
{
	public class AdminController : Controller
	{
		private readonly CsvImportService _csvImportService;
		private readonly AppDbContext _context;
		private readonly OrderAnalyticsService _analyticsService;
		private readonly SalesForecastService _forecastService;

		public AdminController(CsvImportService csvImportService, AppDbContext context, OrderAnalyticsService analyticsService, SalesForecastService forecastService)
		{
			_csvImportService = csvImportService;
			_context = context;
			_analyticsService = analyticsService;
			_forecastService = forecastService;
		}

		[HttpGet]
		public IActionResult ImportOrders()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> ImportOrders(IFormFile csvFile)
		{
			if (csvFile == null || csvFile.Length == 0)
			{
				ViewBag.Message = "Lütfen bir CSV dosyası seçin.";
				return View();
			}

			var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", csvFile.FileName);

			Directory.CreateDirectory(Path.GetDirectoryName(filePath));

			using (var stream = new FileStream(filePath, FileMode.Create))
			{
				await csvFile.CopyToAsync(stream);
			}

			var count = await _csvImportService.ImportOrdersFromCsv(filePath);

			ViewBag.Message = $"{count} sipariş başarıyla eklendi!";
			return View();
		}

		public async Task<IActionResult> Orders()
		{
			var orders = await _context.Orders
				.OrderByDescending(o => o.OrderDate)
				.Take(100)
				.ToListAsync();

			return View(orders);
		}
		
		public async Task<IActionResult> Forecast()
		{
			var monthlySales = await _analyticsService.GetMonthlySales();
			var topProducts = await _analyticsService.GetTopProducts(6);

			if (monthlySales != null && monthlySales.Any())
			{
				Console.WriteLine($"İlk ay: {monthlySales[0].Month}/{monthlySales[0].Year} - ₺{monthlySales[0].TotalRevenue}");
			}

			if (topProducts != null && topProducts.Any())
			{
				Console.WriteLine($"İlk ürün: {topProducts[0].ProductName} - {topProducts[0].TotalQuantity} adet");
			}

			ViewBag.TotalOrders = await _analyticsService.GetTotalOrdersCount();
			ViewBag.TotalRevenue = await _analyticsService.GetTotalRevenue();
			ViewBag.TopProducts = topProducts;
			ViewBag.MonthlySales = monthlySales;
			try
			{
				var forecast = await _forecastService.ForecastNextThreeMonths();
				var historical = await _forecastService.GetHistoricalMonthlySales();

				ViewBag.Forecast = forecast;
				ViewBag.Historical = historical;

				return View();
			}
			catch (Exception ex)
			{
				ViewBag.Error = ex.Message;
				return View();
			}
		}
	}
}
