namespace SistemaGestionTareas.ApplicationCore.Abstractions.Interfaces
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
