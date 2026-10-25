using FrameRent.Application.Interfaces.Repositories;

namespace FrameRent.Application.UseCases.EquipmentManagement.DeactivateEquipment;

public class DeactivateEquipmentUseCase
{
    private readonly IEquipmentRepository _equipmentRepository;

    public DeactivateEquipmentUseCase(IEquipmentRepository equipmentRepository)
    {
        _equipmentRepository = equipmentRepository;
    }

    public async Task ExecuteAsync(Guid id)
    {
        var equipment = await _equipmentRepository.GetByIdAsync(id);

        if (equipment is null)
            throw new KeyNotFoundException("Equipo no encontrado.");

        var hasActiveRentals = await _equipmentRepository.HasActiveOrDeliveredRentalsAsync(id);
        if (hasActiveRentals)
            throw new InvalidOperationException("Un equipo con alquileres aprobados o entregados no puede desactivarse.");

        equipment.Deactivate();

        await _equipmentRepository.UpdateAsync(equipment);
    }
}