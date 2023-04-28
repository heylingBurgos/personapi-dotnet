using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using personapi_dotnet.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace personapi_dotnet.Controllers
{
    public class TelefonoController : Controller
    {
        private readonly PersonaDbContext _context;

        public TelefonoController(PersonaDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene la lista de teléfonos con sus respectivos dueños.
        /// </summary>
        /// <returns>Una vista que muestra la lista de teléfonos con sus dueños.</returns>
        [HttpGet("IndexTelefono")]
        [ProducesResponseType(typeof(ViewResult), 200)]
        public async Task<IActionResult> Index()
        {
            var persona_dbContext = _context.Telefonos.Include(t => t.DuenioNavigation);
            return View(await persona_dbContext.ToListAsync());
        }

        /// <summary>
        /// Muestra la vista de creación de un teléfono.
        /// </summary>
        /// <returns>La vista de creación de un teléfono.</returns>
        [HttpGet("CreateTelefono")]
        [ProducesResponseType(typeof(ViewResult), 200)]
        public IActionResult Create()
        {
            ViewData["Duenio"] = new SelectList(_context.Personas, "Cc", "Cc");
            return View();
        }

        /// <summary>
        /// Crea un nuevo teléfono.
        /// </summary>
        /// <param name="telefono">La información del teléfono a crear.</param>
        /// <returns>Una redirección a la acción "Index" en caso de éxito, o una vista con el modelo en caso contrario.</returns>
        [HttpPost("CreateTelefono")]
        [ValidateAntiForgeryToken]
        [ProducesResponseType(typeof(RedirectToActionResult), 302)]
        [ProducesResponseType(typeof(ViewResult), 400)]
        public async Task<IActionResult> Create([Bind("Num,Oper,Duenio")] Telefono telefono)
        {
            if (ModelState.IsValid)
            {
                _context.Add(telefono);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Duenio"] = new SelectList(_context.Personas, "Cc", "Cc", telefono.Duenio);
            return View(telefono);
        }

        /// <summary>
        /// Muestra la vista de edición de un teléfono.
        /// </summary>
        /// <param name="id">El número del teléfono.</param>
        /// <returns>La vista de edición del teléfono o una vista de error si no se encuentra el teléfono.</returns>
        [HttpGet("EditTel/{id}")]
        [ProducesResponseType(typeof(ViewResult), 200)]
        [ProducesResponseType(typeof(ViewResult), 404)]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Telefonos == null)
            {
                return NotFound();
            }

            var telefono = await _context.Telefonos.FindAsync(id);
            if (telefono == null)
            {
                return NotFound();
            }
            ViewData["Duenio"] = new SelectList(_context.Personas, "Cc", "Cc", telefono.Duenio);
            return View(telefono);
        }

        /// <summary>
        /// Actualiza la información de un teléfono existente.
        /// </summary>
        /// <param name="id">El número del teléfono a actualizar.</param>
        /// <param name="telefono">La información actualizada del teléfono.</param>
        /// <returns>Una redirección a la acción "Index" en caso de éxito, o una vista con el modelo en caso contrario.</returns>
        [HttpPost("EditTel/{id}")]
        [ValidateAntiForgeryToken]
        [ProducesResponseType(typeof(RedirectToActionResult), 302)]
        [ProducesResponseType(typeof(ViewResult), 400)]
        [ProducesResponseType(typeof(ViewResult), 404)]
        public async Task<IActionResult> Edit(string id, [Bind("Num,Oper,Duenio")] Telefono telefono)
        {
            if (id != telefono.Num)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(telefono);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TelefonoExists(telefono.Num))
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
            ViewData["Duenio"] = new SelectList(_context.Personas, "Cc", "Cc", telefono.Duenio);
            return View(telefono);
        }

        /// <summary>
        /// Muestra la vista de eliminación de un teléfono.
        /// </summary>
        /// <param name="id">El número del teléfono.</param>
        /// <returns>La vista de eliminación del teléfono o una vista de error si no se encuentra el teléfono.</returns>
        [HttpGet("DeleteTel/{id}")]
        [ProducesResponseType(typeof(ViewResult), 200)]
        [ProducesResponseType(typeof(ViewResult), 404)]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Telefonos == null)
            {
                return NotFound();
            }

            var telefono = await _context.Telefonos
                .Include(t => t.DuenioNavigation)
                .FirstOrDefaultAsync(m => m.Num == id);
            if (telefono == null)
            {
                return NotFound();
            }

            return View(telefono);
        }

        /// <summary>
        /// Confirma la eliminación de un teléfono.
        /// </summary>
        /// <param name="id">El número del teléfono a eliminar.</param>
        /// <returns>Una redirección a la acción "Index" en caso de éxito.</returns>
        [HttpPost("DeleteTel/{id}")]
        [ValidateAntiForgeryToken]
        [ProducesResponseType(typeof(RedirectToActionResult), 302)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Telefonos == null)
            {
                return Problem("Entity set 'persona_dbContext.Telefonos'  is null.");
            }
            var telefono = await _context.Telefonos.FindAsync(id);
            if (telefono != null)
            {
                _context.Telefonos.Remove(telefono);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TelefonoExists(string id)
        {
            return _context.Telefonos.Any(e => e.Num == id);
        }

    }
}
