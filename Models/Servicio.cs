using System.ComponentModel.DataAnnotations;

namespace barber.Models
{
    public class Servicio
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del servicio es obligatorio :<")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio :<")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "La duración es obligatoria :<")]
        public int Duracion { get; set; }
    }
}

