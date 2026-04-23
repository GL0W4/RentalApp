namespace StarterApp.Services;

public class AuthenticationResult
{
    public bool IsSuccess { get; }
    public string Message { get; }
    public string? Token { get; }
    public DateTime? ExpiresAt { get; }
    public int? UserId { get; }

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