/// @file LoginViewModel.cs
/// @brief Login page view model for user authentication
/// @author StarterApp Development Team
/// @date 2025

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Services;

namespace StarterApp.ViewModels;

/// @brief View model for the login page
/// @details Handles user authentication, validation, loading state, error reporting, and navigation
/// @extends BaseViewModel
public partial class LoginViewModel : BaseViewModel
{
    /// @brief Authentication service for login operations
    private readonly IAuthenticationService _authService;

    /// @brief Navigation service for page navigation
    private readonly INavigationService _navigationService;

    /// @brief User email input
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
    private string email = string.Empty;

    /// @brief User password input
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
    private string password = string.Empty;

    /// @brief Remember-me option
    [ObservableProperty]
    private bool rememberMe;

    /// @brief Password visibility toggle
    [ObservableProperty]
    private bool isPasswordVisible;

    /// @brief Default constructor for design-time support
    public LoginViewModel()
    {
        Title = "Login";
    }

    /// @brief Initializes a new instance of the LoginViewModel class
    /// @param authService The authentication service instance
    /// @param navigationService The navigation service instance
    public LoginViewModel(IAuthenticationService authService, INavigationService navigationService)
    {
        _authService = authService;
        _navigationService = navigationService;
        Title = "Login";
    }

    /// @brief Determines whether login can execute
    /// @returns True if the form is valid and not busy
    private bool CanLogin()
    {
        return !IsBusy
            && !string.IsNullOrWhiteSpace(Email)
            && !string.IsNullOrWhiteSpace(Password);
    }

    /// @brief Attempts to authenticate the user
    [RelayCommand(CanExecute = nameof(CanLogin))]
    private async Task LoginAsync()
    {
        if (IsBusy)
            return;

        ClearError();

        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            SetError("Please enter both email and password.");
            return;
        }

        try
        {
            IsBusy = true;
            LoginCommand.NotifyCanExecuteChanged();

            var result = await _authService.LoginAsync(Email, Password);

            if (result.IsSuccess)
            {
                await _navigationService.NavigateToAsync("MainPage");
            }
            else
            {
                SetError(result.Message ?? "Login failed.");
            }
        }
        catch (Exception ex)
        {
            SetError($"Login failed: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
            LoginCommand.NotifyCanExecuteChanged();
        }
    }

    /// @brief Toggles password visibility
    [RelayCommand]
    private void TogglePasswordVisibility()
    {
        IsPasswordVisible = !IsPasswordVisible;
    }

    /// @brief Navigates to the registration page
    [RelayCommand]
    private async Task NavigateToRegisterAsync()
    {
        await _navigationService.NavigateToAsync("RegisterPage");
    }

    /// @brief Displays forgot-password placeholder
    [RelayCommand]
    private async Task ForgotPasswordAsync()
    {
        await Application.Current.MainPage.DisplayAlert(
            "Forgot Password",
            "Password reset functionality will be implemented in a future version.",
            "OK");
    }
}