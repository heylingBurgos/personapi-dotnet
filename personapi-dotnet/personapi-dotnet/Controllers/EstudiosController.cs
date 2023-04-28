using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.Controllers
{
    public class EstudiosController : Controller
    {
        private readonly PersonaDbContext _context;

        public EstudiosController(PersonaDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene la lista de estudios.
        /// </summary>
        /// <returns>Una vista que muestra la lista de estudios.</returns>
        [HttpGet("IndexEstudios")]
        [ProducesResponseType(typeof(ViewResult), 200)]
        public async Task<IActionResult> Index()
        {
            var persona_dbContext = _context.Estudios.Include(e => e.CcPerNavigation).Include(e => e.IdProfNavigation);
            return View(await persona_dbContext.ToListAsync());
        }

        /// <summary>
        /// Muestra la vista de creación de un estudio.
        /// </summary>
        /// <returns>La vista de creación de un estudio.</returns>
        [HttpGet("CreateEst")]
        [ProducesResponseType(typeof(ViewResult), 200)]
        public IActionResult Create()
        {
            ViewData["CcPer"] = new SelectList(_context.Personas, "Cc", "Cc");
            ViewData["IdProf"] = new SelectList(_context.Profesions, "Id", "Id");
            return View();
        }

        /// <summary>
        /// Crea un nuevo estudio.
        /// </summary>
        /// <param name="estudio">Los datos del estudio a crear.</param>
        /// <returns>Una redirección a la acción "Index" en caso de éxito, o una vista con el modelo en caso contrario.</returns>
        [HttpPost("CreateEst")]
        [ValidateAntiForgeryToken]
        [ProducesResponseType(typeof(RedirectToActionResult), 302)]
        [ProducesResponseType(typeof(ViewResult), 400)]
        public async Task<IActionResult> Create([Bind("IdProf,CcPer,Fecha,Univer")] Estudio estudio)
        {
            if (ModelState.IsValid)
            {
                _context.Add(estudio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CcPer"] = new SelectList(_context.Personas, "Cc", "Cc", estudio.CcPer);
            ViewData["IdProf"] = new SelectList(_context.Profesions, "Id", "Id", estudio.IdProf);
            return View(estudio);
        }

        /// <summary>
        /// Muestra la vista de edición de un estudio.
        /// </summary>
        /// <param name="id">El identificador del estudio.</param>
        /// <returns>La vista de edición del estudio o una vista de error si no se encuentra el estudio.</returns>
        [HttpGet("EditEst/{id}")]
        [ProducesResponseType(typeof(ViewResult), 200)]
        [ProducesResponseType(typeof(ViewResult), 404)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Estudios == null)
            {
                return NotFound();
            }

            var estudio = await _context.Estudios.FindAsync(id);
            if (estudio == null)
            {
                return NotFound();
            }
            ViewData["CcPer"] = new SelectList(_context.Personas, "Cc", "Cc", estudio.CcPer);
            ViewData["IdProf"] = new SelectList(_context.Profesions, "Id", "Id", estudio.IdProf);
            return View(estudio);
        }

        /// <summary>
        /// Actualiza la información de un estudio existente.
        /// </summary>
        /// <param name="id">El identificador del estudio a actualizar.</param>
        /// <param name="estudio">La información actualizada del estudio.</param>
        /// <returns>Una redirección a la acción "Index" en caso de éxito, o una vista con el modelo en caso contrario.</returns>
        [HttpPost("EditEst/{id}")]
        [ValidateAntiForgeryToken]
        [ProducesResponseType(typeof(RedirectToActionResult), 302)]
        [ProducesResponseType(typeof(ViewResult), 400)]
        [ProducesResponseType(typeof(ViewResult), 404)]
        public async Task<IActionResult> Edit(int id, [Bind("IdProf,CcPer,Fecha,Univer")] Estudio estudio)
        {
            if (id != estudio.CcPer)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(estudio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EstudioExists(estudio.CcPer))
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
            ViewData["CcPer"] = new SelectList(_context.Personas, "Cc", "Cc", estudio.CcPer);
            ViewData["IdProf"] = new SelectList(_context.Profesions, "Id", "Id", estudio.IdProf);
            return View(estudio);
        }

        /// <summary>
        /// Muestra la vista de eliminación de un estudio.
        /// </summary>
        /// <param name="id">El identificador del estudio.</param>
        /// <returns>La vista de eliminación del estudio o una vista de error si no se encuentra el estudio.</returns>
        [HttpGet("DeleteEst/{id}")]
        [ProducesResponseType(typeof(ViewResult), 200)]
        [ProducesResponseType(typeof(ViewResult), 404)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Estudios == null)
            {
                return NotFound();
            }

            var estudio = await _context.Estudios
                .Include(e => e.CcPerNavigation)
                .Include(e => e.IdProfNavigation)
                .FirstOrDefaultAsync(m => m.CcPer == id);
            if (estudio == null)
            {
                return NotFound();
            }

            return View(estudio);
        }

        /// <summary>
        /// Confirma la eliminación de un estudio.
        /// </summary>
        /// <param name="id">El identificador del estudio a eliminar.</param>
        /// <returns>Una redirección a la acción "Index" en caso de éxito.</returns>
        [HttpPost("DeleteEst/{id}")]
        [ValidateAntiForgeryToken]
        [ProducesResponseType(typeof(RedirectToActionResult), 302)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Estudios == null)
            {
                return Problem("Entity set 'persona_dbContext.Estudios'  is null.");
            }
            var estudio = await _context.Estudios.FindAsync(id);
            if (estudio != null)
            {
                _context.Estudios.Remove(estudio);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EstudioExists(int id)
        {
            return _context.Estudios.Any(e => e.CcPer == id);
        }

    }
}
