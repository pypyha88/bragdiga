using System.ComponentModel.DataAnnotations;

namespace praktika.Models
{
    public class Role
    {
        [Key]
        public int IdRole { get; set; }
        [Required]
        [StringLength(50)]
        public string? Name { get; set; }

        public ICollection<User>? Users { get; set; }
    }
}
