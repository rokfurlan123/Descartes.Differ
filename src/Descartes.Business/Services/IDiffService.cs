using Descartes.Business.Models;

namespace Descartes.Business.Services
{
    /// <summary>
    /// Computes a diff result for two binary inputs.
    /// </summary>
    public interface IDiffService
    {
        DiffResult Compute(byte[] left, byte[] right);
    }
}
