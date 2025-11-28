namespace PostgreProject.Entities
{
	public class Category
	{
		public Category()
		{
			Products = new List<Product>();
		}

		public int Id { get; set; }
		public string CategoryName { get; set; }
		public string ImageUrl { get; set; }
		public List<Product> Products { get; set; }
	}
}
