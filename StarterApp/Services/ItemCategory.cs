namespace StarterApp.Services;

/// <summary>
/// Category option returned by the API for item creation and filtering.
/// </summary>
public class ItemCategory
{
    /// <summary>Gets or sets the API category identifier.</summary>
    public int Id { get; set; }
    /// <summary>Gets or sets the category display name.</summary>
    public string Name { get; set; } = string.Empty;
}
