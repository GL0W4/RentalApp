namespace StarterApp.Services;

/// <summary>
/// Wraps Shell navigation so ViewModels do not depend directly on route APIs.
/// </summary>
public interface INavigationService
{
    /// <summary>Navigates to a Shell route.</summary>
    Task NavigateToAsync(string route);
    /// <summary>Navigates to a Shell route with object parameters.</summary>
    Task NavigateToAsync(string route, Dictionary<string, object> parameters);
    /// <summary>Navigates one page back in the current navigation stack.</summary>
    Task NavigateBackAsync();
    /// <summary>Navigates to the application root route.</summary>
    Task NavigateToRootAsync();
    /// <summary>Pops all pages back to the root of the current navigation stack.</summary>
    Task PopToRootAsync();
}
