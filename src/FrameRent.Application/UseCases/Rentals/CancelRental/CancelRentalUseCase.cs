using FrameRent.Application.Interfaces.Repositories;

namespace FrameRent.Application.UseCases.Rentals.CancelRental;

public class CancelRentalUseCase
{
    private readonly IRentalRepository _rentalRepository;

    public CancelRentalUseCase(IRentalRepository rentalRepository)
    {
        _rentalRepository = rentalRepository;
    }

    public async Task ExecuteAsync(Guid rentalId, Guid customerId, bool isStaff)
    {
        var rental = await _rentalRepository.GetByIdAsync(rentalId);
        if (rental is null)
            throw new KeyNotFoundException("Alquiler no encontrado.");

        if (!isStaff && rental.CustomerId != customerId)
            throw new UnauthorizedAccessException("No puedes cancelar un alquiler que no te pertenece.");

        rental.Cancel(DateTime.UtcNow);
        await _rentalRepository.UpdateAsync(rental);
    }
}
