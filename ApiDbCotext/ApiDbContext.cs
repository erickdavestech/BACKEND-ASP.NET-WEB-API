// Core library,
using Microsoft.EntityFrameworkCore;
// NameSpace
namespace BACKEND_ASP.NET_WEB_API.Models
{
    // Extends from Db Context en pocas palabras hereda
    public class ApiDbContext : DbContext 
    {
        /* 
           * -> Este constructor acepta opciones de tipo DbContextOptions en su parametro option
           -  a su vez pasa el parametro opcion al contructor de la clasa base
           - el parametro opcion es usado para configurar el DbContext de alli el nombre para esta clase ApiDbContext
           */

        // ApiDbContext(DbContextOptions option) : base(option) {}
        // Fix Making  public ApiDbContext(DbContextOptions option) : base(option) {}
       public ApiDbContext(DbContextOptions option) : base(option) {}
        // Representa nuestra tabla en la base de datos
        public DbSet<User> Users { get; set; }
    }
}
