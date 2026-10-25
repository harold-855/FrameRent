using FrameRent.Domain.Enums;

namespace FrameRent.Domain.Entities;

public class Rental : BaseEntity
{
    public Guid CustomerId { get; private set; }

    public DateTime StartDate { get; private set; }

    public DateTime EndDate { get; private set; }

    public decimal TotalAmount { get; private set; }

    public RentalStatus Status { get; private set; } = RentalStatus.Pendiente;

    private readonly List<RentalDetail> _details = new();
    public IReadOnlyCollection<RentalDetail> Details => _details.AsReadOnly();

    private readonly List<Penalty> _penalties = new();
    public IReadOnlyCollection<Penalty> Penalties => _penalties.AsReadOnly();

    public Rental(
        Guid customerId,
        DateTime startDate,
        DateTime endDate)
    {
        if (customerId == Guid.Empty)
            throw new ArgumentException("El ID del cliente es obligatorio.", nameof(customerId));

        if (startDate.Date < DateTime.UtcNow.Date)
            throw new ArgumentException("La fecha de inicio no puede estar en el pasado.", nameof(startDate));

        if (endDate.Date <= startDate.Date)
            throw new ArgumentException("La fecha de fin debe ser posterior a la de inicio.", nameof(endDate));

        if ((endDate.Date - startDate.Date).Days > 15)
            throw new ArgumentException("Un alquiler dura como máximo 15 días.", nameof(endDate));

        CustomerId = customerId;
        StartDate = DateTime.SpecifyKind(startDate, DateTimeKind.Utc);
        EndDate = DateTime.SpecifyKind(endDate, DateTimeKind.Utc);
    }

    private Rental()
    {
    }

    public void AddDetail(
        Guid equipmentId,
        int quantity,
        decimal dailyRate)
    {
        if (Status != RentalStatus.Pendiente)
            throw new InvalidOperationException("Solo se pueden agregar detalles a alquileres en estado Pendiente.");

        int rentalDays = (EndDate.Date - StartDate.Date).Days;

        if (rentalDays <= 0)
            throw new InvalidOperationException("La duración del alquiler debe ser de al menos un día.");

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
        if (Status != RentalStatus.Pendiente)
            throw new InvalidOperationException("Solo se pueden aprobar alquileres que estén en estado Pendiente.");

        if (_details.Count == 0)
            throw new InvalidOperationException("El alquiler debe tener al menos un detalle para poder ser aprobado.");

        Status = RentalStatus.Aprobado;
    }

    public void Reject()
    {
        if (Status != RentalStatus.Pendiente)
            throw new InvalidOperationException("Solo se pueden rechazar alquileres que estén en estado Pendiente.");

        Status = RentalStatus.Rechazado;
    }

    public void Deliver()
    {
        if (Status != RentalStatus.Aprobado)
            throw new InvalidOperationException("Solo se pueden entregar alquileres que hayan sido previamente aprobados.");

        Status = RentalStatus.Entregado;
    }

    public void Return(DateTime actualReturnDate)
    {
        if (Status != RentalStatus.Entregado)
            throw new InvalidOperationException("Solo se pueden devolver alquileres que estén en estado Entregado.");

        Status = RentalStatus.Devuelto;

        if (actualReturnDate.Date > EndDate.Date)
        {
            int daysLate = (actualReturnDate.Date - EndDate.Date).Days;
            decimal penaltyAmount = 0;

            foreach (var detail in _details)
            {
                // Regla 9: días de retraso × tarifa diaria × cantidad × 1.5
                penaltyAmount += daysLate * detail.DailyRate * detail.Quantity * 1.5m;
            }

            if (penaltyAmount > 0)
            {
                AddPenalty($"Multa por retraso de {daysLate} día(s)", penaltyAmount);
            }
        }
    }

    public void Cancel(DateTime currentDate)
    {
        if (Status != RentalStatus.Pendiente && Status != RentalStatus.Aprobado)
            throw new InvalidOperationException("Solo se pueden cancelar alquileres en estado Pendiente o Aprobado.");

        if (currentDate.Date >= StartDate.Date)
            throw new InvalidOperationException("Un alquiler solo puede cancelarse antes de su fecha de inicio.");

        Status = RentalStatus.Cancelado;
    }

    public void AddPenalty(string reason, decimal amount)
    {
        var penalty = new Penalty(Id, reason, amount);
        _penalties.Add(penalty);
    }
}