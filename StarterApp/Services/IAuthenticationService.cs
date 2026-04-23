namespace StarterApp.Services;

public interface IAuthenticationService
{
    event EventHandler<bool>? AuthenticationStateChanged;

    bool IsAuthenticated { get; }
    string? JwtToken { get; }
    DateTime? TokenExpiresAt { get; }
    int? CurrentUserId { get; }

    Task<AuthenticationResult> LoginAsync(string email, string password);
    Task<AuthenticationResult> RegisterAsync(string firstName, string lastName, string email, string password);
    Task LogoutAsync();

    Task<string?> GetValidTokenAsync();
}