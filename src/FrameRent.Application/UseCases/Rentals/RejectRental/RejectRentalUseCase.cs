using FrameRent.Application.Interfaces.Repositories;
using FrameRent.Domain.Enums;

namespace FrameRent.Application.UseCases.Rentals.RejectRental;

public class RejectRentalUseCase
{
    private readonly IRentalRepository _rentalRepository;

    public RejectRentalUseCase(IRentalRepository rentalRepository)
    {
        _rentalRepository = rentalRepository;
    }

    public async Task ExecuteAsync(Guid rentalId)
    {
        var rental = await _rentalRepository.GetByIdAsync(rentalId);
        if (rental is null)
            throw new KeyNotFoundException("Alquiler no encontrado.");

        if (rental.Status != RentalStatus.Pendiente)
            throw new InvalidOperationException("Solo se pueden rechazar alquileres pendientes.");

        rental.Reject();
        await _rentalRepository.UpdateAsync(rental);
    }
}
