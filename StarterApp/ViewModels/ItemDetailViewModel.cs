using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Globalization;
using StarterApp.Database.Models;
using StarterApp.Services;
using System.Collections.ObjectModel;

namespace StarterApp.ViewModels;

/// <summary>
/// ViewModel for displaying a selected item, its reviews, and related item actions.
/// </summary>
[QueryProperty(nameof(ItemId), "itemId")]
public partial class ItemDetailViewModel : BaseViewModel
{
    private readonly IItemService _itemService;
    private readonly IReviewService _reviewService;


    [ObservableProperty]
    private int itemId;

    [ObservableProperty]
    private Item? item;

     [ObservableProperty]
    private ObservableCollection<ReviewItem> reviews = new();

    [ObservableProperty]
    private double averageRating;

    [ObservableProperty]
    private int totalReviews;

    /// <summary>Gets the title displayed by the item detail page.</summary>
    public string PageTitle => Item?.Title ?? "Item Detail";

    /// <summary>Gets whether review data is available for display.</summary>
    public bool HasReviews => TotalReviews > 0;

    /// <summary>Gets whether the no-reviews message should be displayed.</summary>
    public bool HasNoReviews => !HasReviews;

    /// <summary>
    /// Creates the item detail ViewModel with item and review workflow services.
    /// </summary>
    public ItemDetailViewModel(IItemService itemService, IReviewService reviewService)
    {
        _itemService = itemService;
        _reviewService = reviewService;
        Title = "Item Detail";
    }

    partial void OnItemIdChanged(int value)
    {
        _ = LoadItemAsync(value);
    }

    partial void OnItemChanged(Item? value)
    {
        OnPropertyChanged(nameof(PageTitle));
    }

    partial void OnTotalReviewsChanged(int value)
    {
        OnPropertyChanged(nameof(HasReviews));
        OnPropertyChanged(nameof(HasNoReviews));
    }

    private async Task LoadItemAsync(int itemId)
    {
        try
        {
            IsBusy = true;
            ClearError();

            Item = await _itemService.GetItemByIdAsync(itemId);

            if (Item == null)
            {
                SetError("Item not found.");
                return;
            }

            await LoadReviewsAsync(itemId);
        }
        catch (Exception ex)
        {
            SetError($"Failed to load item: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task NavigateToEditItemAsync()
    {
        if (Item == null)
            return;

        await Shell.Current.GoToAsync($"EditItemPage?itemId={Item.Id}");
    }

    private async Task LoadReviewsAsync(int itemId)
    {
        var result = await _reviewService.GetItemReviewsAsync(itemId);

        Reviews.Clear();

        foreach (var review in result.Reviews)
        {
            Reviews.Add(review);
        }

        AverageRating = result.AverageRating ?? 0;
        TotalReviews = result.TotalReviews;
    }

    [RelayCommand]
    private async Task NavigateToRentalRequestAsync()
    {
        if (Item == null)
            return;

        // AI-assisted: pass the current item's daily rate to the rental request page for client-side estimates.
        var dailyRate = Item.DailyRate.ToString(CultureInfo.InvariantCulture);
        await Shell.Current.GoToAsync(
            $"CreateRentalRequestPage?itemId={Item.Id}&dailyRate={dailyRate}");
    }

    
}
