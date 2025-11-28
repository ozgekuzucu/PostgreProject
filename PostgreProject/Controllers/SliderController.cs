using Microsoft.AspNetCore.Mvc;
using PostgreProject.Context;
using PostgreProject.Entities;

namespace PostgreProject.Controllers
{
	public class SliderController : Controller
	{
		private readonly AppDbContext _context;

		public SliderController(AppDbContext context)
		{
			_context = context;
		}

		public IActionResult Index()
		{
			var sliders = _context.Sliders.ToList();
			return View(sliders);
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Create(Slider model)
		{
			if (!ModelState.IsValid)
				return View(model);

			_context.Sliders.Add(model);
			_context.SaveChanges();

			return RedirectToAction("Index");
		}

		public IActionResult Edit(int id)
		{
			var slider = _context.Sliders.Find(id);
			if (slider == null)
				return NotFound();

			return View(slider);
		}

		[HttpPost]
		public IActionResult Edit(Slider model)
		{
			if (!ModelState.IsValid)
				return View(model);

			_context.Sliders.Update(model);
			_context.SaveChanges();

			return RedirectToAction("Index");
		}

		public IActionResult Delete(int id)
		{
			var slider = _context.Sliders.Find(id);
			if (slider == null)
				return NotFound();

			_context.Sliders.Remove(slider);
			_context.SaveChanges();

			return RedirectToAction("Index");
		}
	}
}

