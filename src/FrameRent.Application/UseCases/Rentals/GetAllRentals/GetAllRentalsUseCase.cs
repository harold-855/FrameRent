using FrameRent.Application.DTOs.Rentals;
using FrameRent.Application.Interfaces.Repositories;
using FrameRent.Domain.Enums;

namespace FrameRent.Application.UseCases.Rentals.GetAllRentals;

public class GetAllRentalsUseCase
{
    private readonly IRentalRepository _rentalRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IEquipmentRepository _equipmentRepository;

    public GetAllRentalsUseCase(
        IRentalRepository rentalRepository,
        ICustomerRepository customerRepository,
        IEquipmentRepository equipmentRepository)
    {
        _rentalRepository = rentalRepository;
        _customerRepository = customerRepository;
        _equipmentRepository = equipmentRepository;
    }

    public async Task<IReadOnlyCollection<RentalResponse>> ExecuteAsync(RentalStatus? status = null)
    {
        var rentals = await _rentalRepository.GetAllAsync(status);
        var result = new List<RentalResponse>();

        foreach (var rental in rentals)
        {
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

            var customerName = customer is null ? "Cliente" : $"{customer.FirstName} {customer.LastName}".Trim();

            result.Add(new RentalResponse(
                rental.Id,
                rental.CustomerId,
                customerName,
                customer?.Email ?? string.Empty,
                rental.StartDate,
                rental.EndDate,
                rental.TotalAmount,
                rental.Status.ToString(),
                detailResponses,
                penalties.Select(p => new PenaltyResponse(
                    p.Id,
                    p.Reason,
                    p.Amount,
                    p.CreatedAt,
                    p.IsPaid)).ToList()));
        }

        return result;
    }
}
