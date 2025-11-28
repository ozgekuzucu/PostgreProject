using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostgreProject.Context;

namespace PostgreProject.ViewComponents
{
	public class _TestimonialComponentPartial : ViewComponent
	{
		private readonly AppDbContext _context;

		public _TestimonialComponentPartial(AppDbContext context)
		{
			_context = context;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var testimonials = await _context.Testimonials.ToListAsync();
			return View(testimonials);
		}
	}
}
