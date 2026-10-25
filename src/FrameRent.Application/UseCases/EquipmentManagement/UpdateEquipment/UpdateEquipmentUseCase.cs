using FrameRent.Application.DTOs.Equipment;
using FrameRent.Application.Interfaces.Repositories;

namespace FrameRent.Application.UseCases.EquipmentManagement.UpdateEquipment;

public class UpdateEquipmentUseCase
{
    private readonly IEquipmentRepository _equipmentRepository;

    public UpdateEquipmentUseCase(IEquipmentRepository equipmentRepository)
    {
        _equipmentRepository = equipmentRepository;
    }

    public async Task ExecuteAsync(Guid id, UpdateEquipmentRequest request)
    {
        var equipment = await _equipmentRepository.GetByIdAsync(id);

        if (equipment is null)
            throw new KeyNotFoundException("Equipo no encontrado.");

        string name = string.IsNullOrWhiteSpace(request.Name) ? equipment.Name : request.Name;
        string category = string.IsNullOrWhiteSpace(request.Category) ? equipment.Category : request.Category;
        string description = request.Description ?? equipment.Description;
        decimal dailyRate = request.DailyRate > 0 ? request.DailyRate : equipment.DailyRate;
        int totalUnits = request.TotalUnits > 0 ? request.TotalUnits : equipment.TotalUnits;

        equipment.Update(name, category, description, dailyRate, totalUnits);

        await _equipmentRepository.UpdateAsync(equipment);
    }
}