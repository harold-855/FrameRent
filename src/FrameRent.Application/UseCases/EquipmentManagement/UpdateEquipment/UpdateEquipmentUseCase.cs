using FrameRent.Application.DTOs.Equipment;
using FrameRent.Application.Interfaces.Repositories;

namespace FrameRent.Application.UseCases.EquipmentManagement.UpdateEquipment;

public class UpdateEquipmentUseCase
{
    private readonly IEquipmentRepository _equipmentRepository;

    public UpdateEquipmentUseCase(
        IEquipmentRepository equipmentRepository)
    {
        _equipmentRepository = equipmentRepository;
    }

    public async Task ExecuteAsync(
        Guid id,
        UpdateEquipmentRequest request)
    {
        var equipment = await _equipmentRepository.GetByIdAsync(id);

        if (equipment is null)
            throw new KeyNotFoundException("Equipment not found.");

        equipment.UpdateDescription(request.Description);
        equipment.UpdateDailyRate(request.DailyRate);
        equipment.UpdateTotalUnits(request.TotalUnits);

        await _equipmentRepository.UpdateAsync(equipment);
    }
}