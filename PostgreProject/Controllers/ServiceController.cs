using Microsoft.AspNetCore.Mvc;
using PostgreProject.Context;
using PostgreProject.Entities;

namespace PostgreProject.Controllers
{
	public class ServiceController : Controller
	{
		private readonly AppDbContext _context;

		public ServiceController(AppDbContext context)
		{
			_context = context;
		}

		public IActionResult Index()
		{
			var services = _context.Services.ToList();
			return View(services);
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Create(Service model)
		{
			if (!ModelState.IsValid)
				return View(model);

			_context.Services.Add(model);
			_context.SaveChanges();

			return RedirectToAction("Index");
		}

		public IActionResult Edit(int id)
		{
			var service = _context.Services.Find(id);
			if (service == null)
				return NotFound();

			return View(service);
		}

		[HttpPost]
		public IActionResult Edit(Service model)
		{
			if (!ModelState.IsValid)
				return View(model);

			_context.Services.Update(model);
			_context.SaveChanges();

			return RedirectToAction("Index");
		}

		public IActionResult Delete(int id)
		{
			var service = _context.Services.Find(id);
			if (service == null)
				return NotFound();

			_context.Services.Remove(service);
			_context.SaveChanges();

			return RedirectToAction("Index");
		}
	}
}
