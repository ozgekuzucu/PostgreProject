using PostgreProject.Dtos.RecipeDtos;
using PostgreProject.Entities;

namespace PostgreProject.Services.RecipeServices
{
	public interface IRecipeService
	{
		Task<List<ResultRecipeDto>> GetAllAsync();
		Task AddAsync(CreateRecipeDto createAboutDto);
		Task UpdateAsync(UpdateRecipeDto updateAboutDto);
		Task DeleteAsync(int id);
	}
}
