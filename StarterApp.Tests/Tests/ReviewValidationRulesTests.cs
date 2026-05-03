using StarterApp.Core.Reviews;

namespace StarterApp.Tests.Tests;

public class ReviewValidationRulesTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(3)]
    [InlineData(5)]
    public void IsValidRating_ReturnsTrue_ForRatingsBetweenOneAndFive(int rating)
    {
        var result = ReviewValidationRules.IsValidRating(rating);

        Assert.True(result);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(6)]
    [InlineData(-1)]
    public void IsValidRating_ReturnsFalse_ForRatingsOutsideOneToFive(int rating)
    {
        var result = ReviewValidationRules.IsValidRating(rating);

        Assert.False(result);
    }

    [Fact]
    public void IsValidComment_ReturnsTrue_ForNullComment()
    {
        var result = ReviewValidationRules.IsValidComment(null);

        Assert.True(result);
    }

    [Fact]
    public void IsValidComment_ReturnsTrue_ForEmptyComment()
    {
        var result = ReviewValidationRules.IsValidComment(string.Empty);

        Assert.True(result);
    }

    [Fact]
    public void IsValidComment_ReturnsTrue_ForFiveHundredCharacterComment()
    {
        var comment = new string('a', ReviewValidationRules.MaximumCommentLength);

        var result = ReviewValidationRules.IsValidComment(comment);

        Assert.True(result);
    }

    [Fact]
    public void IsValidComment_ReturnsFalse_ForCommentLongerThanFiveHundredCharacters()
    {
        var comment = new string('a', ReviewValidationRules.MaximumCommentLength + 1);

        var result = ReviewValidationRules.IsValidComment(comment);

        Assert.False(result);
    }
}