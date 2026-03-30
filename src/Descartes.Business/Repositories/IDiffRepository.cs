using Descartes.Business.Models;

namespace Descartes.Business.Repositories
{
    public interface IDiffRepository
    {
        Task<DiffEntry?> GetAsync(string id, CancellationToken cancellationToken = default);
        Task UpsertAsync(string id, DiffSide side, byte[] data, CancellationToken cancellationToken = default);
    }
}
