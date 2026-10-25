using FrameRent.Application.Interfaces.Repositories;
using FrameRent.Domain.Enums;

namespace FrameRent.Application.UseCases.Rentals.ReturnRental;

public class ReturnRentalUseCase
{
    private readonly IRentalRepository _rentalRepository;

    public ReturnRentalUseCase(IRentalRepository rentalRepository)
    {
        _rentalRepository = rentalRepository;
    }

    public async Task ExecuteAsync(Guid rentalId, DateTime actualReturnDate)
    {
        var rental = await _rentalRepository.GetByIdAsync(rentalId);
        if (rental is null)
            throw new KeyNotFoundException("Alquiler no encontrado.");

        if (rental.Status != RentalStatus.Entregado)
            throw new InvalidOperationException("Solo se pueden devolver alquileres entregados.");

        rental.Return(actualReturnDate);
        await _rentalRepository.UpdateAsync(rental);
    }
}
