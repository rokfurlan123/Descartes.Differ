using Descartes.Business.Models;

namespace Descartes.Business.Repositories
{
    public class DiffEntry
    {
        public DiffEntry(byte[]? left, byte[]? right)
        {
            Left = left;
            Right = right;
        }

        public byte[]? Left { get; }

        public byte[]? Right { get; }

        public bool IsComplete => Left is not null && Right is not null;

        public DiffEntry WithSide(DiffSide side, byte[] data)
        {
            return side == DiffSide.Left
                ? new DiffEntry(data, Right)
                : new DiffEntry(Left, data);
        }
    }
}
