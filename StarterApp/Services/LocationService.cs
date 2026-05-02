namespace StarterApp.Services;



/// <summary>
/// Retrieves device location for nearby item discovery.
/// </summary>
public class LocationService : ILocationService
{
    private static readonly bool UseDevelopmentLocation = true;

    /// <inheritdoc />
    public async Task<LocationResult> GetCurrentLocationAsync()
    {
        if (UseDevelopmentLocation)
        {
            // Return a fixed location for development/testing.
            return new LocationResult
            {
                Latitude = 55.9533,
                Longitude = -3.1883
            };
        }

        try
        {
            var request = new GeolocationRequest(
                GeolocationAccuracy.Medium,
                TimeSpan.FromSeconds(10));

            var location = await Geolocation.Default.GetLocationAsync(request);

            if (location != null)
            {
                return new LocationResult
                {
                    Latitude = location.Latitude,
                    Longitude = location.Longitude
                };
            }
        }
        catch
        {
            // Emulator/device location can fail during development.
            // Fallback keeps the nearby feature testable.
        }

        return new LocationResult
        {
            Latitude = 55.9533,
            Longitude = -3.1883
        };
    }
}
