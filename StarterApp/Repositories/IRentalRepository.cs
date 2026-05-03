using StarterApp.Services;

namespace StarterApp.Repositories;

/// <summary>
/// Defines API operations for rental request submission, listing, and status updates.
/// </summary>
public interface IRentalRepository
{
    /// <summary>Submits a rental request for an item using the authenticated user's JWT.</summary>
    Task SubmitRentalRequestAsync(CreateRentalRequest request, string jwtToken);

    /// <summary>Gets requests received by the current user's items, optionally filtered by status.</summary>
    Task<List<RentalRequestItem>> GetIncomingRentalsAsync(string jwtToken, string? status = null);

    /// <summary>Gets requests made by the current user, optionally filtered by status.</summary>
    Task<List<RentalRequestItem>> GetOutgoingRentalsAsync(string jwtToken, string? status = null);

    /// <summary>Updates a rental request to the next workflow status.</summary>
    Task UpdateRentalStatusAsync(int rentalId, string status, string jwtToken);
}
