using FrameRent.Application.DTOs.Equipment;
using FrameRent.Application.Interfaces.Repositories;

namespace FrameRent.Application.UseCases.EquipmentManagement.GetEquipmentById;

public class GetEquipmentByIdUseCase
{
    private readonly IEquipmentRepository _equipmentRepository;

    public GetEquipmentByIdUseCase(IEquipmentRepository equipmentRepository)
    {
        _equipmentRepository = equipmentRepository;
    }

    public async Task<EquipmentResponse?> ExecuteAsync(Guid id)
    {
        var equipment = await _equipmentRepository.GetByIdAsync(id);
        if (equipment is null) return null;

        return new EquipmentResponse(
            equipment.Id,
            equipment.Name,
            equipment.Category,
            equipment.Description,
            equipment.DailyRate,
            equipment.TotalUnits,
            equipment.IsActive);
    }
}