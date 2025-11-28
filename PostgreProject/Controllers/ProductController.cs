using Microsoft.AspNetCore.Mvc;
using PostgreProject.Dtos.ProductDtos;
using PostgreProject.Services;
using PostgreProject.Services.ProductServices;

namespace PostgreProject.Controllers
{
	public class ProductController : Controller
	{
		private readonly IProductService _productService;
		private readonly IAIService _aiService;

		public ProductController(IProductService productService, IAIService aiService)
		{
			_productService = productService;
			_aiService = aiService;
		}
		public async Task<IActionResult> Index()
		{
			var products = await _productService.GetAllAsync();
			return View(products);
		}

		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(CreateProductDto dto)
		{
			if (!ModelState.IsValid)
				return View(dto);

			await _productService.AddAsync(dto);
			return RedirectToAction(nameof(Index));
		}

		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
			var products = await _productService.GetAllAsync();
			var product = products.FirstOrDefault(x => x.Id == id);

			if (product == null) return NotFound();

			var updateDto = new UpdateProductDto
			{
				Id = product.Id,
				ProductName = product.ProductName,
				Description = product.Description,
				ProductPrice = product.ProductPrice,
				ProductStock = product.ProductStock,
				ProductImage = product.ProductImage,
				CategoryId = product.CategoryId
			};

			return View(updateDto);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(UpdateProductDto dto)
		{
			if (!ModelState.IsValid)
				return View(dto);

			await _productService.UpdateAsync(dto);
			return RedirectToAction(nameof(Index));
		}
		public async Task<IActionResult> Delete(int id)
		{
			await _productService.DeleteAsync(id);
			return RedirectToAction(nameof(Index));
		}

		[HttpPost]
		public async Task<IActionResult> GenerateProductDescription(string productName)
		{
			if (string.IsNullOrWhiteSpace(productName))
				return Json(new { success = false, message = "Lütfen bir ürün adı girin." });

			var result = await _aiService.GenerateProductDescription(productName);
			return Json(new { success = true, text = result });
		}
	}
}