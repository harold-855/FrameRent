using FrameRent.Application.DTOs.Rentals;
using FrameRent.Application.Interfaces.Repositories;

namespace FrameRent.Application.UseCases.Rentals.GetRentalById;

public class GetRentalByIdUseCase
{
    private readonly IRentalRepository _rentalRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IEquipmentRepository _equipmentRepository;

    public GetRentalByIdUseCase(
        IRentalRepository rentalRepository,
        ICustomerRepository customerRepository,
        IEquipmentRepository equipmentRepository)
    {
        _rentalRepository = rentalRepository;
        _customerRepository = customerRepository;
        _equipmentRepository = equipmentRepository;
    }

    public async Task<RentalResponse?> ExecuteAsync(Guid rentalId)
    {
        var rental = await _rentalRepository.GetByIdAsync(rentalId);
        if (rental is null)
            return null;

        var customer = await _customerRepository.GetByIdAsync(rental.CustomerId);
        var details = await _rentalRepository.GetDetailsByRentalIdAsync(rental.Id);
        var penalties = await _rentalRepository.GetPenaltiesByRentalIdAsync(rental.Id);

        var detailResponses = new List<RentalDetailResponse>();
        foreach (var detail in details)
        {
            var equipment = await _equipmentRepository.GetByIdAsync(detail.EquipmentId);
            detailResponses.Add(new RentalDetailResponse(
                detail.Id,
                detail.EquipmentId,
                equipment?.Name ?? "Equipo no disponible",
                detail.Quantity,
                detail.DailyRate,
                detail.RentalDays,
                detail.Subtotal));
        }

        var penaltyResponses = penalties
            .Select(p => new PenaltyResponse(
                p.Id,
                p.Reason,
                p.Amount,
                p.CreatedAt,
                p.IsPaid))
            .ToList();

        var customerName = customer is null ? "Cliente" : $"{customer.FirstName} {customer.LastName}".Trim();

        return new RentalResponse(
            rental.Id,
            rental.CustomerId,
            customerName,
            customer?.Email ?? string.Empty,
            rental.StartDate,
            rental.EndDate,
            rental.TotalAmount,
            rental.Status.ToString(),
            detailResponses,
            penaltyResponses);
    }
}
