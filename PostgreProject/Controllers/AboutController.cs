using Microsoft.AspNetCore.Mvc;
using PostgreProject.Dtos.AboutDtos;
using PostgreProject.Services;
using PostgreProject.Services.AboutServices;

namespace PostgreProject.Controllers
{
	public class AboutController : Controller
	{
		private readonly IAboutService _aboutService;
		private readonly IAIService _aiService;

		public AboutController(IAboutService aboutService, IAIService aiService)
		{
			_aboutService = aboutService;
			_aiService = aiService;
		}

		public async Task<IActionResult> Index()
		{
			var abouts = await _aboutService.GetAllAsync();
			return View(abouts);
		}

		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(CreateAboutDto dto)
		{
			if (!ModelState.IsValid)
				return View(dto);

			await _aboutService.AddAsync(dto);
			return RedirectToAction(nameof(Index));
		}

		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
			var abouts = await _aboutService.GetAllAsync();
			var about = abouts.FirstOrDefault(x => x.Id == id);
			if (about == null) return NotFound();

			var updateDto = new UpdateAboutDto
			{
				Id = about.Id,
				Title = about.Title,
				SubTitle = about.SubTitle,
				Description = about.Description,
				ImageUrl1 = about.ImageUrl1,
				ImageUrl2 = about.ImageUrl2,
				Feature1 = about.Feature1,
				Feature2 = about.Feature2,
				Feature3 = about.Feature3,
				Feature4 = about.Feature4
			};
			return View(updateDto);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(UpdateAboutDto dto)
		{
			if (!ModelState.IsValid)
				return View(dto);

			await _aboutService.UpdateAsync(dto);
			return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> Delete(int id)
		{
			await _aboutService.DeleteAsync(id);
			return RedirectToAction(nameof(Index));
		}

		[HttpPost]
		public async Task<IActionResult> GenerateAIText(string prompt)
		{
			if (string.IsNullOrWhiteSpace(prompt))
				return Json(new { success = false, message = "Lütfen bir konu girin." });

			var result = await _aiService.GenerateAboutText(prompt);
			return Json(new { success = true, text = result });
		}
	}
}

