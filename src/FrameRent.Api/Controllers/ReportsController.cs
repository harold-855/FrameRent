using FrameRent.Application.UseCases.Reports.GetTopEquipment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FrameRent.Api.Controllers;

[ApiController]
[Route("api/v1/reports")]
public class ReportsController : ControllerBase
{
    private readonly GetTopEquipmentUseCase _getTopEquipmentUseCase;

    public ReportsController(GetTopEquipmentUseCase getTopEquipmentUseCase)
    {
        _getTopEquipmentUseCase = getTopEquipmentUseCase;
    }

    [Authorize(Roles = "Staff")]
    [HttpGet("top-equipment")]
    public async Task<IActionResult> GetTopEquipment([FromQuery] int limit = 10)
    {
        var result = await _getTopEquipmentUseCase.ExecuteAsync(limit);
        return Ok(result);
    }
}
