using System.Net.Http.Json;
using Microsoft.Maui.Storage;

namespace StarterApp.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly HttpClient _httpClient;

    public event EventHandler<bool>? AuthenticationStateChanged;

    public string? JwtToken { get; private set; }
    public DateTime? TokenExpiresAt { get; private set; }
    public int? CurrentUserId { get; private set; }

    public bool IsAuthenticated =>
        !string.IsNullOrWhiteSpace(JwtToken) &&
        TokenExpiresAt.HasValue &&
        TokenExpiresAt > DateTime.UtcNow;

    public AuthenticationService()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://set09102-api.b-davison.workers.dev")
        };
    }

    public async Task<AuthenticationResult> LoginAsync(string email, string password)
    {
        try
        {
            var request = new
            {
                email,
                password
            };

            var response = await _httpClient.PostAsJsonAsync("/auth/token", request);

            if (!response.IsSuccessStatusCode)
            {
                var message = await ExtractErrorMessageAsync(response, "Invalid email or password.", true);
    return new AuthenticationResult(false, message);
            }

            var result = await response.Content.ReadFromJsonAsync<AuthTokenResponse>();

            if (result == null || string.IsNullOrWhiteSpace(result.Token))
            {
                return new AuthenticationResult(false, "Login failed: invalid token response.");
            }

            JwtToken = result.Token;
            TokenExpiresAt = result.ExpiresAt;
            CurrentUserId = result.UserId;

            await SecureStorage.SetAsync("jwt_token", JwtToken);
            await SecureStorage.SetAsync("jwt_expires_at", TokenExpiresAt.Value.ToString("O"));
            await SecureStorage.SetAsync("jwt_user_id", CurrentUserId?.ToString() ?? string.Empty);

            AuthenticationStateChanged?.Invoke(this, true);

            return new AuthenticationResult(
                true,
                "Login successful",
                JwtToken,
                TokenExpiresAt,
                CurrentUserId);
        }
        catch (Exception ex)
        {
            return new AuthenticationResult(false, $"Login failed: {ex.Message}");
        }
    }

    public async Task<AuthenticationResult> RegisterAsync(string firstName, string lastName, string email, string password)
    {
        try
        {
            var request = new
            {
                firstName,
                lastName,
                email,
                password
            };

            var response = await _httpClient.PostAsJsonAsync("/auth/register", request);

            if (!response.IsSuccessStatusCode)
            {
                var message = await ExtractErrorMessageAsync(response, "Invalid email or password.", false);
                return new AuthenticationResult(false, message);
            }

            return new AuthenticationResult(true, "Registration successful");
        }
        catch (Exception ex)
        {
            return new AuthenticationResult(false, $"Registration failed: {ex.Message}");
        }
    }

    public async Task<string?> GetValidTokenAsync()
    {
        if (IsAuthenticated)
            return JwtToken;

        var token = await SecureStorage.GetAsync("jwt_token");
        var expiresAtRaw = await SecureStorage.GetAsync("jwt_expires_at");
        var userIdRaw = await SecureStorage.GetAsync("jwt_user_id");

        if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(expiresAtRaw))
            return null;

        if (!DateTime.TryParse(expiresAtRaw, out var expiresAt))
            return null;

        if (expiresAt <= DateTime.UtcNow)
            return null;

        JwtToken = token;
        TokenExpiresAt = expiresAt;

        if (int.TryParse(userIdRaw, out var userId))
            CurrentUserId = userId;

        return JwtToken;
    }

    public async Task LogoutAsync()
    {
        JwtToken = null;
        TokenExpiresAt = null;
        CurrentUserId = null;

        SecureStorage.Remove("jwt_token");
        SecureStorage.Remove("jwt_expires_at");
        SecureStorage.Remove("jwt_user_id");

        AuthenticationStateChanged?.Invoke(this, false);
        await Task.CompletedTask;
    }

    private class AuthTokenResponse
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public int UserId { get; set; }
    }

    private async Task<string> ExtractErrorMessageAsync(
    HttpResponseMessage response,
    string fallbackMessage,
    bool isLoginFlow)
{
    try
    {
        var errorBody = await response.Content.ReadAsStringAsync();

        if (string.IsNullOrWhiteSpace(errorBody))
            return fallbackMessage;

        if (errorBody.Contains("email already exists", StringComparison.OrdinalIgnoreCase) ||
            errorBody.Contains("already exists", StringComparison.OrdinalIgnoreCase))
        {
            return "An account with this email already exists.";
        }

        if (errorBody.Contains("uppercase", StringComparison.OrdinalIgnoreCase))
        {
            return "Password must contain at least one uppercase letter.";
        }

        if (errorBody.Contains("number", StringComparison.OrdinalIgnoreCase))
        {
            return "Password must contain at least one number.";
        }

        if (errorBody.Contains("special character", StringComparison.OrdinalIgnoreCase) ||
            errorBody.Contains("special", StringComparison.OrdinalIgnoreCase))
        {
            return "Password must contain at least one special character.";
        }

        if (isLoginFlow &&
            (errorBody.Contains("invalid", StringComparison.OrdinalIgnoreCase) ||
             errorBody.Contains("unauthorized", StringComparison.OrdinalIgnoreCase)))
        {
            return "Invalid email or password.";
        }

        if (!isLoginFlow &&
            errorBody.Contains("validation", StringComparison.OrdinalIgnoreCase))
        {
            return "Please check your registration details and try again.";
        }

        return fallbackMessage;
    }
    catch
    {
        return fallbackMessage;
    }
}
}