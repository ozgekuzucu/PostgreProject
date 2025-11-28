using Microsoft.AspNetCore.Mvc;
using PostgreProject.Context;
using PostgreProject.Entities;

namespace PostgreProject.Controllers
{
	public class CategoryController : Controller
	{
		private readonly AppDbContext _context;

		public CategoryController(AppDbContext context)
		{
			_context = context;
		}

		public IActionResult Index()
		{
			var Categorys = _context.Categories.ToList();
			return View(Categorys);
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Create(Category model)
		{
			if (!ModelState.IsValid)
				return View(model);

			_context.Categories.Add(model);
			_context.SaveChanges();

			return RedirectToAction("Index");
		}

		public IActionResult Edit(int id)
		{
			var Category = _context.Categories.Find(id);
			if (Category == null)
				return NotFound();

			return View(Category);
		}

		[HttpPost]
		public IActionResult Edit(Category model)
		{
			if (!ModelState.IsValid)
				return View(model);

			_context.Categories.Update(model);
			_context.SaveChanges();

			return RedirectToAction("Index");
		}


		public IActionResult Delete(int id)
		{
			var Category = _context.Categories.Find(id);
			if (Category == null)
				return NotFound();

			_context.Categories.Remove(Category);
			_context.SaveChanges();

			return RedirectToAction("Index");
		}
	}
}

