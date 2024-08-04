using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BACKEND_ASP.NET_WEB_API.Models;

namespace BACKEND_ASP.NET_WEB_API.Controllers
{
    // Define la ruta base del controlador como "api/[controller]", donde [controller] se reemplaza por el nombre del controlador.
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        // Campo privado para la instancia del contexto de la base de datos.
        private readonly ApiDbContext _context;

        // Constructor que recibe el contexto de la base de datos a través de inyección de dependencias.
        public UsersController(ApiDbContext context)
        {
            _context = context;
        }

        // Método GET: api/Users
        // Recupera todos los usuarios de la base de datos de manera asíncrona.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Users>>> Getusers()
        {
            return await _context.users.ToListAsync();
        }

        // Método GET: api/Users/5
        // Recupera un usuario específico por su ID de manera asíncrona.
        [HttpGet("{id}")]
        public async Task<ActionResult<Users>> GetUsers(int id)
        {
            var users = await _context.users.FindAsync(id);

            // Si el usuario no existe, retorna un estado 404 (No encontrado).
            if (users == null)
            {
                return NotFound();
            }

            return users;
        }

        // Método PUT: api/Users/5
        // Actualiza un usuario específico por su ID. Protege contra ataques de sobrepublicación.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsers(int id, Users users)
        {
            // Verifica si el ID en la URL coincide con el ID del usuario en el cuerpo de la solicitud.
            if (id != users.userId)
            {
                return BadRequest();
            }

            // Marca la entidad como modificada.
            _context.Entry(users).State = EntityState.Modified;

            try
            {
                // Intenta guardar los cambios en la base de datos.
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Si el usuario no existe, retorna un estado 404 (No encontrado).
                if (!UsersExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // Retorna un estado 204 (Sin contenido) en caso de éxito.
            return NoContent();
        }

        // Método POST: api/Users
        // Crea un nuevo usuario. Protege contra ataques de sobrepublicación.
        [HttpPost]
        public async Task<ActionResult<Users>> PostUsers(Users users)
        {
            // Agrega el nuevo usuario al contexto.
            _context.users.Add(users);
            await _context.SaveChangesAsync();

            // Retorna un estado 201 (Creado) con la ubicación del nuevo recurso.
            return CreatedAtAction("GetUsers", new { id = users.userId }, users);
        }

        // Método DELETE: api/Users/5
        // Elimina un usuario específico por su ID.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsers(int id)
        {
            var users = await _context.users.FindAsync(id);
            // Si el usuario no existe, retorna un estado 404 (No encontrado).
            if (users == null)
            {
                return NotFound();
            }

            // Elimina el usuario del contexto.
            _context.users.Remove(users);
            await _context.SaveChangesAsync();

            // Retorna un estado 204 (Sin contenido) en caso de éxito.
            return NoContent();
        }

        // Método auxiliar privado para verificar si un usuario existe por su ID.
        private bool UsersExists(int id)
        {
            return _context.users.Any(e => e.userId == id);
        }
    }
}
