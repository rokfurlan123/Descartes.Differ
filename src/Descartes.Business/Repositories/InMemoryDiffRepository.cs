using System.Collections.Concurrent;
using Descartes.Business.Models;

namespace Descartes.Business.Repositories
{
    public class InMemoryDiffRepository : IDiffRepository
    {
        private readonly ConcurrentDictionary<string, DiffEntry> _dictionary = new();

        public Task<DiffEntry?> GetAsync(string id, CancellationToken cancellationToken = default)
        {
            _dictionary.TryGetValue(id, out var entry);
            return Task.FromResult(entry);
        }

        public Task UpsertAsync(string id, DiffSide side, byte[] data, CancellationToken cancellationToken = default)
        {
            _dictionary.AddOrUpdate(
                id,
                _ => side == DiffSide.Left ? new DiffEntry(data, null) : new DiffEntry(null, data),
                (_, existing) => existing.WithSide(side, data));

            return Task.CompletedTask;
        }
    }
}
