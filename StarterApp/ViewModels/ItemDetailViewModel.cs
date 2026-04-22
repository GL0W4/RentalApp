/// @file ItemDetailViewModel.cs
/// @brief View model for displaying a single item
/// @author Alan Glowacz
/// @date 2025

using CommunityToolkit.Mvvm.ComponentModel;
using StarterApp.Database.Models;

namespace StarterApp.ViewModels;

public partial class ItemDetailViewModel : BaseViewModel
{
    [ObservableProperty]
    private Item? item;

    public ItemDetailViewModel()
    {
        Title = "Item Detail";
    }

    public void SetItem(Item selectedItem)
    {
        Item = selectedItem;
        Title = selectedItem.Title;
    }
}