using Descartes.Business.Models;

namespace Descartes.Business.Services
{
    /// <summary>
    /// Simple diff implementation that reports contiguous differing ranges.
    /// </summary>
    public class DiffService : IDiffService
    {
        public DiffResult Compute(byte[] left, byte[] right)
        {
            if (left.Length != right.Length)
            {
                return DiffResult.SizeDoNotMatch();
            }

            if (left.Length == 0)
            {
                return DiffResult.Equal();
            }

            var diffs = new List<DiffSegment>();
            var index = 0;

            while (index < left.Length)
            {
                if (left[index] == right[index])
                {
                    index++;
                    continue;
                }

                var start = index;
                while (index < left.Length && left[index] != right[index])
                {
                    index++;
                }

                diffs.Add(new DiffSegment(start, index - start));
            }

            return diffs.Count == 0 ? DiffResult.Equal() : DiffResult.ContentDoNotMatch(diffs);
        }
    }
}
