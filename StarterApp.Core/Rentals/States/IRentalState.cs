namespace StarterApp.Core.Rentals.States;

/// <summary>
/// Represents a rental lifecycle state and the transitions allowed from it.
/// </summary>
public interface IRentalState
{
    /// <summary>Gets the display/API status value represented by this state.</summary>
    string Status { get; }

    /// <summary>Gets the statuses that can legally follow this state.</summary>
    IReadOnlyCollection<string> AllowedTransitions { get; }

    /// <summary>
    /// Checks whether the state permits a transition to the supplied status.
    /// </summary>
    bool CanTransitionTo(string nextStatus);
}
