using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Database.Models;
using StarterApp.Services;
using System.Collections.ObjectModel;
using StarterApp.Core.Items;

namespace StarterApp.ViewModels;

public partial class ItemsListViewModel : BaseViewModel
{
    private readonly IItemService _itemService;
    private readonly INavigationService _navigationService;
    private readonly ILocationService _locationService;

    [ObservableProperty]
    private ObservableCollection<Item> items = new ObservableCollection<Item>();

    [ObservableProperty]
    private string radiusKmText = "10";

    [ObservableProperty]
    private bool isNearbyMode;

    [ObservableProperty]
    private string locationSummary = string.Empty;

    public ItemsListViewModel(IItemService itemService, INavigationService navigationService, ILocationService locationService)
    {
        _itemService = itemService;
        _navigationService = navigationService;
        _locationService = locationService;
        Title = "Items List";
    }

    [RelayCommand]
    private async Task NavigateToCreateItemAsync()
    {
        await _navigationService.NavigateToAsync("CreateItemPage");
    }

    [RelayCommand]
    public async Task LoadItemsAsync()
    {
        try
        {
            IsBusy = true;
            ClearError();

            var fetchedItems = await _itemService.GetItemsAsync();
            Items.Clear();

            foreach (var item in fetchedItems)
            {
                Items.Add(item);
            }

            IsNearbyMode = false;
            LocationSummary = string.Empty;
        }
        catch (Exception ex)
        {
            SetError($"Failed to load items: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task LoadNearbyItemsAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            ClearError();

            if (string.IsNullOrWhiteSpace(RadiusKmText) ||
                !double.TryParse(RadiusKmText, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out var radiusKm))
            {
                SetError("Radius must be a valid number.");
                return;
            }

            if (!ItemValidationRules.IsValidSearchRadius(radiusKm))
            {
                SetError($"Radius must be between 1 and {ItemValidationRules.MaximumSearchRadiusKm:F0} km.");
                return;
            }

            var location = await _locationService.GetCurrentLocationAsync();

            var nearbyItems = await _itemService.GetNearbyItemsAsync(
                location.Latitude,
                location.Longitude,
                radiusKm);

            Items.Clear();

            foreach (var item in nearbyItems)
            {
                Items.Add(item);
            }

            IsNearbyMode = true;
            LocationSummary = $"Showing items within {radiusKm:F0} km of {location.Latitude:F4}, {location.Longitude:F4}";
        }
        catch (Exception ex)
        {
            SetError($"Failed to load nearby items: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task OpenItemDetailAsync(Item item)
    {
        if (item == null)
            return;

        await Shell.Current.GoToAsync($"ItemDetailPage?itemId={item.Id}");
    }
}
