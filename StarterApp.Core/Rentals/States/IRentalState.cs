namespace StarterApp.Core.Rentals.States;

public interface IRentalState
{
    string Status { get; }

    IReadOnlyCollection<string> AllowedTransitions { get; }

    bool CanTransitionTo(string nextStatus);
}