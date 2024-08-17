using CMSAPI.Models.DTOs;

namespace CMSAPI.Interfaces
{
    public interface IUserService
    {
        Task<bool> RegisterUserAsync(string username, string password);
    }
}
