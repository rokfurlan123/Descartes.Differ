using Descartes.Api.Models;
using Descartes.Business.Models;
using Descartes.Business.Repositories;
using Descartes.Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace Descartes.Api.Controllers
{
    [ApiController]
    [Route("v1/diff")]
    public class DiffController : ControllerBase
    {
        private readonly IDiffRepository _repository;
        private readonly IDiffService _diffService;

        public DiffController(IDiffRepository repository, IDiffService diffService)
        {
            _repository = repository;
            _diffService = diffService;
        }

        [HttpPut("{id}/left")]
        public Task<IActionResult> PutLeftAsync(string id, [FromBody] DiffRequest request, CancellationToken cancellationToken)
            => PutSideAsync(id, DiffSide.Left, request, cancellationToken);

        [HttpPut("{id}/right")]
        public Task<IActionResult> PutRightAsync(string id, [FromBody] DiffRequest request, CancellationToken cancellationToken)
            => PutSideAsync(id, DiffSide.Right, request, cancellationToken);

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDiffAsync(string id, CancellationToken cancellationToken)
        {
            var entry = await _repository.GetAsync(id, cancellationToken);
            if (entry is null || !entry.IsComplete)
            {
                return NotFound();
            }

            var result = _diffService.Compute(entry.Left!, entry.Right!);
            var response = ToResponse(result);
            return Ok(response);
        }

        private async Task<IActionResult> PutSideAsync(string id, DiffSide side, DiffRequest request, CancellationToken cancellationToken)
        {
            if (request is null || request.Data is null)
            {
                return BadRequest();
            }

            byte[] data;
            try
            {
                data = Convert.FromBase64String(request.Data);
            }
            catch (FormatException)
            {
                return BadRequest();
            }

            await _repository.UpsertAsync(id, side, data, cancellationToken);
            return StatusCode(StatusCodes.Status201Created);
        }

        private static DiffResponse ToResponse(DiffResult result)
        {
            return new DiffResponse
            {
                DiffResultType = result.DiffResultType.ToString(),
                Diffs = result.Diffs?.Select(segment => new DiffSegmentResponse
                {
                    Offset = segment.Offset,
                    Length = segment.Length
                }).ToArray()
            };
        }
    }
}
