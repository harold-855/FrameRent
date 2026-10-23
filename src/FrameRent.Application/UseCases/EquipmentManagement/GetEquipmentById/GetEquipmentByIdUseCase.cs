using FrameRent.Application.Interfaces.Repositories;
using FrameRent.Domain.Entities;

namespace FrameRent.Application.UseCases.GetEquipmentById;

public class GetEquipmentByIdUseCase
{
    private readonly IEquipmentRepository _equipmentRepository;

    public GetEquipmentByIdUseCase(
        IEquipmentRepository equipmentRepository)
    {
        _equipmentRepository = equipmentRepository;
    }

    public async Task<Equipment?> ExecuteAsync(Guid id)
    {
        return await _equipmentRepository.GetByIdAsync(id);
    }
}