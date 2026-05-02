namespace StarterApp.Core.Rentals.States;

/// <summary>
/// Rental state used while the item is with the borrower.
/// </summary>
public sealed class OutForRentState : RentalStateBase
{
    /// <inheritdoc />
    public override string Status => RentalStatuses.OutForRent;

    /// <inheritdoc />
    public override IReadOnlyCollection<string> AllowedTransitions =>
        new[]
        {
            RentalStatuses.Returned
        };
}
