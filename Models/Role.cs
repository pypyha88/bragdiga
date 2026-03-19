using System.ComponentModel.DataAnnotations;

namespace praktika.Models
{
    public class Role
    {
        [Key]
        public int IdRole { get; set; }

        [Required(ErrorMessage = "Название роли обязательно")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Название роли должно быть от 2 до 50 символов")]
        public string? Name { get; set; }

        public ICollection<User>? Users { get; set; }
    }
}