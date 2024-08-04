using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BACKEND_ASP.NET_WEB_API.Models;

namespace BACKEND_ASP.NET_WEB_API.Interfaces
{
    public interface IUsersRepository
    {
        public Task<ICollection<User>> GetAllAsync();
        public Task<User?> GetByIdAsync(int id);
        public Task<User?> GetByEmailAsync(string email);
        public Task<User> CreateAsync(User user);
        public Task<User?> DeleteAsync(int id);
        public Task<bool> ExistsAsync(int id);
    }
}