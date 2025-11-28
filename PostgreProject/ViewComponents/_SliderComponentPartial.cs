using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostgreProject.Context;

namespace PostgreProject.ViewComponents
{
	public class _SliderComponentPartial : ViewComponent
	{
		private readonly AppDbContext _context;

		public _SliderComponentPartial(AppDbContext context)
		{
			_context = context;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var sliders = await _context.Sliders.ToListAsync();
			return View(sliders);
		}
	}
}
