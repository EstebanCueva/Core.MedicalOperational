using Core.MedicalOperational.Application.DTOs.OperationalCompatibility;
using Core.MedicalOperational.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Core.MedicalOperational.Api.Controllers;

[ApiController]
[Route("api/operational-compatibility")]
[Authorize(Roles = "Admin,OperationalManager")]
public class OperationalCompatibilityController : ControllerBase
{
    private readonly IOperationalCompatibilityService _operationalCompatibilityService;

    public OperationalCompatibilityController(IOperationalCompatibilityService operationalCompatibilityService)
    {
        _operationalCompatibilityService = operationalCompatibilityService;
    }

    [HttpPost("analyze")]
    public async Task<IActionResult> Analyze(
        [FromBody] OperationalCompatibilityRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _operationalCompatibilityService.AnalyzeAsync(request, cancellationToken);
        return Ok(response);
    }

    [HttpPost("analyze-demo")]
    public async Task<IActionResult> AnalyzeDemo(CancellationToken cancellationToken)
    {
        var request = new OperationalCompatibilityRequest
        {
            AnalysisDate = DateTime.Now,
            SpecialtyCode = null,
            ProcedureCode = null,
            IncludeOnlyCompatible = false,
            SaveResults = true
        };

        var response = await _operationalCompatibilityService.AnalyzeAsync(request, cancellationToken);
        return Ok(response);
    }
}