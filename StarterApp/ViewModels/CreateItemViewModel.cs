using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Services;
using System.Collections.ObjectModel;

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
    private decimal dailyRate;

    [ObservableProperty]
    private double latitude = 55.9533;

    [ObservableProperty]
    private double longitude = -3.1883;

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

        if (!ValidateForm())
            return;

        try
        {
            IsBusy = true;
            ClearError();

            var request = new CreateItemRequest
            {
                Title = TitleText.Trim(),
                Description = string.IsNullOrWhiteSpace(Description) ? null : Description.Trim(),
                DailyRate = DailyRate,
                CategoryId = SelectedCategory!.Id,
                Latitude = Latitude,
                Longitude = Longitude
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

    private bool ValidateForm()
    {
        if (string.IsNullOrWhiteSpace(TitleText))
        {
            SetError("Title is required.");
            return false;
        }

        if (SelectedCategory == null)
        {
            SetError("Please select a category.");
            return false;
        }

        if (DailyRate <= 0)
        {
            SetError("Daily rate must be greater than 0.");
            return false;
        }

        if (Latitude < -90 || Latitude > 90)
        {
            SetError("Latitude must be between -90 and 90.");
            return false;
        }

        if (Longitude < -180 || Longitude > 180)
        {
            SetError("Longitude must be between -180 and 180.");
            return false;
        }

        return true;
    }
}