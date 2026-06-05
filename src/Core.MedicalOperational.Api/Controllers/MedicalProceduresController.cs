using Core.MedicalOperational.Application.DTOs.MedicalProcedures;
using Core.MedicalOperational.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Core.MedicalOperational.Api.Controllers;

[ApiController]
[Route("api/medical-procedures")]
public class MedicalProceduresController : ControllerBase
{
    private readonly IMedicalProcedureService _medicalProcedureService;

    public MedicalProceduresController(IMedicalProcedureService medicalProcedureService)
    {
        _medicalProcedureService = medicalProcedureService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var response = await _medicalProcedureService.GetAllAsync(cancellationToken);
        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var response = await _medicalProcedureService.GetByIdAsync(id, cancellationToken);

        if (response is null)
        {
            return NotFound("Medical procedure not found.");
        }

        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateMedicalProcedureRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _medicalProcedureService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateMedicalProcedureRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _medicalProcedureService.UpdateAsync(id, request, cancellationToken);
        return Ok(response);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _medicalProcedureService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}