namespace FrameRent.Application.DTOs.Equipment;

public class UpdateEquipmentRequest
{
    public string Description { get; set; } = string.Empty;
    public decimal DailyRate { get; set; }
    public int TotalUnits { get; set; }
}