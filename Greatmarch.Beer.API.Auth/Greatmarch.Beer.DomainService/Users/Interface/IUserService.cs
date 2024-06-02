using Greatmarch.Beer.Model.Entities;

namespace Greatmarch.Beer.DomainService.Users.Interface
{
    public interface IUserService
    {
        Task<List<User>> GetAllAsync();
        Task<User> GetByEmailAsync(string email);
        Task<User> GetAsync(int id);
        Task UpdateAsync(User user);
    }
}