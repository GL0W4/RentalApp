using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Database.Models;
using StarterApp.Services;
using System.Collections.ObjectModel;



namespace StarterApp.ViewModels;

public partial class ItemsListViewModel : BaseViewModel
{
    private readonly IItemService _itemService;
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private ObservableCollection<Item> items = new ObservableCollection<Item>();


    public ItemsListViewModel(IItemService itemService, INavigationService navigationService)
    {
        _itemService = itemService;
        _navigationService = navigationService;
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
    private async Task OpenItemDetailAsync(Item item)
    {
        if (item == null)
            return;

        await Shell.Current.GoToAsync($"ItemDetailPage?itemId={item.Id}");
    }

}
