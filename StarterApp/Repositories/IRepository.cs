namespace StarterApp.Repositories;

/// <summary>
/// Minimal read-only repository contract used for API-backed list retrieval.
/// </summary>
public interface IRepository<T>
{
    /// <summary>
    /// Retrieves all available records of the given type from the backing data source.
    /// </summary>
    Task<List<T>> GetAllAsync();
}
