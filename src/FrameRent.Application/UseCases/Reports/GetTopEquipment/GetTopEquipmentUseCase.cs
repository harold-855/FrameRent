using FrameRent.Application.DTOs.Reports;
using FrameRent.Application.Interfaces.Repositories;

namespace FrameRent.Application.UseCases.Reports.GetTopEquipment;

public class GetTopEquipmentUseCase
{
    private readonly IRentalRepository _rentalRepository;

    public GetTopEquipmentUseCase(IRentalRepository rentalRepository)
    {
        _rentalRepository = rentalRepository;
    }

    public async Task<IReadOnlyCollection<TopEquipmentDto>> ExecuteAsync(int limit = 10)
    {
        return await _rentalRepository.GetTopRentedEquipmentAsync(limit);
    }
}
