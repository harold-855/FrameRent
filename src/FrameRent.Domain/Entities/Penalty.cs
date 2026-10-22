namespace FrameRent.Domain.Entities;

public class Penalty : BaseEntity
{
    public Guid RentalId { get; private set; }

    public string Reason { get; private set; } = string.Empty;

    public decimal Amount { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public bool IsPaid { get; private set; }

    public Penalty(
        Guid rentalId,
        string reason,
        decimal amount)
    {
        if (rentalId == Guid.Empty)
            throw new ArgumentException(
                "The rental ID is required.",
                nameof(rentalId));

        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException(
                "The reason is required.",
                nameof(reason));

        if (amount <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(amount),
                "The penalty amount must be greater than zero.");

        RentalId = rentalId;
        Reason = reason.Trim();
        Amount = amount;
        CreatedAt = DateTime.UtcNow;
        IsPaid = false;
    }

    private Penalty()
    {
    }

    public void MarkAsPaid()
    {
        if (IsPaid)
            throw new InvalidOperationException(
                "This penalty has already been paid.");

        IsPaid = true;
    }

    public void UpdateReason(string newReason)
    {
        if (string.IsNullOrWhiteSpace(newReason))
            throw new ArgumentException(
                "The reason is required.",
                nameof(newReason));

        Reason = newReason.Trim();
    }
}