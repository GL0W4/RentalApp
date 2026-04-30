namespace StarterApp.Core.Rentals.States;

public abstract class RentalStateBase : IRentalState
{
    public abstract string Status { get; }

    public abstract IReadOnlyCollection<string> AllowedTransitions { get; }

    public bool CanTransitionTo(string nextStatus)
    {
        return AllowedTransitions.Any(status =>
            string.Equals(status, nextStatus, StringComparison.OrdinalIgnoreCase));
    }
}