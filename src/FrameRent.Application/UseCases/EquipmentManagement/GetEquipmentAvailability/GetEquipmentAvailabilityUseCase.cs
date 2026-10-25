using FrameRent.Application.DTOs.Equipment;
using FrameRent.Application.Interfaces.Repositories;

namespace FrameRent.Application.UseCases.EquipmentManagement.GetEquipmentAvailability;

public class GetEquipmentAvailabilityUseCase
{
    private readonly IEquipmentRepository _equipmentRepository;

    public GetEquipmentAvailabilityUseCase(IEquipmentRepository equipmentRepository)
    {
        _equipmentRepository = equipmentRepository;
    }

    public async Task<AvailabilityResponse> ExecuteAsync(Guid equipmentId, DateTime from, DateTime to)
    {
        var equipment = await _equipmentRepository.GetByIdAsync(equipmentId);
        if (equipment is null)
            throw new KeyNotFoundException("Equipo no encontrado.");

        if (to <= from)
            throw new ArgumentException("La fecha final debe ser posterior a la fecha inicial.");

        int rentedUnits = await _equipmentRepository.GetRentedUnitsInDateRangeAsync(equipmentId, from, to);
        int availableUnits = Math.Max(0, equipment.TotalUnits - rentedUnits);

        return new AvailabilityResponse(
            equipment.Id,
            equipment.Name,
            equipment.TotalUnits,
            rentedUnits,
            availableUnits,
            from,
            to);
    }
}
