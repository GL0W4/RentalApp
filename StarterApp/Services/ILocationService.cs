namespace StarterApp.Services;

public interface ILocationService
{
    Task<LocationResult> GetCurrentLocationAsync();
}