namespace StarterApp.Core.Items;

public static class ItemValidationRules
{
    public const int MinimumTitleLength = 5;
    public const double MaximumSearchRadiusKm = 50;

    public static bool HasTitle(string? title)
    {
        return !string.IsNullOrWhiteSpace(title);
    }

    public static bool HasValidTitleLength(string? title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            return false;
        }
        return HasTitle(title) && title.Trim().Length >= MinimumTitleLength;
    }

    public static bool IsValidDailyRate(decimal dailyRate)
    {
        return dailyRate > 0;
    }

    public static bool IsValidLatitude(double latitude)
    {
        return latitude >= -90 && latitude <= 90;
    }

    public static bool IsValidLongitude(double longitude)
    {
        return longitude >= -180 && longitude <= 180;
    }

    public static bool IsValidSearchRadius(double radiusKm)
    {
        return radiusKm > 0 && radiusKm <= MaximumSearchRadiusKm;
    }
}