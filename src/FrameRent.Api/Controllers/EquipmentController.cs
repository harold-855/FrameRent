
using FrameRent.Application.DTOs.Equipment;
using FrameRent.Application.UseCases.EquipmentManagement.ActivateEquipment;
using FrameRent.Application.UseCases.EquipmentManagement.CreateEquipment;
using FrameRent.Application.UseCases.EquipmentManagement.DeactivateEquipment;
using FrameRent.Application.UseCases.EquipmentManagement.GetAllEquipment;
using FrameRent.Application.UseCases.EquipmentManagement.UpdateEquipment;
using FrameRent.Application.UseCases.EquipmentManagement.GetEquipmentById;
using Microsoft.AspNetCore.Mvc;

namespace FrameRent.Api.Controllers;

// Indica que esta clase recibe solicitudes HTTP y funciona como controlador.
[ApiController]

// Define la ruta base: api/equipment.
[Route("api/[controller]")]
public class EquipmentController : ControllerBase
{
    // Casos de uso que ejecutan las operaciones del módulo de equipos.
    private readonly GetAllEquipmentUseCase _getAllEquipmentUseCase;
    private readonly GetEquipmentByIdUseCase _getEquipmentByIdUseCase;
    private readonly CreateEquipmentUseCase _createEquipmentUseCase;
    private readonly UpdateEquipmentUseCase _updateEquipmentUseCase;
    private readonly ActivateEquipmentUseCase _activateEquipmentUseCase;
    private readonly DeactivateEquipmentUseCase _deactivateEquipmentUseCase;

    // ASP.NET Core proporciona estas dependencias automáticamente.
    public EquipmentController(
        GetAllEquipmentUseCase getAllEquipmentUseCase,
        GetEquipmentByIdUseCase getEquipmentByIdUseCase,
        CreateEquipmentUseCase createEquipmentUseCase,
        UpdateEquipmentUseCase updateEquipmentUseCase,
        ActivateEquipmentUseCase activateEquipmentUseCase,
        DeactivateEquipmentUseCase deactivateEquipmentUseCase)
    {
        _getAllEquipmentUseCase = getAllEquipmentUseCase;
        _getEquipmentByIdUseCase = getEquipmentByIdUseCase;
        _createEquipmentUseCase = createEquipmentUseCase;
        _updateEquipmentUseCase = updateEquipmentUseCase;
        _activateEquipmentUseCase = activateEquipmentUseCase;
        _deactivateEquipmentUseCase = deactivateEquipmentUseCase;
    }

    // GET: api/equipment
    // Consulta todos los equipos.
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var equipment = await _getAllEquipmentUseCase.ExecuteAsync();

        return Ok(equipment);
    }

    // GET: api/equipment/{id}
    // Consulta un equipo por su identificador.
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var equipment = await _getEquipmentByIdUseCase.ExecuteAsync(id);

        if (equipment is null)
        {
            return NotFound();
        }

        return Ok(equipment);
    }

    // POST: api/equipment
    // Crea un equipo nuevo.
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateEquipmentRequest request)
    {
        await _createEquipmentUseCase.ExecuteAsync(request);

        return Ok();
    }

    // PUT: api/equipment/{id}
    // Actualiza la descripción, la tarifa diaria y la cantidad de unidades.
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateEquipmentRequest request)
    {
        try
        {
            await _updateEquipmentUseCase.ExecuteAsync(id, request);

            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    // PATCH: api/equipment/{id}/activate
    // Cambia el estado del equipo a activo.
    [HttpPatch("{id:guid}/activate")]
    public async Task<IActionResult> Activate(Guid id)
    {
        try
        {
            await _activateEquipmentUseCase.ExecuteAsync(id);

            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    // PATCH: api/equipment/{id}/deactivate
    // Cambia el estado del equipo a inactivo.
    [HttpPatch("{id:guid}/deactivate")]
    public async Task<IActionResult> Deactivate(Guid id)
    {
        try
        {
            await _deactivateEquipmentUseCase.ExecuteAsync(id);

            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}
