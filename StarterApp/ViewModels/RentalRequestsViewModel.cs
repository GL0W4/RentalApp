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
    
    [ObservableProperty]
    private ObservableCollection<string> statusFilters = new()
    {
        "All",
        "Requested",
        "Approved",
        "Rejected",
        "Out for Rent",
        "Overdue",
        "Returned",
        "Completed"
    };

    [ObservableProperty]
    private string selectedStatusFilter = "All";

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

        var status = SelectedStatusFilter == "All" ? null : SelectedStatusFilter;

        var incoming = await _rentalService.GetIncomingRentalsAsync(status);
        var outgoing = await _rentalService.GetOutgoingRentalsAsync(status);

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

    [RelayCommand]
    private async Task RejectRentalAsync(RentalRequestItem rental)
    {
        if (rental == null)
            return;

        await UpdateRentalStatusAsync(rental, "Rejected");
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

    [RelayCommand]
    private async Task MarkOutForRentAsync(RentalRequestItem rental)
    {
        if (rental == null)
            return;

        await UpdateRentalStatusAsync(rental, "Out for Rent");
    }

    [RelayCommand]
    private async Task MarkReturnedAsync(RentalRequestItem rental)
    {
        if (rental == null)
            return;

        await UpdateRentalStatusAsync(rental, "Returned");
    }

    [RelayCommand]
    private async Task CompleteRentalAsync(RentalRequestItem rental)
    {
        if (rental == null)
            return;

        await UpdateRentalStatusAsync(rental, "Completed");
    }

    partial void OnSelectedStatusFilterChanged(string value)
    {
        if (!IsBusy)
        {
            LoadRentalsCommand.Execute(null);
        }
    }
}