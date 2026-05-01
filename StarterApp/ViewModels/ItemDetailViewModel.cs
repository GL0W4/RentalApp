using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Globalization;
using StarterApp.Database.Models;
using StarterApp.Services;
using System.Collections.ObjectModel;

namespace StarterApp.ViewModels;

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

    public string PageTitle => Item?.Title ?? "Item Detail";

    public bool HasReviews => TotalReviews > 0;

    public bool HasNoReviews => !HasReviews;

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

            var items = await _itemService.GetItemsAsync();
            Item = items.FirstOrDefault(i => i.Id == itemId);

            if (Item == null)
            {
                SetError("Item not found.");
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

        AverageRating = result.AverageRating;
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
