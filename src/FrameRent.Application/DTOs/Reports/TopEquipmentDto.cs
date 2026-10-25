namespace FrameRent.Application.DTOs.Reports;

public record TopEquipmentDto(
    Guid EquipmentId,
    string EquipmentName,
    string Category,
    int TotalRentals,
    int TotalUnitsRented);
