namespace FrameRent.Domain.Entities;

public class RentalDetail : BaseEntity
{
    public Guid RentalId { get; private set; }

    public Guid EquipmentId { get; private set; }

    public int Quantity { get; private set; }

    public decimal DailyRate { get; private set; }

    public int RentalDays { get; private set; }

    public decimal Subtotal { get; private set; }

    public RentalDetail(
        Guid rentalId,
        Guid equipmentId,
        int quantity,
        decimal dailyRate,
        int rentalDays)
    {
        if (rentalId == Guid.Empty)
            throw new ArgumentException(
                "The rental ID is required.",
                nameof(rentalId));

        if (equipmentId == Guid.Empty)
            throw new ArgumentException(
                "The equipment ID is required.",
                nameof(equipmentId));

        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(quantity),
                "The quantity must be greater than zero.");

        if (dailyRate < 0)
            throw new ArgumentOutOfRangeException(
                nameof(dailyRate),
                "The daily rate cannot be negative.");

        if (rentalDays <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(rentalDays),
                "The rental days must be greater than zero.");

        RentalId = rentalId;
        EquipmentId = equipmentId;
        Quantity = quantity;
        DailyRate = dailyRate;
        RentalDays = rentalDays;

        Subtotal = quantity * dailyRate * rentalDays;
    }

    private RentalDetail()
    {
    }
}