namespace Descartes.Business.Models
{
    public class DiffResult
    {
        public DiffResult(DiffResultType diffResultType, IReadOnlyList<DiffSegment>? diffs)
        {
            DiffResultType = diffResultType;
            Diffs = diffs;
        }

        public DiffResultType DiffResultType { get; }

        public IReadOnlyList<DiffSegment>? Diffs { get; }

        public static DiffResult Equal() => new(DiffResultType.Equals, null);

        public static DiffResult SizeDoNotMatch() => new(DiffResultType.SizeDoNotMatch, null);

        public static DiffResult ContentDoNotMatch(IReadOnlyList<DiffSegment> diffs)
            => new(DiffResultType.ContentDoNotMatch, diffs);
    }
}
