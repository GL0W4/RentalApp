namespace StarterApp.Services;

/// <summary>
/// Represents the outcome of a login or registration operation.
/// </summary>
public class AuthenticationResult
{
    /// <summary>Gets whether the authentication operation succeeded.</summary>
    public bool IsSuccess { get; }
    /// <summary>Gets a user-facing message describing the result.</summary>
    public string Message { get; }
    /// <summary>Gets the JWT token returned by the API when login succeeds.</summary>
    public string? Token { get; }
    /// <summary>Gets the token expiry time returned by the API.</summary>
    public DateTime? ExpiresAt { get; }
    /// <summary>Gets the authenticated user identifier returned by the API.</summary>
    public int? UserId { get; }

    /// <summary>
    /// Creates an immutable authentication result for ViewModels to interpret.
    /// </summary>
    public AuthenticationResult(
        bool isSuccess,
        string message,
        string? token = null,
        DateTime? expiresAt = null,
        int? userId = null)
    {
        IsSuccess = isSuccess;
        Message = message;
        Token = token;
        ExpiresAt = expiresAt;
        UserId = userId;
    }
}
