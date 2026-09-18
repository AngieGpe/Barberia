using System.ComponentModel.DataAnnotations;

namespace barber.Models
{
    public class Cliente
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio :<")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El teléfono es obligatorio :<")]
        public string Telefono { get; set; }

    }
}

