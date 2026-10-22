namespace FrameRent.Application.DTOs.Equipment;

public class CreateEquipmentRequest
{
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal DailyRate { get; set; }
    public int TotalUnits { get; set; }
}