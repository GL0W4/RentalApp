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
        catch (Exception ex)
        {
            SetError($"Failed to load rental requests: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }
}