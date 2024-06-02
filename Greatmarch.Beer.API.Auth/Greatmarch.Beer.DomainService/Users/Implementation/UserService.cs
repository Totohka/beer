using Microsoft.Extensions.Logging;
using Greatmarch.Beer.Model.Entities;
using Greatmarch.Beer.DAL.Repositories.User.Interface;
using Greatmarch.Beer.DomainService.Users.Interface;

namespace Greatmarch.Beer.DomainService.Users.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repositoryUser;
        private readonly ILogger<UserService> _logger;
        public UserService(IUserRepository repositoryUser,
                           ILogger<UserService> logger)
        {
            _logger = logger;
            _repositoryUser = repositoryUser;
        }

        public async Task<User> GetAsync(int id)
        {

            return await _repositoryUser.GetAsync(id);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _repositoryUser.GetByEmailAsync(email);
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _repositoryUser.GetAllAsync();
        }

        public async Task UpdateAsync(User user)
        {

            var userOld = await _repositoryUser.GetAsync(user.Id);

            userOld.FirstName = user.FirstName != string.Empty ? user.FirstName : userOld.FirstName;
            userOld.LastName = user.LastName != string.Empty ? user.LastName : userOld.LastName;
            userOld.Email = user.Email != string.Empty ? user.Email : userOld.Email;

            _repositoryUser.Update(userOld);
        }
    }
}