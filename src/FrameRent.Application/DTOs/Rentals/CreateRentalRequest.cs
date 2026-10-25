namespace FrameRent.Application.DTOs.Rentals;

public record CreateRentalRequest(
    Guid EquipmentId,
    int Quantity,
    DateTime StartDate,
    DateTime EndDate);
