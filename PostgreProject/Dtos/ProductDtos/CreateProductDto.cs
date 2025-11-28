using PostgreProject.Entities;

namespace PostgreProject.Dtos.ProductDtos
{
	public class CreateProductDto
	{
		public string ProductName { get; set; }
		public decimal ProductPrice { get; set; }
		public string Description { get; set; }
		public string ProductImage { get; set; }
		public int ProductStock { get; set; }
		public int CategoryId { get; set; }
	}
}
