using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.Controllers
{
    public class ProfesionController : Controller
    {

        private readonly PersonaDbContext _context;

        public ProfesionController(PersonaDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene la lista de profesiones.
        /// </summary>
        /// <returns>Una vista que muestra la lista de profesiones.</returns>
        [HttpGet("IndexProf")]
        [ProducesResponseType(typeof(ViewResult), 200)]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Profesions.ToListAsync());
        }

        /// <summary>
        /// Muestra la vista de creación de una profesión.
        /// </summary>
        /// <returns>La vista de creación de una profesión.</returns>
        [HttpGet("CreateProf")]
        [ProducesResponseType(typeof(ViewResult), 200)]
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Crea una nueva profesión.
        /// </summary>
        /// <param name="profesion">La información de la profesión a crear.</param>
        /// <returns>Una redirección a la acción "Index" en caso de éxito, o una vista con el modelo en caso contrario.</returns>
        [HttpPost("CreateProf")]
        [ValidateAntiForgeryToken]
        [ProducesResponseType(typeof(RedirectToActionResult), 302)]
        [ProducesResponseType(typeof(ViewResult), 400)]
        public async Task<IActionResult> Create([Bind("Id,Nom,Des")] Profesion profesion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(profesion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(profesion);
        }

        /// <summary>
        /// Muestra la vista de edición de una profesión.
        /// </summary>
        /// <param name="id">El identificador de la profesión.</param>
        /// <returns>La vista de edición de la profesión o una vista de error si no se encuentra la profesión.</returns>
        [HttpGet("EditProf/{id}")]
        [ProducesResponseType(typeof(ViewResult), 200)]
        [ProducesResponseType(typeof(ViewResult), 404)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Profesions == null)
            {
                return NotFound();
            }

            var profesion = await _context.Profesions.FindAsync(id);
            if (profesion == null)
            {
                return NotFound();
            }
            return View(profesion);
        }

        /// <summary>
        /// Actualiza la información de una profesión existente.
        /// </summary>
        /// <param name="id">El identificador de la profesión a actualizar.</param>
        /// <param name="profesion">La información actualizada de la profesión.</param>
        /// <returns>Una redirección a la acción "Index" en caso de éxito, o una vista con el modelo en caso contrario.</returns>
        [HttpPost("EditProf/{id}")]
        [ValidateAntiForgeryToken]
        [ProducesResponseType(typeof(RedirectToActionResult), 302)]
        [ProducesResponseType(typeof(ViewResult), 400)]
        [ProducesResponseType(typeof(ViewResult), 404)]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nom,Des")] Profesion profesion)
        {
            if (id != profesion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(profesion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProfesionExists(profesion.Id))
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
            return View(profesion);
        }

        /// <summary>
        /// Muestra la vista de eliminación de una profesión.
        /// </summary>
        /// <param name="id">El identificador de la profesión.</param>
        /// <returns>La vista de eliminación de la profesión o una vista de error si no se encuentra la profesión.</returns>
        [HttpGet("DeleteProf/{id}")]
        [ProducesResponseType(typeof(ViewResult), 200)]
        [ProducesResponseType(typeof(ViewResult), 404)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Profesions == null)
            {
                return NotFound();
            }

            var profesion = await _context.Profesions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (profesion == null)
            {
                return NotFound();
            }

            return View(profesion);
        }

        /// <summary>
        /// Confirma la eliminación de una profesión.
        /// </summary>
        /// <param name="id">El identificador de la profesión a eliminar.</param>
        /// <returns>Una redirección a la acción "Index" en caso de éxito.</returns>
        [HttpPost("DeleteProf/{id}")]
        [ValidateAntiForgeryToken]
        [ProducesResponseType(typeof(RedirectToActionResult), 302)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Profesions == null)
            {
                return Problem("Entity set 'persona_dbContext.Profesions'  is null.");
            }
            var profesion = await _context.Profesions.FindAsync(id);
            if (profesion != null)
            {
                _context.Profesions.Remove(profesion);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProfesionExists(int id)
        {
            return _context.Profesions.Any(e => e.Id == id);
        }

    }
}
