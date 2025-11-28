using PostgreProject.Dtos.AboutDtos;
using PostgreProject.Entities;

namespace PostgreProject.Services.AboutServices
{
	public interface IAboutService
	{
		Task<List<ResultAboutDto>> GetAllAsync();
		Task AddAsync(CreateAboutDto createAboutDto);
		Task UpdateAsync(UpdateAboutDto updateAboutDto);
		Task DeleteAsync(int id);
	}
}
