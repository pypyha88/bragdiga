using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace praktika.Models
{
    public class User
    {
        [Key]
        public int IdUser { get; set; }

        [Required(ErrorMessage = "Логин обязателен")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Логин должен быть от 3 до 30 символов")]
        public string? Login { get; set; }

        [Required(ErrorMessage = "Пароль обязателен")]
        [StringLength(30, MinimumLength = 4, ErrorMessage = "Пароль должен быть от 4 до 30 символов")]
        public string? Password { get; set; }

        [EmailAddress(ErrorMessage = "Некорректный формат email")]
        [StringLength(200, ErrorMessage = "Email не может быть длиннее 200 символов")]
        public string? Email { get; set; }

        public int IdRole { get; set; }
        [ForeignKey("IdRole")]
        public Role? Role { get; set; }
    }
}