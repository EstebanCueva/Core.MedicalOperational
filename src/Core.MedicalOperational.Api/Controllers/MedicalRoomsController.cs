using Core.MedicalOperational.Application.DTOs.MedicalRooms;
using Core.MedicalOperational.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Core.MedicalOperational.Api.Controllers;

[ApiController]
[Route("api/medical-rooms")]
public class MedicalRoomsController : ControllerBase
{
    private readonly IMedicalRoomService _medicalRoomService;

    public MedicalRoomsController(IMedicalRoomService medicalRoomService)
    {
        _medicalRoomService = medicalRoomService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var response = await _medicalRoomService.GetAllAsync(cancellationToken);
        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var response = await _medicalRoomService.GetByIdAsync(id, cancellationToken);

        if (response is null)
        {
            return NotFound("Medical room not found.");
        }

        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateMedicalRoomRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _medicalRoomService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateMedicalRoomRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _medicalRoomService.UpdateAsync(id, request, cancellationToken);
        return Ok(response);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _medicalRoomService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}