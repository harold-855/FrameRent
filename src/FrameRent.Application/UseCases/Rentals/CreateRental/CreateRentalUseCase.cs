using FrameRent.Application.DTOs.Rentals;
using FrameRent.Application.Interfaces.Repositories;
using FrameRent.Domain.Entities;

namespace FrameRent.Application.UseCases.Rentals.CreateRental;

public class CreateRentalUseCase
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IEquipmentRepository _equipmentRepository;
    private readonly IRentalRepository _rentalRepository;

    public CreateRentalUseCase(
        ICustomerRepository customerRepository,
        IEquipmentRepository equipmentRepository,
        IRentalRepository rentalRepository)
    {
        _customerRepository = customerRepository;
        _equipmentRepository = equipmentRepository;
        _rentalRepository = rentalRepository;
    }

    public async Task<Guid> ExecuteAsync(Guid customerId, CreateRentalRequest request)
    {
        var customer = await _customerRepository.GetByIdAsync(customerId);
        if (customer is null)
            throw new KeyNotFoundException("Cliente no encontrado.");

        if (request.Quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(request.Quantity), "La cantidad debe ser mayor que cero.");

        if (request.StartDate.Date < DateTime.UtcNow.Date)
            throw new ArgumentException("La fecha de inicio no puede estar en el pasado.", nameof(request.StartDate));

        if (request.EndDate <= request.StartDate)
            throw new ArgumentException("La fecha final debe ser posterior a la inicial.", nameof(request.EndDate));

        var durationDays = (request.EndDate.Date - request.StartDate.Date).Days;
        if (durationDays > 15)
            throw new ArgumentException("Un alquiler dura como máximo 15 días.", nameof(request.EndDate));

        var equipment = await _equipmentRepository.GetByIdAsync(request.EquipmentId);
        if (equipment is null)
            throw new KeyNotFoundException("Equipo no encontrado.");

        if (!equipment.IsActive)
            throw new InvalidOperationException("El equipo no está activo y no puede alquilarse.");

        var activePendingCount = await _rentalRepository.CountActiveOrPendingByCustomerIdAsync(customerId);
        if (activePendingCount >= 3)
            throw new InvalidOperationException("Un cliente no puede tener más de 3 alquileres pendientes o aprobados al mismo tiempo.");

        var rentedUnits = await _equipmentRepository.GetRentedUnitsInDateRangeAsync(
            request.EquipmentId,
            request.StartDate,
            request.EndDate);

        var availableUnits = equipment.TotalUnits - rentedUnits;
        if (request.Quantity > availableUnits)
            throw new InvalidOperationException("No hay suficientes unidades disponibles para ese rango de fechas.");

        var rental = new Rental(customerId, request.StartDate, request.EndDate);
        rental.AddDetail(request.EquipmentId, request.Quantity, equipment.DailyRate);

        await _rentalRepository.AddAsync(rental);
        return rental.Id;
    }
}
