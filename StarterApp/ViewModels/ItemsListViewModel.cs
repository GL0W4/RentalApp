/// @file ItemsListViewModel.cs
/// @brief Items list page view model
/// @author Alan Glowacz
/// @date 2025

using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace StarterApp.ViewModels;

public partial class ItemsListViewModel : BaseViewModel
{
    [ObservableProperty]
    private ObservableCollection<string> items = new();

    public ItemsListViewModel()
    {
        Title = "Items List";

        Items = new ObservableCollection<string>
        {
            "Item 1",
            "Item 2",
            "Item 3",
            "Item 4",
            "Item 5"
        };
    }
}