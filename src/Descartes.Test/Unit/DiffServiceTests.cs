using Descartes.Business.Models;
using Descartes.Business.Services;

namespace Descartes.Test.Unit
{
    public class DiffServiceTests
    {
        private readonly DiffService _service = new();

        [Fact]
        public void Compute_ReturnsEquals_WhenInputsMatch()
        {
            var left = Convert.FromBase64String("AAAAAA==");
            var right = Convert.FromBase64String("AAAAAA==");

            var result = _service.Compute(left, right);

            Assert.Equal(DiffResultType.Equals, result.DiffResultType);
            Assert.Null(result.Diffs);
        }

        [Fact]
        public void Compute_ReturnsSizeDoNotMatch_WhenLengthsDiffer()
        {
            var left = Convert.FromBase64String("AAA=");
            var right = Convert.FromBase64String("AAAAAA==");

            var result = _service.Compute(left, right);

            Assert.Equal(DiffResultType.SizeDoNotMatch, result.DiffResultType);
            Assert.Null(result.Diffs);
        }

        [Fact]
        public void Compute_ReturnsDiffs_WhenContentDiffers()
        {
            var left = Convert.FromBase64String("AAAAAA==");
            var right = Convert.FromBase64String("AQABAQ==");

            var result = _service.Compute(left, right);

            Assert.Equal(DiffResultType.ContentDoNotMatch, result.DiffResultType);
            Assert.NotNull(result.Diffs);
            var diffs = result.Diffs!;
            Assert.Collection(
                diffs,
                diff =>
                {
                    Assert.Equal(0, diff.Offset);
                    Assert.Equal(1, diff.Length);
                },
                diff =>
                {
                    Assert.Equal(2, diff.Offset);
                    Assert.Equal(2, diff.Length);
                });
        }

        [Fact]
        public void Compute_ReturnsEquals_WhenBothAreEmpty()
        {
            var left = Array.Empty<byte>();
            var right = Array.Empty<byte>();

            var result = _service.Compute(left, right);

            Assert.Equal(DiffResultType.Equals, result.DiffResultType);
            Assert.Null(result.Diffs);
        }
    }
}
