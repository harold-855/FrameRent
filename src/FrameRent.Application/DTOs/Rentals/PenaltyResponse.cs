namespace FrameRent.Application.DTOs.Rentals;

public record PenaltyResponse(
    Guid Id,
    string Reason,
    decimal Amount,
    DateTime CreatedAt,
    bool IsPaid);
