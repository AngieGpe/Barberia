using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using barber.Models;

namespace barber.Controllers
{
    public class CitasController : Controller
    {
        private readonly BarberiaContext _context;

        public CitasController(BarberiaContext context)
        {
            _context = context;
        }

        // GET: Citas
        public async Task<IActionResult> Index()
        {
            var barberiaContext = _context.Citas.Include(c => c.Cliente).Include(c => c.Servicio);
            return View(await barberiaContext.ToListAsync());
        }

        // GET: Citas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cita = await _context.Citas
                .Include(c => c.Cliente)
                .Include(c => c.Servicio)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cita == null)
            {
                return NotFound();
            }

            return View(cita);
        }

        // GET: Citas/Create
        public IActionResult Create()
        {
            ViewData["ServicioId"] = new SelectList(_context.Servicios, "Id", "Nombre");
            return View();
        }

        // POST: Citas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(
            string NombreCliente,
            string TelefonoCliente,
            int ServicioId,
            DateTime Fecha,
            TimeSpan Hora)
        {

            // Validar que sea de lunes a sábado
            if (Fecha.DayOfWeek == DayOfWeek.Sunday)
            {
                ModelState.AddModelError("Fecha", "La barbería no abre los domingos.");
            }

            // Validar que la hora esté entre 4:00 pm y 8:00
            if (Hora < new TimeSpan(16, 0, 0) || Hora > new TimeSpan(20, 0, 0))
            {
                ModelState.AddModelError("Hora", "El horario de atención es de 4:00 pm a 8:00 pm.");
            }
            // Verificar si ya existe una cita en esa fecha y hora
            var existe = await _context.Citas
                .AnyAsync(c => c.Fecha == Fecha && c.Hora == Hora);

            if (existe)
            {
                ModelState.AddModelError("Hora", "Esa hora ya está apartada, elige otra.");
            }


            // Validar que el cliente haya puesto nombre y teléfono
            if (string.IsNullOrWhiteSpace(NombreCliente))
            {
                ModelState.AddModelError("NombreCliente", "Escribe tu nombre.");
            }
            if (string.IsNullOrWhiteSpace(TelefonoCliente))
            {
                ModelState.AddModelError("TelefonoCliente", "Escribe tu teléfono.");
            }

            if (ModelState.IsValid)
            {
                // Buscar si el cliente ya existe por teléfono
                var cliente = await _context.Clientes
                    .FirstOrDefaultAsync(c => c.Telefono == TelefonoCliente);

                // Si no existe, crearlo
                if (cliente == null)
                {
                    cliente = new Cliente
                    {
                        Nombre = NombreCliente,
                        Telefono = TelefonoCliente
                    };
                    _context.Clientes.Add(cliente);
                    await _context.SaveChangesAsync();
                }

                // Crear la cita
                var cita = new Cita
                {
                    ClienteId = cliente.Id,
                    ServicioId = ServicioId,
                    Fecha = Fecha,
                    Hora = Hora,
                    Estado = "Pendiente"
                };

                _context.Citas.Add(cita);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Confirmacion));
            }

            ViewData["ServicioId"] = new SelectList(_context.Servicios, "Id", "Nombre", ServicioId);
            return View(new Cita { Fecha = Fecha, Hora = Hora });
        }

        // GET: Citas/Confirmacion
        public IActionResult Confirmacion()
        {
            return View();
        }

        // GET: Citas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cita = await _context.Citas.FindAsync(id);
            if (cita == null)
            {
                return NotFound();
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Nombre", cita.ClienteId);
            ViewData["ServicioId"] = new SelectList(_context.Servicios, "Id", "Nombre", cita.ServicioId);
            return View(cita);
        }

        // POST: Citas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ClienteId,ServicioId,Fecha,Hora,Estado")] Cita cita)
        {
            if (id != cita.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cita);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CitaExists(cita.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Nombre", cita.ClienteId);
            ViewData["ServicioId"] = new SelectList(_context.Servicios, "Id", "Nombre", cita.ServicioId);
            return View(cita);
        }

        // GET: Citas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cita = await _context.Citas
                .Include(c => c.Cliente)
                .Include(c => c.Servicio)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cita == null)
            {
                return NotFound();
            }

            return View(cita);
        }

        // POST: Citas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cita = await _context.Citas.FindAsync(id);
            if (cita != null)
            {
                _context.Citas.Remove(cita);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CitaExists(int id)
        {
            return _context.Citas.Any(e => e.Id == id);
        }
    }
}
