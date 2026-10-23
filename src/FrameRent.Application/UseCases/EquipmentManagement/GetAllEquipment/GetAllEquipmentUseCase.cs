using FrameRent.Application.Interfaces.Repositories;
using FrameRent.Domain.Entities;

namespace FrameRent.Application.UseCases.EquipmentManagement.GetAllEquipment;

public class GetAllEquipmentUseCase
{
    private readonly IEquipmentRepository _equipmentRepository;

    public GetAllEquipmentUseCase(
        IEquipmentRepository equipmentRepository)
    {
        _equipmentRepository = equipmentRepository;
    }

    public async Task<IReadOnlyCollection<Equipment>> ExecuteAsync()
    {
        return await _equipmentRepository.GetAllAsync();
    } 
}