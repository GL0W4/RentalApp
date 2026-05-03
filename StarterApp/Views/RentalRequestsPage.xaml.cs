using StarterApp.ViewModels;

namespace StarterApp.Views;

public partial class RentalRequestsPage : ContentPage
{
    private readonly RentalRequestsViewModel _viewModel;

    public RentalRequestsPage(RentalRequestsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (_viewModel.LoadRentalsCommand.CanExecute(null))
        {
        _viewModel.LoadRentalsCommand.Execute(null);
        }
    }
}