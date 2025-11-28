using Microsoft.EntityFrameworkCore;
using PostgreProject.Context;

namespace PostgreProject.Services
{
	public class OrderAnalyticsService
	{
		private readonly AppDbContext _context;

		public OrderAnalyticsService(AppDbContext context)
		{
			_context = context;
		}

		// Toplam sipariş sayısı
		public async Task<int> GetTotalOrdersCount()
		{
			return await _context.Orders.CountAsync();
		}

		// Toplam gelir
		public async Task<decimal> GetTotalRevenue()
		{
			return await _context.Orders.SumAsync(o => o.TotalPrice);
		}

		// En çok satan 5 ürün (son 6 ay)
		public async Task<List<TopProductDto>> GetTopProducts(int months = 6)
		{
			var startDate = DateTime.UtcNow.AddMonths(-months); 

			var topProducts = await _context.Orders
				.Where(o => o.OrderDate >= startDate)
				.GroupBy(o => o.ProductName)
				.Select(g => new TopProductDto
				{
					ProductName = g.Key,
					TotalQuantity = g.Sum(o => o.Quantity),
					TotalRevenue = g.Sum(o => o.TotalPrice)
				})
				.OrderByDescending(p => p.TotalQuantity)
				.Take(5)
				.ToListAsync();

			if (!topProducts.Any())
			{
				return new List<TopProductDto> {
			new TopProductDto { ProductName = "Veri Yok", TotalQuantity = 0, TotalRevenue = 0 }
		};
			}

			return topProducts;
		}
		// Aylık satış özeti
		public async Task<List<MonthlySalesDto>> GetMonthlySales()
		{
			var monthlySales = await _context.Orders
				.GroupBy(o => new { o.OrderDate.Year, o.OrderDate.Month })
				.Select(g => new MonthlySalesDto
				{
					Year = g.Key.Year,
					Month = g.Key.Month,
					TotalOrders = g.Count(),
					TotalRevenue = g.Sum(o => o.TotalPrice)
				})
				.OrderBy(m => m.Year).ThenBy(m => m.Month)
				.ToListAsync();

			return monthlySales;
		}
	}

	public class TopProductDto
	{
		public string ProductName { get; set; }
		public int TotalQuantity { get; set; }
		public decimal TotalRevenue { get; set; }
	}

	public class MonthlySalesDto
	{
		public int Year { get; set; }
		public int Month { get; set; }
		public int TotalOrders { get; set; }
		public decimal TotalRevenue { get; set; }
	}
}