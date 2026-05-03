namespace StarterApp.Services;

/// <summary>
/// Request body sent to the API when changing a rental workflow status.
/// </summary>
public class UpdateRentalStatusRequest
{
    /// <summary>Gets or sets the next requested rental status.</summary>
    public string Status { get; set; } = string.Empty;
}
