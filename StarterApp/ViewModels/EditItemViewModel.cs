using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Database.Models;
using StarterApp.Core.Items;
using StarterApp.Services;
using System.Globalization;

namespace StarterApp.ViewModels;

/// <summary>
/// ViewModel for editing an existing item listing owned by the authenticated user.
/// </summary>
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

    /// <summary>
    /// Creates the edit-item ViewModel with item workflow and navigation dependencies.
    /// </summary>
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

            var item = await _itemService.GetItemByIdAsync(itemId);

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

        // Validate locally before delegating ownership and persistence checks to the API.
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

        if (!ItemValidationRules.HasTitle(TitleText))
        {
            SetError("Title is required.");
            return false;
        }

        if (!ItemValidationRules.HasValidTitleLength(TitleText))
        {
            SetError("Title must be at least {ItemValidationRules.MinimumTitleLength} characters long.");
            return false;
        }

        if (string.IsNullOrWhiteSpace(DailyRateText) ||
            !decimal.TryParse(DailyRateText, NumberStyles.Number, CultureInfo.InvariantCulture, out dailyRate))
        {
            SetError("Daily rate must be a valid number.");
            return false;
        }

        if (!ItemValidationRules.IsValidDailyRate(dailyRate))
        {
            SetError("Daily rate must be greater than zero.");
            return false;
        }

        return true;
    }
}
