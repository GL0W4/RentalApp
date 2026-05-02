using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Core.Reviews;
using StarterApp.Services;

namespace StarterApp.ViewModels;

/// <summary>
/// ViewModel for submitting a review after a rental has been completed.
/// </summary>
[QueryProperty(nameof(RentalId), "rentalId")]
public partial class CreateReviewViewModel : BaseViewModel
{
    private readonly IReviewService _reviewService;

    [ObservableProperty]
    private int rentalId;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SubmitReviewCommand))]
    private int rating = 5;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SubmitReviewCommand))]
    private string comment = string.Empty;

    /// <summary>
    /// Creates the review ViewModel with the review workflow service.
    /// </summary>
    public CreateReviewViewModel(IReviewService reviewService)
    {
        _reviewService = reviewService;
        Title = "Leave Review";
    }

    private bool CanSubmitReview()
    {
        return !IsBusy
            && RentalId > 0
            && ReviewValidationRules.IsValidRating(Rating)
            && ReviewValidationRules.IsValidComment(Comment);
    }

    [RelayCommand(CanExecute = nameof(CanSubmitReview))]
    private async Task SubmitReviewAsync()
    {
        if (IsBusy)
            return;

        ClearError();

        // Local validation mirrors the API contract and gives immediate feedback.
        if (!ReviewValidationRules.IsValidRating(Rating))
        {
            SetError("Rating must be between 1 and 5.");
            return;
        }

        if (!ReviewValidationRules.IsValidComment(Comment))
        {
            SetError("Comment must be 500 characters or fewer.");
            return;
        }

        try
        {
            IsBusy = true;
            SubmitReviewCommand.NotifyCanExecuteChanged();

            await _reviewService.CreateReviewAsync(RentalId, Rating, Comment);

            await Shell.Current.DisplayAlertAsync(
                "Review submitted",
                "Your review has been submitted successfully.",
                "OK");

            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            SetError(ex.Message);
        }
        finally
        {
            IsBusy = false;
            SubmitReviewCommand.NotifyCanExecuteChanged();
        }
    }

    [RelayCommand]
    private async Task CancelAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}
