using CommunityToolkit.Mvvm.ComponentModel;
using StarterApp.Database.Models;
using StarterApp.Services;

namespace StarterApp.ViewModels;

[QueryProperty(nameof(ItemId), "itemId")]
public partial class ItemDetailViewModel : BaseViewModel
{
    private readonly IItemService _itemService;

    [ObservableProperty]
    private int itemId;

    [ObservableProperty]
    private Item? item;

    public string PageTitle => Item?.Title ?? "Item Detail";

    public ItemDetailViewModel(IItemService itemService)
    {
        _itemService = itemService;
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
}