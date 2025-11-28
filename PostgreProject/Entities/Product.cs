namespace PostgreProject.Entities
{
	public class Product
	{
		public int Id { get; set; }
		public string ProductName { get; set; }
		public decimal ProductPrice { get; set; }
		public string Description { get; set; }
		public string ProductImage { get; set; }
		public int ProductStock { get; set; }
		public int CategoryId { get; set; }
		public Category Category { get; set; }
	}
}
