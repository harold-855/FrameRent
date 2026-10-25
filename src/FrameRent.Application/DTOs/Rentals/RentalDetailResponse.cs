namespace FrameRent.Application.DTOs.Rentals;

public record RentalDetailResponse(
    Guid Id,
    Guid EquipmentId,
    string EquipmentName,
    int Quantity,
    decimal DailyRate,
    int RentalDays,
    decimal Subtotal);
