using System.ComponentModel.DataAnnotations.Schema;

namespace BoutiqueLoginRegister.Models
{
    public class Product
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }

        public Category? Category { get; set; }

        public string? ImageName { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
