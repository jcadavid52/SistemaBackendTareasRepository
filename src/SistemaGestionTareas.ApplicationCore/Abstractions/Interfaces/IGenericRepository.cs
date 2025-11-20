namespace SistemaGestionTareas.ApplicationCore.Abstractions.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> Query();

        Task CreateAsync(T entity);

        void Update(T entity);

        void Delete(T entity);
    }
}
