using System.ComponentModel.DataAnnotations;

namespace PieShop.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string? Description { get; set; }
        public List<Pie>? Pies { get; set; }
    }
}