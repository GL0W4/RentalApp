using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Database.Models;
using StarterApp.Services;
using System.Collections.ObjectModel;
using StarterApp.Core.Items;

namespace StarterApp.ViewModels;

/// <summary>
/// ViewModel for browsing all items and performing nearby item discovery.
/// </summary>
public partial class ItemsListViewModel : BaseViewModel
{
    private readonly IItemService _itemService;
    private readonly INavigationService _navigationService;
    private readonly ILocationService _locationService;

    [ObservableProperty]
    private ObservableCollection<Item> items = new ObservableCollection<Item>();

    [ObservableProperty]
    private double radiusKm = 10;

    [ObservableProperty]
    private bool isNearbyMode;

    [ObservableProperty]
    private string locationSummary = string.Empty;

    private const double MinimumSearchRadiusKm = 1;

    /// <summary>
    /// Creates the item list ViewModel with item, navigation, and location services.
    /// </summary>
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
    private void DecreaseRadius()
    {
        RadiusKm = Math.Max(MinimumSearchRadiusKm, Math.Round(RadiusKm) - 1);
    }

    [RelayCommand]
    private void IncreaseRadius()
    {
        RadiusKm = Math.Min(ItemValidationRules.MaximumSearchRadiusKm, Math.Round(RadiusKm) + 1);
    }

    /// <summary>
    /// Loads all item listings from the hosted API.
    /// </summary>
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

            var radiusKm = RadiusKm;

            if (!ItemValidationRules.IsValidSearchRadius(radiusKm))
            {
                SetError($"Radius must be between 1 and {ItemValidationRules.MaximumSearchRadiusKm:F0} km.");
                return;
            }

            // Location lookup is abstracted to support both device GPS and the development fallback.
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
