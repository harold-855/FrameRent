using FrameRent.Application.Interfaces.Repositories;
using FrameRent.Domain.Enums;

namespace FrameRent.Application.UseCases.Rentals.ApproveRental;

public class ApproveRentalUseCase
{
    private readonly IRentalRepository _rentalRepository;

    public ApproveRentalUseCase(IRentalRepository rentalRepository)
    {
        _rentalRepository = rentalRepository;
    }

    public async Task ExecuteAsync(Guid rentalId)
    {
        var rental = await _rentalRepository.GetByIdAsync(rentalId);
        if (rental is null)
            throw new KeyNotFoundException("Alquiler no encontrado.");

        if (rental.Status != RentalStatus.Pendiente)
            throw new InvalidOperationException("Solo se pueden aprobar alquileres pendientes.");

        rental.Approve();
        await _rentalRepository.UpdateAsync(rental);
    }
}
