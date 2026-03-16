using System.ComponentModel.DataAnnotations;

namespace praktika.Models
{
    public class Category
    {
        [Key]
        public int IdCategory { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public string? Description { get; set; }
    }
}