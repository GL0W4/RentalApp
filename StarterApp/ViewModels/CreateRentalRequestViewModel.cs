using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Services;

namespace StarterApp.ViewModels;

[QueryProperty(nameof(ItemId), "itemId")]
public partial class CreateRentalRequestViewModel : BaseViewModel
{
    private readonly IRentalService _rentalService;

    [ObservableProperty]
    private int itemId;

    [ObservableProperty]
    private DateTime startDate = DateTime.Today;

    [ObservableProperty]
    private DateTime endDate = DateTime.Today.AddDays(1);

    public CreateRentalRequestViewModel(IRentalService rentalService)
    {
        _rentalService = rentalService;
        Title = "Request Rental";
    }

    [RelayCommand]
    private async Task SubmitRequestAsync()
    {
        if (IsBusy)
            return;

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