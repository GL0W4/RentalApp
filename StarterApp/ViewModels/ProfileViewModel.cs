/// @file ProfileViewModel.cs
/// @brief User profile management view model
/// @author StarterApp Development Team
/// @date 2025

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Services;

namespace StarterApp.ViewModels;

/// @brief View model for the user profile page
/// @details Manages profile display and temporary placeholder account functionality
/// @extends BaseViewModel
public partial class ProfileViewModel : BaseViewModel
{
    /// @brief Authentication service for managing user authentication
    private readonly IAuthenticationService _authService;
    
    /// @brief Navigation service for managing page navigation
    private readonly INavigationService _navigationService;

    /// @brief Display name for the current user
    /// @details Observable property containing a placeholder user label
    [ObservableProperty]
    private string displayName = "Authenticated User";

    /// @brief The user's current password for verification
    /// @details Observable property bound to the current password input field
    [ObservableProperty]
    private string currentPassword = string.Empty;

    /// @brief The user's new password
    /// @details Observable property bound to the new password input field
    [ObservableProperty]
    private string newPassword = string.Empty;

    /// @brief Confirmation of the user's new password
    /// @details Observable property bound to the confirm new password input field
    [ObservableProperty]
    private string confirmNewPassword = string.Empty;

    /// @brief Indicates whether the password change mode is active
    /// @details Observable property that controls the visibility of password change fields
    [ObservableProperty]
    private bool isChangingPassword;

    /// @brief Initializes a new instance of the ProfileViewModel class
    /// @param authService The authentication service instance
    /// @param navigationService The navigation service instance
    /// @details Sets up the required services, initializes the title, and loads profile data
    public ProfileViewModel(IAuthenticationService authService, INavigationService navigationService)
    {
        _authService = authService;
        _navigationService = navigationService;
        Title = "Profile";

        LoadUserData();
    }

    /// @brief Loads the current profile data
    /// @details Uses a temporary placeholder profile while API-backed profile retrieval is not yet implemented
    private void LoadUserData()
    {
        DisplayName = _authService.IsAuthenticated ? "Authenticated User" : "Guest";
    }

    /// @brief Displays password change placeholder information
    /// @details Relay command that informs the user password change is not yet available in the API-backed flow
    /// @return A task representing the asynchronous operation
    [RelayCommand]
    private async Task ChangePasswordAsync()
    {
        await Application.Current!.Windows[0].Page!.DisplayAlertAsync(
            "Not Available",
            "Password change is not implemented in the current API-backed authentication flow.",
            "OK");

        ClearPasswordFields();
        IsChangingPassword = false;
    }

    /// @brief Toggles the password change mode
    /// @details Relay command that shows/hides password change fields and clears data when hiding
    [RelayCommand]
    private void TogglePasswordChangeMode()
    {
        IsChangingPassword = !IsChangingPassword;
        if (!IsChangingPassword)
        {
            ClearPasswordFields();
            ClearError();
        }
    }

    /// @brief Navigates back to the previous page
    /// @details Relay command that performs backward navigation
    /// @return A task representing the asynchronous navigation operation
    [RelayCommand]
    private async Task NavigateBackAsync()
    {
        await _navigationService.NavigateBackAsync();
    }

    /// @brief Clears all password input fields
    /// @details Resets all password-related properties to empty strings
    private void ClearPasswordFields()
    {
        CurrentPassword = string.Empty;
        NewPassword = string.Empty;
        ConfirmNewPassword = string.Empty;
    }
}