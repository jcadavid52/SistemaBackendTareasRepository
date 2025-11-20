using Microsoft.EntityFrameworkCore;
using SistemaGestionTareas.ApplicationCore.Abstractions.Interfaces;
using SistemaGestionTareas.ApplicationCore.Exceptions;
using SistemaGestionTareas.Infrastructure.Data;

namespace SistemaGestionTareas.Infrastructure.Common
{
    public class UnitOfWork(DataContext dataContext): IUnitOfWork
    {
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await dataContext.SaveChangesAsync(cancellationToken);

                return result;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new ConcurrencyException($"La excepcion por concurrencia se disparo {ex}");
            }
        }
    }
}
