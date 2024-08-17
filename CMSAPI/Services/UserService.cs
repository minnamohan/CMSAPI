using CMSAPI.Data;
using CMSAPI.Helpers;
using CMSAPI.Interfaces;
using CMSAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CMSAPI.Services
{
    public class UserService : IUserService
    {
        readonly AppDbContext _appDbContext;
        public UserService(AppDbContext appDbContext) 
        {
            _appDbContext = appDbContext;
        }
        public async Task<bool> RegisterUserAsync(string username, string password)
        {
            try
            {
                var user = new User
                {
                    Username = username,
                    PasswordHash = PasswordHasher.HashPassword(password)
                };

                _appDbContext.Users.Add(user);
                await _appDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"An exception occurred while registering User {ex}");
            }

            return true;
        }
    }
}
