using FrameRent.Domain.Enums;

namespace FrameRent.Application.DTOs.Rentals;

public record RentalResponse(
    Guid Id,
    Guid CustomerId,
    string CustomerName,
    string CustomerEmail,
    DateTime StartDate,
    DateTime EndDate,
    decimal TotalAmount,
    string Status,
    IReadOnlyCollection<RentalDetailResponse> Details,
    IReadOnlyCollection<PenaltyResponse> Penalties);
