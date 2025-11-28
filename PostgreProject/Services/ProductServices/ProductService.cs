using Microsoft.EntityFrameworkCore;
using PostgreProject.Context;
using PostgreProject.Dtos.ProductDtos;
using PostgreProject.Entities;

namespace PostgreProject.Services.ProductServices
{
	public class ProductService : IProductService
	{
		private readonly AppDbContext _context;

		public ProductService(AppDbContext context)
		{
			_context = context;
		}

		public async Task<List<ResultProductDto>> GetAllAsync()
		{
			var products = await _context.Products
										 .Include(p => p.Category)
										 .Select(p => new ResultProductDto
										 {
											 Id = p.Id,
											 ProductName = p.ProductName,
											 Description = p.Description,
											 ProductPrice = p.ProductPrice,
											 ProductStock = p.ProductStock,
											 ProductImage = p.ProductImage,
											 CategoryId = p.CategoryId,
											 CategoryName = p.Category != null ? p.Category.CategoryName : "Kategori Yok"
										 })
										 .ToListAsync();
			return products;
		}

		public async Task<ResultProductDto> GetByIdAsync(int id)
		{
			var product = await _context.Products
										.Include(p => p.Category)
										.Where(p => p.Id == id)
										.Select(p => new ResultProductDto
										{
											Id = p.Id,
											ProductName = p.ProductName,
											Description = p.Description,
											ProductPrice = p.ProductPrice,
											ProductStock = p.ProductStock,
											ProductImage = p.ProductImage,
											CategoryId = p.CategoryId,
											CategoryName = p.Category != null ? p.Category.CategoryName : "Kategori Yok"
										})
										.FirstOrDefaultAsync();
			return product;
		}

		public async Task AddAsync(CreateProductDto dto)
		{
			var product = new Product
			{
				ProductName = dto.ProductName,
				Description = dto.Description,
				ProductPrice = dto.ProductPrice,
				ProductStock = dto.ProductStock,
				ProductImage = dto.ProductImage,
				CategoryId = dto.CategoryId
			};

			_context.Products.Add(product);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateAsync(UpdateProductDto dto)
		{
			var product = await _context.Products.FindAsync(dto.Id);

			if (product != null)
			{
				product.ProductName = dto.ProductName;
				product.Description = dto.Description;
				product.ProductPrice = dto.ProductPrice;
				product.ProductStock = dto.ProductStock;
				product.ProductImage = dto.ProductImage;
				product.CategoryId = dto.CategoryId;

				_context.Products.Update(product);
				await _context.SaveChangesAsync();
			}
		}

		public async Task DeleteAsync(int id)
		{
			var product = await _context.Products.FindAsync(id);

			if (product != null)
			{
				_context.Products.Remove(product);
				await _context.SaveChangesAsync();
			}
		}
	}
}