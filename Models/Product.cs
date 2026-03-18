using Microsoft.EntityFrameworkCore.Migrations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace praktika.Models
{
    public class Product
    {
        [Key]
        public int IdProduct { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        public string? Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? OldPrice { get; set; }

        public string? ImagePath { get; set; }

        public bool IsHit { get; set; }
        public bool IsNew { get; set; }
        public bool InStock { get; set; } = true;

        public string? Article { get; set; }

        public int IdCategory { get; set; }
        [ForeignKey("IdCategory")]
        public Category? Category { get; set; }
        public string? Specifications { get; set; } // JSON с характеристиками
    }
}