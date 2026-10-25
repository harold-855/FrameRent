namespace FrameRent.Domain.Entities;

public class Equipment : BaseEntity
{
    public string Name { get; private set; } = string.Empty;

    public string Category { get; private set; } = string.Empty;

    public string Description { get; private set; } = string.Empty;

    public decimal DailyRate { get; private set; }

    public int TotalUnits { get; private set; }

    public bool IsActive { get; private set; } = true;

    public Equipment(
        string name,
        string category,
        string description,
        decimal dailyRate,
        int totalUnits)
    {
        ValidateAndSet(name, category, description, dailyRate, totalUnits);
    }

    private Equipment()
    {
    }

    public void Update(
        string name,
        string category,
        string description,
        decimal dailyRate,
        int totalUnits)
    {
        ValidateAndSet(name, category, description, dailyRate, totalUnits);
    }

    private void ValidateAndSet(
        string name,
        string category,
        string description,
        decimal dailyRate,
        int totalUnits)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre del equipo es obligatorio.", nameof(name));

        if (string.IsNullOrWhiteSpace(category))
            throw new ArgumentException("La categoría del equipo es obligatoria.", nameof(category));

        if (dailyRate < 0)
            throw new ArgumentOutOfRangeException(nameof(dailyRate), "La tarifa diaria no puede ser negativa.");

        if (totalUnits <= 0)
            throw new ArgumentOutOfRangeException(nameof(totalUnits), "El total de unidades debe ser mayor a cero.");

        Name = name.Trim();
        Category = category.Trim();
        Description = description?.Trim() ?? string.Empty;
        DailyRate = dailyRate;
        TotalUnits = totalUnits;
    }

    public void UpdateDailyRate(decimal newDailyRate)
    {
        if (newDailyRate < 0)
            throw new ArgumentOutOfRangeException(nameof(newDailyRate), "La tarifa diaria no puede ser negativa.");

        DailyRate = newDailyRate;
    }

    public void UpdateTotalUnits(int newTotalUnits)
    {
        if (newTotalUnits <= 0)
            throw new ArgumentOutOfRangeException(nameof(newTotalUnits), "El total de unidades debe ser mayor a cero.");

        TotalUnits = newTotalUnits;
    }

    public void UpdateDescription(string description)
    {
        Description = description?.Trim() ?? string.Empty;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void Activate()
    {
        IsActive = true;
    }
}