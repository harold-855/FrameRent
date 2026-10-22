using FrameRent.Domain.Entities;

namespace FrameRent.Application.Interfaces.Repositories;

public interface IEquipmentRepository
{
    Task AddAsync(Equipment equipment);

    Task<Equipment?> GetByIdAsync(Guid id);

    Task<IReadOnlyCollection<Equipment>> GetAllAsync();

    Task<bool> ExistsAsync(Guid id);
}