using FrameRent.Application.DTOs.Equipment;
using FrameRent.Application.Interfaces.Repositories;
using FrameRent.Domain.Entities;

namespace FrameRent.Application.UseCases.EquipmentManagement.CreateEquipment;

public class CreateEquipmentUseCase
{
    private readonly IEquipmentRepository _equipmentRepository;

    public CreateEquipmentUseCase(
        IEquipmentRepository equipmentRepository)
    {
        _equipmentRepository = equipmentRepository;
    }

    public async Task ExecuteAsync(CreateEquipmentRequest request)
    {
        var equipment = new Equipment(
            request.Name,
            request.Category,
            request.Description,
            request.DailyRate,
            request.TotalUnits);

        await _equipmentRepository.AddAsync(equipment);
    }
}