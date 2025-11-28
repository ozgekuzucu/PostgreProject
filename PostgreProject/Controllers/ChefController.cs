using Microsoft.AspNetCore.Mvc;
using PostgreProject.Context;
using PostgreProject.Entities;

namespace PostgreProject.Controllers
{
	public class ChefController : Controller
	{
		private readonly AppDbContext _context;

		public ChefController(AppDbContext context)
		{
			_context = context;
		}

		public IActionResult Index()
		{
			var chefs = _context.Chefs.ToList();
			return View(chefs);
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Create(Chef model)
		{
			if (!ModelState.IsValid)
				return View(model);

			_context.Chefs.Add(model);
			_context.SaveChanges();

			return RedirectToAction("Index");
		}

		public IActionResult Edit(int id)
		{
			var chef = _context.Chefs.Find(id);
			if (chef == null)
				return NotFound();

			return View(chef);
		}

		[HttpPost]
		public IActionResult Edit(Chef model)
		{
			if (!ModelState.IsValid)
				return View(model);

			_context.Chefs.Update(model);
			_context.SaveChanges();

			return RedirectToAction("Index");
		}

		public IActionResult Delete(int id)
		{
			var chef = _context.Chefs.Find(id);
			if (chef == null)
				return NotFound();

			_context.Chefs.Remove(chef);
			_context.SaveChanges();

			return RedirectToAction("Index");
		}
	}
}
