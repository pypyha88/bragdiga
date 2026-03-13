using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.X509Certificates;

namespace praktika.Models
{
    public class User
    {
        [Key]
        public int IdUser { get; set; }

        [Required]
        [StringLength(100)]
        public string? Login { get; set; }

        [Required]
        [StringLength(100)]
        public string? Password { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public int IdRole { get; set; }

        [ForeignKey("IdRole")]
        public Role? Role { get; set; }
    }
}