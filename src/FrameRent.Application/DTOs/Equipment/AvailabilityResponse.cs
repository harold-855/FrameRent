namespace FrameRent.Application.DTOs.Equipment;

public record AvailabilityResponse(
    Guid EquipmentId,
    string EquipmentName,
    int TotalUnits,
    int RentedUnits,
    int AvailableUnits,
    DateTime From,
    DateTime To);
