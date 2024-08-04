using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BACKEND_ASP.NET_WEB_API.Interfaces;
using BACKEND_ASP.NET_WEB_API.Models;
using Microsoft.EntityFrameworkCore;

namespace BACKEND_ASP.NET_WEB_API.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly ApiDbContext _context;

        public UsersRepository(ApiDbContext context)
        {
            _context = context;
        }
        public async Task<User> CreateAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(user.Id) ?? user;
        }

        public async Task<User?> DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return null;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);

            // Si user es diferente a null manda true, si es null manda false
            return user != null;
        }

        public async Task<ICollection<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }
    }
}