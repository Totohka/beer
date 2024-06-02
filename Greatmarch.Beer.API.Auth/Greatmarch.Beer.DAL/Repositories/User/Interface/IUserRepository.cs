using Greatmarch.Beer.DAL.Repositories.Template.Interface;
using Greatmarch.Beer.Model.Entities;

namespace Greatmarch.Beer.DAL.Repositories.User.Interface
{
    public interface IUserRepository : IRepository<Model.Entities.User>
    {
        Task<Model.Entities.User> GetByEmailAsync(string email);
    }
}
