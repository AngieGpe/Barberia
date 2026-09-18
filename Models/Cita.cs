using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace barber.Models
{
    public class Cita
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Selecciona un cliente")]
        [Display(Name = "Cliente")]
        public int ClienteId { get; set; }

        [Required(ErrorMessage = "Selecciona un servicio")]
        [Display(Name = "Servicio")]
        public int ServicioId { get; set; }

        [Required(ErrorMessage = "La fecha es obligatoria")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "La hora es obligatoria")]
        [DataType(DataType.Time)]
        [Display(Name = "Hora")]
        public TimeSpan Hora { get; set; }

        [Display(Name = "Estado")]
        public string Estado { get; set; } = "Pendiente";

        [ForeignKey("ClienteId")]
        public Cliente? Cliente { get; set; }

        [ForeignKey("ServicioId")]
        public Servicio? Servicio { get; set; }
    }
}

