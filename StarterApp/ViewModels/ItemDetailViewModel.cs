using CommunityToolkit.Mvvm.Input;
using StarterApp.Database.Models;
using StarterApp.Services;
using System.ComponentModel;

namespace StarterApp.ViewModels;

public partial class ItemDetailViewModel : INotifyPropertyChanged
{
    private readonly IItemService _itemService;

    private int _itemId;
    private Item? _item;
    private bool _isLoading;
    private string _errorMessage = string.Empty;

    public ItemDetailViewModel(IItemService itemService)
    {
        _itemService = itemService;
    }

    public int ItemId
    {
        get => _itemId;
        set
        {
            _itemId = value;
            OnPropertyChanged();
            _ = Task.Run(LoadItemAsync);
        }
    }

    public Item? Item
    {
        get => _item;
        set
        {
            _item = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(PageTitle));
        }
    }

    public bool IsLoading
    {
        get => _isLoading;
        set
        {
            _isLoading = value;
            OnPropertyChanged();
        }
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        set
        {
            _errorMessage = value;
            OnPropertyChanged();
        }
    }

    public string PageTitle => Item?.Title ?? "Item Detail";

    private async Task LoadItemAsync()
    {
        System.Diagnostics.Debug.WriteLine($"ITEM DETAIL LOAD START: ItemId={ItemId}");

        IsLoading = true;
        ErrorMessage = string.Empty;

        try
        {
            var items = await _itemService.GetItemsAsync();
            Item = items.FirstOrDefault(i => i.Id == ItemId);

            if (Item == null)
            {
                ErrorMessage = "Item not found.";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error loading item: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
        System.Diagnostics.Debug.WriteLine($"ITEM DETAIL LOAD RESULT: {(Item == null ? "NULL" : Item.Title)}");
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}