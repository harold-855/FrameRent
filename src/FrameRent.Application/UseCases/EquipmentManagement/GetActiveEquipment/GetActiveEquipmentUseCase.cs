using FrameRent.Application.DTOs.Equipment;
using FrameRent.Application.Interfaces.Repositories;

namespace FrameRent.Application.UseCases.EquipmentManagement.GetActiveEquipment;

public class GetActiveEquipmentUseCase
{
    private readonly IEquipmentRepository _equipmentRepository;

    public GetActiveEquipmentUseCase(IEquipmentRepository equipmentRepository)
    {
        _equipmentRepository = equipmentRepository;
    }

    public async Task<IReadOnlyCollection<EquipmentResponse>> ExecuteAsync()
    {
        var equipments = await _equipmentRepository.GetAllActiveAsync();

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
