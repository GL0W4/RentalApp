namespace StarterApp.Services;

/// <summary>
/// Provides the current device location used by nearby item search.
/// </summary>
public interface ILocationService
{
    /// <summary>Gets the best available location, with a development fallback in the implementation.</summary>
    Task<LocationResult> GetCurrentLocationAsync();
}
