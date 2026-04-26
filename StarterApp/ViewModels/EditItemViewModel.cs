using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Database.Models;
using StarterApp.Services;
using System.Globalization;

namespace StarterApp.ViewModels;

[QueryProperty(nameof(ItemId), "itemId")]
public partial class EditItemViewModel : BaseViewModel
{
    private readonly IItemService _itemService;
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private int itemId;

    [ObservableProperty]
    private string titleText = string.Empty;

    [ObservableProperty]
    private string description = string.Empty;

    [ObservableProperty]
    private string dailyRateText = string.Empty;

    [ObservableProperty]
    private bool isAvailable = true;

    public EditItemViewModel(IItemService itemService, INavigationService navigationService)
    {
        _itemService = itemService;
        _navigationService = navigationService;
        Title = "Edit Item";
    }

    partial void OnItemIdChanged(int value)
    {
        _ = LoadItemAsync(value);
    }

    private async Task LoadItemAsync(int itemId)
    {
        try
        {
            IsBusy = true;
            ClearError();

            var items = await _itemService.GetItemsAsync();
            var item = items.FirstOrDefault(i => i.Id == itemId);

            if (item == null)
            {
                SetError("Item not found.");
                return;
            }

            TitleText = item.Title;
            Description = item.Description;
            DailyRateText = item.DailyRate.ToString(CultureInfo.InvariantCulture);
            IsAvailable = item.IsAvailable;
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

    [RelayCommand]
    private async Task SaveItemAsync()
    {
        if (IsBusy)
            return;

        if (!ValidateForm(out var dailyRate))
            return;

        try
        {
            IsBusy = true;
            ClearError();

            var request = new UpdateItemRequest
            {
                Title = TitleText.Trim(),
                Description = string.IsNullOrWhiteSpace(Description) ? null : Description.Trim(),
                DailyRate = dailyRate,
                IsAvailable = IsAvailable
            };

            await _itemService.UpdateItemAsync(ItemId, request);

            await Application.Current!.Windows[0].Page!.DisplayAlertAsync(
                "Success",
                "Item updated successfully.",
                "OK");

            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            SetError($"Failed to update item: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task CancelAsync()
    {
        await Shell.Current.GoToAsync("..");
    }

    private bool ValidateForm(out decimal dailyRate)
    {
        dailyRate = 0;

        if (string.IsNullOrWhiteSpace(TitleText))
        {
            SetError("Title is required.");
            return false;
        }

        if (TitleText.Trim().Length < 5)
        {
            SetError("Title must be at least 5 characters long.");
            return false;
        }

        if (string.IsNullOrWhiteSpace(DailyRateText) ||
            !decimal.TryParse(DailyRateText, NumberStyles.Number, CultureInfo.InvariantCulture, out dailyRate))
        {
            SetError("Daily rate must be a valid number.");
            return false;
        }

        if (dailyRate <= 0)
        {
            SetError("Daily rate must be greater than zero.");
            return false;
        }

        return true;
    }
}