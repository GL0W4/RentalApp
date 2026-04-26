using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Services;
using System.Collections.ObjectModel;

namespace StarterApp.ViewModels;

public partial class RentalRequestsViewModel : BaseViewModel
{
    private readonly IRentalService _rentalService;

    [ObservableProperty]
    private ObservableCollection<RentalRequestItem> incomingRentals = new();

    [ObservableProperty]
    private ObservableCollection<RentalRequestItem> outgoingRentals = new();

    public RentalRequestsViewModel(IRentalService rentalService)
    {
        _rentalService = rentalService;
        Title = "Rental Requests";
    }

    [RelayCommand]
    private async Task LoadRentalsAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            ClearError();

            await LoadRentalListsAsync();
        }
        catch (Exception ex)
        {
            SetError($"Failed to load rental requests: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }
    private async Task LoadRentalListsAsync()
    {

            var incoming = await _rentalService.GetIncomingRentalsAsync();
            var outgoing = await _rentalService.GetOutgoingRentalsAsync();

            IncomingRentals.Clear();
            foreach (var rental in incoming)
            {
                IncomingRentals.Add(rental);
            }

            OutgoingRentals.Clear();
            foreach (var rental in outgoing)
            {
                OutgoingRentals.Add(rental);
            }
    }

    [RelayCommand]
    private async Task ApproveRentalAsync(RentalRequestItem rental)
    {
        if (rental == null)
            return;

        await UpdateRentalStatusAsync(rental, "Approved");
    }

    private async Task UpdateRentalStatusAsync(RentalRequestItem rental, string status)
    {
        if (IsBusy)
            return;

        var confirm = await Application.Current!.Windows[0].Page!.DisplayAlertAsync(
            $"{status} Request",
            $"{status} rental request for '{rental.ItemTitle}'?",
            status,
            "Cancel");

        if (!confirm)
            return;

        try
        {
            IsBusy = true;
            ClearError();

            await _rentalService.UpdateRentalStatusAsync(rental.Id, status);

            await Application.Current!.Windows[0].Page!.DisplayAlertAsync(
                "Success",
                $"Rental request {status.ToLower()} successfully.",
                "OK");

            await LoadRentalListsAsync();
        }
        catch (Exception ex)
        {
            SetError($"Failed to update rental status: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }

        
    }
}