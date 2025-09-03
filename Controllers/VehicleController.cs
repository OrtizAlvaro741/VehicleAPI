using Microsoft.AspNetCore.Mvc;
using VehicleAPI.Models;

namespace VehicleAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // -> api/vehicles
    public class VehiclesController : ControllerBase
    {
        // Persistencia en memoria (dura mientras corre la app)
        private static readonly List<Vehicle> Data = new();

        // GET: api/vehicles?make=Ford&year=2020
        [HttpGet]
        public ActionResult<IEnumerable<Vehicle>> Get(string? make, int? year)
        {
            var result = Data.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(make))
                result = result.Where(v => v.Make.Contains(make, StringComparison.OrdinalIgnoreCase));

            if (year.HasValue && year > 0)
                result = result.Where(v => v.Year == year);

            return Ok(result.ToList());
        }

        // GET: api/vehicles/{id}
        [HttpGet("{id:guid}")]
        public ActionResult<Vehicle> GetById(Guid id)
        {
            var vehicle = Data.FirstOrDefault(v => v.Id == id);
            if (vehicle is null) return NotFound();
            return Ok(vehicle);
        }

        // POST: api/vehicles
        [HttpPost]
        public ActionResult<Vehicle> Create([FromBody] Vehicle vehicle)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            // Si tu propiedad Id es { get; set; }:
            vehicle.Id = Guid.NewGuid();

            // Si tu propiedad Id es { get; init; }, usa:
            // vehicle = new Vehicle { Id = Guid.NewGuid(), Make = vehicle.Make, Model = vehicle.Model, Year = vehicle.Year };

            Data.Add(vehicle);
            return CreatedAtAction(nameof(GetById), new { id = vehicle.Id }, vehicle);
        }

        // PUT: api/vehicles/{id}
        [HttpPut("{id:guid}")]
        public IActionResult Replace(Guid id, [FromBody] Vehicle vehicle)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var existing = Data.FirstOrDefault(v => v.Id == id);
            if (existing is null) return NotFound();

            // (Opcional) si en el body viene un Id distinto, forzamos a usar el de la ruta:
            // vehicle.Id = id;

            existing.Make  = vehicle.Make;
            existing.Model = vehicle.Model;
            existing.Year  = vehicle.Year;

            return NoContent();
        }

        // DELETE: api/vehicles/{id}
        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var existing = Data.FirstOrDefault(v => v.Id == id);
            if (existing is null) return NotFound();

            Data.Remove(existing);
            return NoContent();
        }
    }
}
