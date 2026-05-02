namespace StarterApp.Services;

/// <summary>
/// Provides authentication state, token access, and login/registration operations.
/// </summary>
public interface IAuthenticationService
{
    /// <summary>Raised when login or logout changes the authenticated state.</summary>
    event EventHandler<bool>? AuthenticationStateChanged;

    /// <summary>Gets a value indicating whether a non-expired JWT is currently available.</summary>
    bool IsAuthenticated { get; }
    /// <summary>Gets the active JWT token when the user is authenticated.</summary>
    string? JwtToken { get; }
    /// <summary>Gets the UTC expiry time returned by the API for the JWT.</summary>
    DateTime? TokenExpiresAt { get; }
    /// <summary>Gets the authenticated user's API identifier when known.</summary>
    int? CurrentUserId { get; }

    /// <summary>Attempts to authenticate the user and persist the returned JWT details.</summary>
    Task<AuthenticationResult> LoginAsync(string email, string password);
    /// <summary>Registers a new account through the hosted API.</summary>
    Task<AuthenticationResult> RegisterAsync(string firstName, string lastName, string email, string password);
    /// <summary>Clears local authentication state and stored token details.</summary>
    Task LogoutAsync();

    /// <summary>Returns a valid token from memory or secure storage, or null if authentication has expired.</summary>
    Task<string?> GetValidTokenAsync();
}
