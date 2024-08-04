using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BACKEND_ASP.NET_WEB_API.Models;
using BACKEND_ASP.NET_WEB_API.Interfaces;
using BACKEND_ASP.NET_WEB_API.DTOs;
using AutoMapper;

namespace BACKEND_ASP.NET_WEB_API.Controllers
{
    // Define la ruta base del controlador como "api/[controller]", donde [controller] se reemplaza por el nombre del controlador.
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        // Campo privado para la instancia del contexto de la base de datos.
        private readonly ApiDbContext _context;
        private readonly IUsersRepository _usersRepo;
        private readonly IMapper _mapper;

        // Constructor que recibe el contexto de la base de datos a través de inyección de dependencias.
        public UsersController(ApiDbContext context, IUsersRepository usersRepository, IMapper mapper)
        {
            _context = context;
            // se agrega el repositorio
            _usersRepo = usersRepository;
            _mapper = mapper;
        }

        // Método GET: api/Users
        // Recupera todos los usuarios de la base de datos de manera asíncrona.
        [HttpGet]
        public async Task<ActionResult> GetUsers()
        {
            
            return Ok(await _usersRepo.GetAllAsync());
        }

        // Método GET: api/Users/5
        // Recupera un usuario específico por su ID de manera asíncrona.
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            var user = await _usersRepo.GetByIdAsync(id);

            // Si el usuario no existe, retorna un estado 404 (No encontrado).
            if (user == null)
                return NotFound( new { Message = "No se encontro el usuario" } );
            

            return user;
        }

        // Método PUT: api/Users/5
        // Actualiza un usuario específico por su ID. Protege contra ataques de sobrepublicación.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            // Verifica si el ID en la URL coincide con el ID del usuario en el cuerpo de la solicitud.
            if (id != user.Id)
                return BadRequest();
            

            // Marca la entidad como modificada.
            _context.Entry(user).State = EntityState.Modified;

            try
            {
                // Intenta guardar los cambios en la base de datos.
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Si el usuario no existe, retorna un estado 404 (No encontrado).
                if (!await _usersRepo.ExistsAsync(id))
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
        public async Task<ActionResult<User>> CreateUser(UserDto userDto)
        {
            // Por medio de los data annotations verifica que esten los campos requeridos y sea email valido
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Obtiene el usuario por su correo
            var user = _usersRepo.GetByEmailAsync(userDto.Email!);

            if (user == null)
                return Conflict(new { Message = "This user already exists." });

            // Convierte un usuario que solo tiene nombre y correo y le agrega el id
            // de esta forma para enviar el dato solo se necesita Name y Email
            var userModel = _mapper.Map<User>(userDto);

            // agrega el elemento
            var userSaved = await _usersRepo.CreateAsync(userModel);

            // Retorna un estado 201 (Creado) con la ubicación del nuevo recurso.
            return CreatedAtAction("GetUsers", new { id = userSaved.Id }, user);
        }

        // Método DELETE: api/Users/5
        // Elimina un usuario específico por su ID.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var userExists = await _usersRepo.ExistsAsync(id);
            // Si el usuario no existe, retorna un estado 404 (No encontrado).
            if (!userExists)
                return NotFound(new { Message = "This user doesn't exists."});
            
            // Elimina el usuario del contexto.
            await _usersRepo.DeleteAsync(id);

            // Retorna un estado 204 (Sin contenido) en caso de éxito.
            return NoContent();
        }
    }
}
