namespace tongkangku_be.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(Guid id);
        Task<T?> GetByIdAsync(Guid id, params string[] includeProperties);
        Task<List<T>> GetAllAsync();
        Task<List<T>> GetAllAsync(params string[] includeProperties);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<int> SaveChangesAsync();
    }
}
