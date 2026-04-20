using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Database.Models;
using StarterApp.Services;
using System.Collections.ObjectModel;

namespace StarterApp.ViewModels;

public partial class ItemsListViewModel : BaseViewModel
{
    private readonly IItemService _itemService;

    [ObservableProperty]
    private ObservableCollection<Item> items = new();

    public ItemsListViewModel()
    {
        Title = "Items List";
    }

    public ItemsListViewModel(IItemService itemService)
    {
        _itemService = itemService;
        Title = "Items List";
    }

    [RelayCommand]
    private async Task LoadItemsAsync()
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
}