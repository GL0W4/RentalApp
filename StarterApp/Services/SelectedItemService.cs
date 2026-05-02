using StarterApp.Database.Models;

namespace StarterApp.Services;

/// <summary>
/// Simple shared state holder for passing a selected item between views when needed.
/// </summary>
public class SelectedItemService
{
    /// <summary>Gets or sets the currently selected item.</summary>
    public Item? SelectedItem { get; set; }
}
