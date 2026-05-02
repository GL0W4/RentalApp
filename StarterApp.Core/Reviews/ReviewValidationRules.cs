namespace StarterApp.Core.Reviews;

/// <summary>
/// Centralises review validation rules shared by ViewModels and services.
/// </summary>
public static class ReviewValidationRules
{
    /// <summary>Minimum permitted review rating.</summary>
    public const int MinimumRating = 1;
    /// <summary>Maximum permitted review rating.</summary>
    public const int MaximumRating = 5;
    /// <summary>Maximum permitted length for optional review comments.</summary>
    public const int MaximumCommentLength = 500;

    /// <summary>Checks that a review rating is within the accepted range.</summary>
    public static bool IsValidRating(int rating)
    {
        return rating >= MinimumRating && rating <= MaximumRating;
    }

    /// <summary>Checks that an optional comment satisfies the maximum length rule.</summary>
    public static bool IsValidComment(string? comment)
    {
        return string.IsNullOrEmpty(comment)
            || comment.Length <= MaximumCommentLength;
    }
}
