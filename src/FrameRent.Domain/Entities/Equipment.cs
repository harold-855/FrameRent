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
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(
                "The equipment name is required.",
                nameof(name));

        if (string.IsNullOrWhiteSpace(category))
            throw new ArgumentException(
                "The equipment category is required.",
                nameof(category));

        if (dailyRate < 0)
            throw new ArgumentOutOfRangeException(
                nameof(dailyRate),
                "The daily rate cannot be negative.");

        if (totalUnits <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(totalUnits),
                "The total units must be greater than zero.");

        Name = name.Trim();
        Category = category.Trim();
        Description = description?.Trim() ?? string.Empty;
        DailyRate = dailyRate;
        TotalUnits = totalUnits;
    }

    public void UpdateDailyRate(decimal newDailyRate)
    {
        if (newDailyRate < 0)
            throw new ArgumentOutOfRangeException(
                nameof(newDailyRate),
                "The daily rate cannot be negative.");

        DailyRate = newDailyRate;
    }

    public void UpdateTotalUnits(int newTotalUnits)
    {
        if (newTotalUnits <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(newTotalUnits),
                "The total units must be greater than zero.");

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