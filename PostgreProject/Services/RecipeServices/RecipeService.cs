using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PostgreProject.Context;
using PostgreProject.Dtos.RecipeDtos;
using PostgreProject.Entities;

namespace PostgreProject.Services.RecipeServices
{
	public class RecipeService : IRecipeService
	{
		private readonly AppDbContext _context;
		private readonly IMapper _mapper;

		public RecipeService(AppDbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public async Task AddAsync(CreateRecipeDto createAboutDto)
		{
			var about = _mapper.Map<About>(createAboutDto);
			await _context.Abouts.AddAsync(about);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteAsync(int id)
		{
			var about = await _context.Abouts.FindAsync(id);
			if (about == null) return;

			_context.Abouts.Remove(about);
			await _context.SaveChangesAsync();
		}
		public async Task<List<ResultRecipeDto>> GetAllAsync()
		{
			var abouts = await _context.Abouts.OrderBy(x => x.Id).ToListAsync();
			return _mapper.Map<List<ResultRecipeDto>>(abouts);
		}

		public async Task UpdateAsync(UpdateRecipeDto updateAboutDto)
		{
			var about = await _context.Abouts.FindAsync(updateAboutDto.Id);
			if (about == null) return;

			_mapper.Map(updateAboutDto, about);
			await _context.SaveChangesAsync();
		}
	}
}
