using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Core.Items;
using StarterApp.Services;
using System.Collections.ObjectModel;
using System.Globalization;

namespace StarterApp.ViewModels;

public partial class CreateItemViewModel : BaseViewModel
{
    private readonly INavigationService _navigationService;
    private readonly IItemService _itemService;

    [ObservableProperty]
    private string titleText = string.Empty;

    [ObservableProperty]
    private string description = string.Empty;

    [ObservableProperty]
    private ObservableCollection<ItemCategory> categories = new();

    [ObservableProperty]
    private ItemCategory? selectedCategory;

    [ObservableProperty]
    private string dailyRateText = string.Empty;

    [ObservableProperty]
    private string latitudeText = "55.9533";

    [ObservableProperty]
    private string longitudeText = "-3.1883";

    public CreateItemViewModel(IItemService itemService, INavigationService navigationService)
    {
        _itemService = itemService;
        _navigationService = navigationService;
        Title = "Create Item";

        _ = Task.Run(LoadCategoriesAsync);
    }

    private async Task LoadCategoriesAsync()
    {
        try
        {
            var fetchedCategories = await _itemService.GetCategoriesAsync();

            MainThread.BeginInvokeOnMainThread(() =>
            {
                Categories.Clear();
                foreach (var category in fetchedCategories)
                {
                    Categories.Add(category);
                }
            });
        }
        catch (Exception ex)
        {
            SetError($"Failed to load categories: {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task SaveItemAsync()
    {
        if (IsBusy)
            return;

        if (!ValidateForm(out var dailyRate, out var latitude, out var longitude))
            return;

        try
        {
            IsBusy = true;
            ClearError();

            var request = new CreateItemRequest
            {
                Title = TitleText.Trim(),
                Description = string.IsNullOrWhiteSpace(Description) ? null : Description.Trim(),
                DailyRate = dailyRate,
                CategoryId = SelectedCategory!.Id,
                Latitude = latitude,
                Longitude = longitude
            };

            await _itemService.AddItemAsync(request);

            await Application.Current!.Windows[0].Page!.DisplayAlertAsync(
                "Success",
                "Item created successfully.",
                "OK");

            await _navigationService.NavigateToAsync("ItemsListPage");
        }
        catch (Exception ex)
        {
            SetError($"Failed to create item: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task CancelAsync()
    {
        await _navigationService.NavigateToAsync("ItemsListPage");
    }

    private bool ValidateForm(out decimal dailyRate, out double latitude, out double longitude)
    {
        dailyRate = 0;
        latitude = 0;
        longitude = 0;

        if (!ItemValidationRules.HasTitle(TitleText))
        {
            SetError("Title is required.");
            return false;
        }

        if (!ItemValidationRules.HasValidTitleLength(TitleText))
        {
            SetError("Title must be at least {ItemValidationRules.MinimumTitleLength} characters long.");
            return false;
        }

        if (SelectedCategory == null)
        {
            SetError("Please select a category.");
            return false;
        }

        if (string.IsNullOrWhiteSpace(DailyRateText) || 
            !decimal.TryParse(DailyRateText, NumberStyles.Number, CultureInfo.InvariantCulture, out dailyRate))
        {
            SetError("Daily rate must be a valid number.");
            return false;
        }

        if (!ItemValidationRules.IsValidDailyRate(dailyRate))
        {
            SetError("Daily rate must be greater than zero.");
            return false;
        }

        if (string.IsNullOrWhiteSpace(LatitudeText) ||
        !double.TryParse(LatitudeText, NumberStyles.Float | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out latitude))
        {
            SetError("Latitude must be a valid number.");
            return false;
        }

        if (!ItemValidationRules.IsValidLatitude(latitude))
        {
            SetError("Latitude must be between -90 and 90.");
            return false;
        }

        if (string.IsNullOrWhiteSpace(LongitudeText) ||
        !double.TryParse(LongitudeText, NumberStyles.Float | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out longitude))
        {
            SetError("Longitude must be a valid number.");
            return false;
        }

        if (!ItemValidationRules.IsValidLongitude(longitude))
        {
            SetError("Longitude must be between -180 and 180.");
            return false;
        }

        return true;
    }
}
