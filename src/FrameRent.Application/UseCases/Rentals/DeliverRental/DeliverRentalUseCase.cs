using FrameRent.Application.Interfaces.Repositories;
using FrameRent.Domain.Enums;

namespace FrameRent.Application.UseCases.Rentals.DeliverRental;

public class DeliverRentalUseCase
{
    private readonly IRentalRepository _rentalRepository;

    public DeliverRentalUseCase(IRentalRepository rentalRepository)
    {
        _rentalRepository = rentalRepository;
    }

    public async Task ExecuteAsync(Guid rentalId)
    {
        var rental = await _rentalRepository.GetByIdAsync(rentalId);
        if (rental is null)
            throw new KeyNotFoundException("Alquiler no encontrado.");

        if (rental.Status != RentalStatus.Aprobado)
            throw new InvalidOperationException("Solo se pueden entregar alquileres aprobados.");

        rental.Deliver();
        await _rentalRepository.UpdateAsync(rental);
    }
}
