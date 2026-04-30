namespace StarterApp.Core.Rentals.States;

public static class RentalStateFactory
{
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