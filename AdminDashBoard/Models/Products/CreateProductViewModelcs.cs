namespace AdminDashBoard.Models.Products
{
    public class CreateProductViewModel
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int BrandId { get; set; }
        public int TypeId { get; set; }
        public IFormFile? Image { get; set; }
    }

}
