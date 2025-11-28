using PostgreProject.Dtos.ProductDtos;

namespace PostgreProject.Services.ProductServices
{
	public interface IProductService
	{
		Task<List<ResultProductDto>> GetAllAsync();
		Task<ResultProductDto> GetByIdAsync(int id);
		Task AddAsync(CreateProductDto dto);
		Task UpdateAsync(UpdateProductDto dto);
		Task DeleteAsync(int id);
	}
}