using FrameRent.Application.Interfaces.Repositories;
namespace FrameRent.Application.UseCases.EquipmentManagement.ActivateEquipment;

public class ActivateEquipmentUseCase
{
    private readonly IEquipmentRepository _equipmentRepository;

    public ActivateEquipmentUseCase(
        IEquipmentRepository equipmentRepository)
    {
        _equipmentRepository = equipmentRepository;
    }

    public async Task ExecuteAsync(Guid id)
    {
        var equipment = await _equipmentRepository.GetByIdAsync(id);

        if (equipment is null)
            throw new KeyNotFoundException("Equipment not found.");

        equipment.Activate();

        await _equipmentRepository.UpdateAsync(equipment);
    }
}