using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Services;
using System.Globalization;
using StarterApp.Core.Rentals;

namespace StarterApp.ViewModels;

/// <summary>
/// ViewModel for submitting a rental request for a selected item.
/// </summary>
[QueryProperty(nameof(ItemId), "itemId")]
[QueryProperty(nameof(DailyRateText), "dailyRate")]
public partial class CreateRentalRequestViewModel : BaseViewModel
{
    private readonly IRentalService _rentalService;

    [ObservableProperty]
    private int itemId;

    // AI-assisted: daily rate is supplied by Shell query and used only for the local estimate.
    // Reviewed and modified to string to allow passing the raw value from the query without parsing issues, and then parsed client-side for calculations.
    [ObservableProperty]
    private string dailyRateText = string.Empty;

    [ObservableProperty]
    private DateTime startDate = DateTime.Today;

    [ObservableProperty]
    private DateTime endDate = DateTime.Today.AddDays(1);

    /// <summary>Gets the daily rate parsed from the Shell query parameter.</summary>
    public decimal DailyRate =>
    decimal.TryParse(DailyRateText, NumberStyles.Number, CultureInfo.InvariantCulture, out var rate)
        ? rate
        : 0;

    // AI-assisted: estimate values are calculated client-side from DateTime picker values.
    // Reviewed and modified to adapt to unit tests
    /// <summary>Gets the number of rental days used in the client-side estimate.</summary>
    public int RentalDays =>
    RentalPriceCalculator.CalculateRentalDays(StartDate, EndDate);

    /// <summary>Gets the estimated total price displayed before submission.</summary>
    public decimal EstimatedTotal =>
    RentalPriceCalculator.CalculateEstimatedTotal(DailyRate, StartDate, EndDate);

    /// <summary>
    /// Creates the rental request ViewModel with the rental workflow service.
    /// </summary>
    public CreateRentalRequestViewModel(IRentalService rentalService)
    {
        _rentalService = rentalService;
        Title = "Request Rental";
    }

    // AI-assisted: keep estimate bindings refreshed when the incoming rate or dates change.
    partial void OnDailyRateTextChanged(string value)
    {
        OnPropertyChanged(nameof(DailyRate));
        OnPropertyChanged(nameof(EstimatedTotal));
    }

    partial void OnStartDateChanged(DateTime value)
    {
        OnPropertyChanged(nameof(RentalDays));
        OnPropertyChanged(nameof(EstimatedTotal));
    }

    partial void OnEndDateChanged(DateTime value)
    {
        OnPropertyChanged(nameof(RentalDays));
        OnPropertyChanged(nameof(EstimatedTotal));
    }

    [RelayCommand]
    private async Task SubmitRequestAsync()
    {
        if (IsBusy)
            return;

        // Date validation is performed before the request is sent to the hosted API.
        if (!ValidateForm())
            return;

        try
        {
            IsBusy = true;
            ClearError();

            var request = new CreateRentalRequest
            {
                ItemId = ItemId,
                StartDate = StartDate.ToString("yyyy-MM-dd"),
                EndDate = EndDate.ToString("yyyy-MM-dd")
            };

            await _rentalService.SubmitRentalRequestAsync(request);

            await Application.Current!.Windows[0].Page!.DisplayAlertAsync(
                "Success",
                "Rental request submitted successfully.",
                "OK");

            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            SetError(ex.Message);
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

    private bool ValidateForm()
    {
        if (ItemId <= 0)
        {
            SetError("Item could not be identified.");
            return false;
        }

        if (StartDate.Date < DateTime.Today)
        {
            SetError("Start date cannot be in the past.");
            return false;
        }

        if (EndDate.Date <= StartDate.Date)
        {
            SetError("End date must be after the start date.");
            return false;
        }

        return true;
    }
}
