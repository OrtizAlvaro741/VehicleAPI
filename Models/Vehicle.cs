using System.ComponentModel.DataAnnotations;

namespace VehicleAPI.Models
{
    public class Vehicle
    {
        [Key]                       // Clave primaria
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required, StringLength(50)]
        public string Make { get; set; } = null!;   // Marca

        [Required, StringLength(50)]
        public string Model { get; set; } = null!;  // Modelo

        [Range(1900, 2100)]
        public int Year { get; set; }               // Año
    }
}