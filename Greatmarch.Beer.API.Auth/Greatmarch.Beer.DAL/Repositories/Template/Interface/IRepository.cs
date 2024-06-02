namespace Greatmarch.Beer.DAL.Repositories.Template.Interface
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetAsync(int id);
        Task<List<T>> GetAllAsync();
        void Create(T item);
        void Update(T item);
        Task DeleteAsync(int id);
    }
}
