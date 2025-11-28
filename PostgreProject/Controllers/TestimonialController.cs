using Microsoft.AspNetCore.Mvc;
using PostgreProject.Context;
using PostgreProject.Entities;

namespace PostgreProject.Controllers
{
	public class TestimonialController : Controller
	{
		private readonly AppDbContext _context;

		public TestimonialController(AppDbContext context)
		{
			_context = context;
		}

		public IActionResult Index()
		{
			var testimonials = _context.Testimonials.ToList();
			return View(testimonials);
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Create(Testimonial model)
		{
			if (!ModelState.IsValid)
				return View(model);

			_context.Testimonials.Add(model);
			_context.SaveChanges();

			return RedirectToAction("Index");
		}

		public IActionResult Edit(int id)
		{
			var testimonial = _context.Testimonials.Find(id);
			if (testimonial == null)
				return NotFound();

			return View(testimonial);
		}

		[HttpPost]
		public IActionResult Edit(Testimonial model)
		{
			if (!ModelState.IsValid)
				return View(model);

			_context.Testimonials.Update(model);
			_context.SaveChanges();

			return RedirectToAction("Index");
		}

		public IActionResult Delete(int id)
		{
			var testimonial = _context.Testimonials.Find(id);
			if (testimonial == null)
				return NotFound();

			_context.Testimonials.Remove(testimonial);
			_context.SaveChanges();

			return RedirectToAction("Index");
		}
	}
}
