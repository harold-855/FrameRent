using FrameRent.Application.DTOs.Equipment;
using FrameRent.Application.Interfaces.Repositories;

namespace FrameRent.Application.UseCases.EquipmentManagement.GetAllEquipment;

public class GetAllEquipmentUseCase
{
    private readonly IEquipmentRepository _equipmentRepository;

    public GetAllEquipmentUseCase(IEquipmentRepository equipmentRepository)
    {
        _equipmentRepository = equipmentRepository;
    }

    public async Task<IReadOnlyCollection<EquipmentResponse>> ExecuteAsync()
    {
        var equipments = await _equipmentRepository.GetAllAsync();
        return equipments.Select(e => new EquipmentResponse(
            e.Id,
            e.Name,
            e.Category,
            e.Description,
            e.DailyRate,
            e.TotalUnits,
            e.IsActive)).ToList();
    }
}