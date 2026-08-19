using ECommerce.Domain.Entities.Products;
using System.ComponentModel.DataAnnotations;

namespace AdminDashBoard.Models.Products
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Product name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description name is required")]

        public string Description { get; set; }
        public IFormFile? Image { get; set; }
        public string PictureUrl { get; set; }

        [Required(ErrorMessage = "Product Price is required")]
        [Range(1,30000, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Product brand id is required")]

        public int BrandId { get; set; }
        public string? Brand { get; set; }
        [Required(ErrorMessage = "Product type id is required")]

        public int TypeId  { get; set; }
        public string? Type { get; set; }
    }
}
