using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using personapi_dotnet.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace personapi_dotnet.Controllers
{
    public class PersonaController : Controller
    {

        private readonly PersonaDbContext _context;

        public PersonaController(PersonaDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene la lista de personas.
        /// </summary>
        /// <returns>Una lista de personas.</returns>
        [HttpGet("IndexPersona")]
        [ProducesResponseType(typeof(List<Persona>), 200)]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Personas.ToListAsync());
        } 

        /// <summary>
        /// Muestra la vista de creación de una persona.
        /// </summary>
        /// <returns>La vista de creación de una persona.</returns>
        [HttpGet("CreatePer")]
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Crea una nueva persona.
        /// </summary>
        /// <param name="persona">La información de la persona a crear.</param>
        /// <returns>Una redirección a la acción "Index" en caso de éxito, o una vista con el modelo en caso contrario.</returns>
        [HttpPost("CreatePer")]
        [ValidateAntiForgeryToken]
        [ProducesResponseType(typeof(RedirectToActionResult), 302)]
        [ProducesResponseType(typeof(ViewResult), 400)]
        public async Task<IActionResult> Create([Bind("Cc,Nombre,Apellido,Genero,Edad")] Persona persona)
        {
            if (ModelState.IsValid)
            {
                _context.Add(persona);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(persona);
        }

        /// <summary>
        /// Muestra la vista de edición de una persona.
        /// </summary>
        /// <param name="id">El identificador de la persona.</param>
        /// <returns>La vista de edición de la persona o una vista de error si no se encuentra la persona.</returns>
        [HttpGet("Edit/{id}")]
        [ProducesResponseType(typeof(ViewResult), 200)]
        [ProducesResponseType(typeof(ViewResult), 404)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Personas == null)
            {
                return NotFound();
            }

            var persona = await _context.Personas.FindAsync(id);
            if (persona == null)
            {
                return NotFound();
            }
            return View(persona);
        }

        /// <summary>
        /// Actualiza una persona existente.
        /// </summary>
        /// <param name="id">El identificador de la persona.</param>
        /// <param name="persona">La información actualizada de la persona.</param>
        /// <returns>Una redirección a la acción "Index" en caso de éxito, o una vista con el modelo en caso contrario.</returns>
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        [ProducesResponseType(typeof(RedirectToActionResult), 302)]
        [ProducesResponseType(typeof(ViewResult), 400)]
        [ProducesResponseType(typeof(ViewResult), 404)]
        public async Task<IActionResult> Edit(int id, [Bind("Cc,Nombre,Apellido,Genero,Edad")] Persona persona)
        {
            if (id != persona.Cc)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(persona);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonaExists(persona.Cc))
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
            return View(persona);
        }

        /// <summary>
        /// Elimina una persona por su identificador.
        /// </summary>
        /// <param name="id">El identificador de la persona a eliminar.</param>
        /// <returns>La vista de la persona a eliminar en caso de éxito, o una vista de error en caso contrario.</returns>
        [HttpGet("Delete/{id}")]
        [ProducesResponseType(typeof(ViewResult), 200)]
        [ProducesResponseType(typeof(ViewResult), 404)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Personas == null)
            {
                return NotFound();
            }

            var persona = await _context.Personas
                .FirstOrDefaultAsync(m => m.Cc == id);
            if (persona == null)
            {
                return NotFound();
            }

            return View(persona);
        }

        /// <summary>
        /// Confirma la eliminación de una persona.
        /// </summary>
        /// <param name="id">El identificador de la persona a eliminar.</param>
        /// <returns>Una redirección a la acción "Index" en caso de éxito.</returns>
        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        [ProducesResponseType(typeof(RedirectToActionResult), 302)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Personas == null)
            {
                return Problem("Entity set 'persona_dbContext.Personas'  is null.");
            }
            var persona = await _context.Personas.FindAsync(id);
            if (persona != null)
            {
                _context.Personas.Remove(persona);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonaExists(int id)
        {
          return _context.Personas.Any(e => e.Cc == id);
        }

    }
}
