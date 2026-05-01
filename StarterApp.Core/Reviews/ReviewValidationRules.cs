namespace StarterApp.Core.Reviews;

public static class ReviewValidationRules
{
    public const int MinimumRating = 1;
    public const int MaximumRating = 5;
    public const int MaximumCommentLength = 500;

    public static bool IsValidRating(int rating)
    {
        return rating >= MinimumRating && rating <= MaximumRating;
    }

    public static bool IsValidComment(string? comment)
    {
        return string.IsNullOrEmpty(comment)
            || comment.Length <= MaximumCommentLength;
    }
}