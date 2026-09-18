using Microsoft.EntityFrameworkCore;

namespace barber.Models
{
    public class BarberiaContext : DbContext
    {
        public BarberiaContext(DbContextOptions<BarberiaContext> options)
            : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Servicio> Servicios { get; set; }
        public DbSet<Cita> Citas { get; set; }
    }
}
