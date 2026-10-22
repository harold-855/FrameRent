using FrameRent.Domain.Enums;

namespace FrameRent.Domain.Entities;

public class Rental : BaseEntity
{
    public Guid CustomerId { get; private set; }

    public DateTime StartDate { get; private set; }

    public DateTime EndDate { get; private set; }

    public decimal TotalAmount { get; private set; }

    public RentalStatus Status { get; private set; }
        = RentalStatus.Pending;

    private readonly List<RentalDetail> _details = new();

    public IReadOnlyCollection<RentalDetail> Details => _details.AsReadOnly();

    public Rental(
        Guid customerId,
        DateTime startDate,
        DateTime endDate)
    {
        if (customerId == Guid.Empty)
            throw new ArgumentException(
                "The customer ID is required.",
                nameof(customerId));

        if (endDate <= startDate)
            throw new ArgumentException(
                "The end date must be later than the start date.",
                nameof(endDate));

        CustomerId = customerId;
        StartDate = startDate;
        EndDate = endDate;
    }

    private Rental()
    {
    }

    public void AddDetail(
        Guid equipmentId,
        int quantity,
        decimal dailyRate)
    {
        if (Status != RentalStatus.Pending)
            throw new InvalidOperationException(
                "Details can only be added to pending rentals.");

        int rentalDays = (EndDate.Date - StartDate.Date).Days;

        if (rentalDays <= 0)
            throw new InvalidOperationException(
                "The rental duration must be at least one day.");

        var detail = new RentalDetail(
            Id,
            equipmentId,
            quantity,
            dailyRate,
            rentalDays);

        _details.Add(detail);

        RecalculateTotal();
    }

    private void RecalculateTotal()
    {
        TotalAmount = _details.Sum(detail => detail.Subtotal);
    }

    public void Approve()
    {
        if (Status != RentalStatus.Pending)
            throw new InvalidOperationException(
                "Only pending rentals can be approved.");

        if (_details.Count == 0)
            throw new InvalidOperationException(
                "A rental must have at least one detail to be approved.");

        Status = RentalStatus.Approved;
    }

    public void Start()
    {
        if (Status != RentalStatus.Approved)
            throw new InvalidOperationException(
                "Only approved rentals can be started.");

        Status = RentalStatus.Active;
    }

    public void Complete()
    {
        if (Status != RentalStatus.Active)
            throw new InvalidOperationException(
                "Only active rentals can be completed.");

        Status = RentalStatus.Completed;
    }

    public void Cancel()
    {
        if (Status == RentalStatus.Completed ||
            Status == RentalStatus.Cancelled)
        {
            throw new InvalidOperationException(
                "Completed or cancelled rentals cannot be cancelled.");
        }

        Status = RentalStatus.Cancelled;
    }
}