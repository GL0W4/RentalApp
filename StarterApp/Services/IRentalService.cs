namespace StarterApp.Services;

/// <summary>
/// Coordinates rental request workflows and authentication checks.
/// </summary>
public interface IRentalService
{
    /// <summary>Submits a rental request for the authenticated borrower.</summary>
    Task SubmitRentalRequestAsync(CreateRentalRequest request);
    /// <summary>Gets rental requests received by the authenticated user's items.</summary>
    Task<List<RentalRequestItem>> GetIncomingRentalsAsync(string? status = null);
    /// <summary>Gets rental requests submitted by the authenticated user.</summary>
    Task<List<RentalRequestItem>> GetOutgoingRentalsAsync(string? status = null);
    /// <summary>Updates a rental request status according to the workflow rules enforced by the API.</summary>
    Task UpdateRentalStatusAsync(int rentalId, string status);

}
