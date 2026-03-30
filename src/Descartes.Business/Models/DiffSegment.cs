namespace Descartes.Business.Models
{
    public class DiffSegment
    {
        public DiffSegment(int offset, int length)
        {
            Offset = offset;
            Length = length;
        }

        public int Offset { get; }
        public int Length { get; }
    }
}
