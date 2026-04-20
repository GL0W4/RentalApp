namespace StarterApp.Database.Models;

public class Item
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal DailyRate { get; set; }
    public string Category { get; set; } = string.Empty;
    public string OwnerName { get; set; } = string.Empty;
    public bool IsAvailable { get; set; }
    public string? ImageUrl { get; set; }
}