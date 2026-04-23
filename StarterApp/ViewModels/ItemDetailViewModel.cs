/// @file ItemDetailViewModel.cs
/// @brief View model for displaying a single item
/// @author Alan Glowacz
/// @date 2025

using CommunityToolkit.Mvvm.ComponentModel;
using StarterApp.Database.Models;

namespace StarterApp.ViewModels;

public partial class ItemDetailViewModel : BaseViewModel
{
    private readonly SelectedItemService _selectedItemService;

    [ObservableProperty]
    private Item? item;

    public ItemDetailViewModel(SelectedItemService selectedItemService)
    {
        _selectedItemService = selectedItemService;
        Title = "Item Detail";
        Item = _selectedItemService.SelectedItem;

        if (Item != null)
        {
            Title = Item.Title;
        }
    }

    public void SetItem(Item selectedItem)
    {
        Item = selectedItem;
        Title = selectedItem.Title;
    }
}