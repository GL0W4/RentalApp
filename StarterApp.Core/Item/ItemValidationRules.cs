namespace StarterApp.Core.Items;

/// <summary>
/// Centralises item validation rules shared by ViewModels and services.
/// </summary>
public static class ItemValidationRules
{
    /// <summary>Minimum title length accepted by the app and API contract.</summary>
    public const int MinimumTitleLength = 5;
    /// <summary>Maximum radius allowed for nearby item search.</summary>
    public const double MaximumSearchRadiusKm = 50;

    /// <summary>Checks that a title has been supplied.</summary>
    public static bool HasTitle(string? title)
    {
        return !string.IsNullOrWhiteSpace(title);
    }

    /// <summary>Checks that a supplied title meets the minimum length requirement.</summary>
    public static bool HasValidTitleLength(string? title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            return false;
        }
        return HasTitle(title) && title.Trim().Length >= MinimumTitleLength;
    }

    /// <summary>Checks that the daily rental rate is positive.</summary>
    public static bool IsValidDailyRate(decimal dailyRate)
    {
        return dailyRate > 0;
    }

    /// <summary>Checks that a latitude value is within valid geographic bounds.</summary>
    public static bool IsValidLatitude(double latitude)
    {
        return latitude >= -90 && latitude <= 90;
    }

    /// <summary>Checks that a longitude value is within valid geographic bounds.</summary>
    public static bool IsValidLongitude(double longitude)
    {
        return longitude >= -180 && longitude <= 180;
    }

    /// <summary>Checks that a nearby-search radius is within the supported range.</summary>
    public static bool IsValidSearchRadius(double radiusKm)
    {
        return radiusKm > 0 && radiusKm <= MaximumSearchRadiusKm;
    }
}
