using Greatmarch.Beer.Model.Entities;

namespace Greatmarch.Beer.DomainService.Users.Interface
{
    public interface IJWTService
    {
        public Task<string> RegistrationAsync(User user);
        public Task<string> AuthAsync(string email, string password);
        public Task<string> UpdateTokenAsync(int userId);
    }
}
