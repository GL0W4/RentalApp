namespace StarterApp.Core.Rentals.States;

/// <summary>
/// Base implementation for state-pattern classes used by rental workflow rules.
/// </summary>
public abstract class RentalStateBase : IRentalState
{
    /// <inheritdoc />
    public abstract string Status { get; }

    /// <inheritdoc />
    public abstract IReadOnlyCollection<string> AllowedTransitions { get; }

    /// <inheritdoc />
    public bool CanTransitionTo(string nextStatus)
    {
        return AllowedTransitions.Any(status =>
            string.Equals(status, nextStatus, StringComparison.OrdinalIgnoreCase));
    }
}
