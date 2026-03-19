using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace praktika.Models
{
    public class Product
    {
        [Key]
        public int IdProduct { get; set; }

        [Required(ErrorMessage = "Название товара обязательно")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Название должно быть от 2 до 200 символов")]
        public string Name { get; set; }

        [StringLength(2000, ErrorMessage = "Описание не может быть длиннее 2000 символов")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Цена обязательна")]
        [Range(0.01, 9999999, ErrorMessage = "Цена должна быть от 0.01 до 9 999 999")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Range(0.01, 9999999, ErrorMessage = "Старая цена должна быть от 0.01 до 9 999 999")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? OldPrice { get; set; }

        public string? ImagePath { get; set; }
        public bool IsHit { get; set; }
        public bool IsNew { get; set; }
        public bool InStock { get; set; } = true;

        [StringLength(50, ErrorMessage = "Артикул не может быть длиннее 50 символов")]
        public string? Article { get; set; }

        [Required(ErrorMessage = "Выберите категорию")]
        public int IdCategory { get; set; }
        [ForeignKey("IdCategory")]
        public Category? Category { get; set; }

        // Характеристики — отдельные поля
        [StringLength(100, ErrorMessage = "Не более 100 символов")]
        public string? Brand { get; set; }        // Марка/Производитель

        [StringLength(100, ErrorMessage = "Не более 100 символов")]
        public string? Size { get; set; }         // Размер

        [StringLength(100, ErrorMessage = "Не более 100 символов")]
        public string? Weight { get; set; }       // Вес

        [StringLength(100, ErrorMessage = "Не более 100 символов")]
        public string? Material { get; set; }     // Материал

        [StringLength(100, ErrorMessage = "Не более 100 символов")]
        public string? Color { get; set; }        // Цвет

        [StringLength(100, ErrorMessage = "Не более 100 символов")]
        public string? Country { get; set; }      // Страна производства
    }
}