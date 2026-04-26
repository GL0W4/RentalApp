using StarterApp.Services;

namespace StarterApp.Repositories;

public interface IRentalRepository
{
    Task SubmitRentalRequestAsync(CreateRentalRequest request, string jwtToken);
    Task<List<RentalRequestItem>> GetIncomingRentalsAsync(string jwtToken, string? status = null);
    Task<List<RentalRequestItem>> GetOutgoingRentalsAsync(string jwtToken, string? status = null);
    Task UpdateRentalStatusAsync(int rentalId, string status, string jwtToken);
}