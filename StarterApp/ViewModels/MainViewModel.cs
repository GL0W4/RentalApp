/// @file MainViewModel.cs
/// @brief Main dashboard view model for authenticated users
/// @author StarterApp Development Team
/// @date 2025

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Services;

namespace StarterApp.ViewModels;

/// @brief View model for the main dashboard page
/// @details Manages the main dashboard display and navigation to other sections
/// @extends BaseViewModel
public partial class MainViewModel : BaseViewModel
{
    /// @brief Authentication service for managing user authentication
    private readonly IAuthenticationService _authService;
    
    /// @brief Navigation service for managing page navigation
    private readonly INavigationService _navigationService;

    /// @brief Welcome message displayed to the user
    /// @details Observable property showing a personalized welcome message
    [ObservableProperty]
    private string welcomeMessage = string.Empty;

    /// @brief Default constructor for design-time support
    /// @details Sets the title to "Dashboard"
    public MainViewModel()
    {
        // Default constructor for design time support
        Title = "Dashboard";
        WelcomeMessage = "Welcome!";
    }
    
    /// @brief Initializes a new instance of the MainViewModel class
    /// @param authService The authentication service instance
    /// @param navigationService The navigation service instance
    /// @details Sets up the required services, initializes the title, and loads dashboard data
    public MainViewModel(IAuthenticationService authService, INavigationService navigationService)
    {
        _authService = authService;
        _navigationService = navigationService;
        Title = "Dashboard";

        LoadUserData();
    }

    /// @brief Loads dashboard data
    /// @details Sets a generic welcome message for the authenticated user
    private void LoadUserData()
    {
        WelcomeMessage = "Welcome!";
    }

    /// @brief Logs out the current user
    /// @details Relay command that confirms logout and performs the logout operation
    /// @return A task representing the asynchronous logout operation
    [RelayCommand]
    private async Task LogoutAsync()
    {
        var result = await Application.Current!.Windows[0].Page!.DisplayAlertAsync(
            "Logout", 
            "Are you sure you want to logout?", 
            "Yes", 
            "No");

        if (result)
        {
            await _authService.LogoutAsync();
            await _navigationService.NavigateToAsync("LoginPage");
        }
    }

    /// @brief Navigates to the user profile page
    /// @details Relay command that navigates to the profile management page
    /// @return A task representing the asynchronous navigation operation
    [RelayCommand]
    private async Task NavigateToProfileAsync()
    {
        await _navigationService.NavigateToAsync("TempPage");
    }

    /// @brief Navigates to the settings page
    /// @details Relay command that navigates to the application settings page
    /// @return A task representing the asynchronous navigation operation
    [RelayCommand]
    private async Task NavigateToSettingsAsync()
    {
        await _navigationService.NavigateToAsync("TempPage");
    }

    /// @brief Navigates to the items list page
    /// @details Relay command that navigates to the items browsing page
    /// @return A task representing the asynchronous navigation operation
    [RelayCommand]
    private async Task NavigateToItemsListAsync()
    {
        await _navigationService.NavigateToAsync("ItemsListPage");
    }

    /// @brief Refreshes the dashboard data
    /// @details Relay command that reloads dashboard data and simulates a refresh operation
    /// @return A task representing the asynchronous refresh operation
    [RelayCommand]
    private async Task RefreshDataAsync()
    {
        try
        {
            IsBusy = true;
            LoadUserData();
            
            // Simulate refresh delay
            await Task.Delay(1000);
        }
        catch (Exception ex)
        {
            SetError($"Failed to refresh data: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }
}