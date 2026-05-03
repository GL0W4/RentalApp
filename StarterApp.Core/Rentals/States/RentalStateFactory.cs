namespace StarterApp.Core.Rentals.States;

/// <summary>
/// Creates rental state objects from API status text.
/// </summary>
public static class RentalStateFactory
{
    /// <summary>
    /// Maps a status string to the matching state-pattern implementation.
    /// </summary>
    public static IRentalState? Create(string? status)
    {
        return status?.Trim().ToLowerInvariant() switch
        {
            "requested" => new RequestedState(),
            "approved" => new ApprovedState(),
            "rejected" => new RejectedState(),
            "out for rent" => new OutForRentState(),
            "overdue" => new OverdueState(),
            "returned" => new ReturnedState(),
            "completed" => new CompletedState(),
            _ => null
        };
    }
}
