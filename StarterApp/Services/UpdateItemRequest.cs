namespace StarterApp.Services;

public class UpdateItemRequest
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal DailyRate { get; set; }
    public bool IsAvailable { get; set; }
}