namespace StarterApp.Services;

public interface IRentalService
{
    Task SubmitRentalRequestAsync(CreateRentalRequest request);
    Task<List<RentalRequestItem>> GetIncomingRentalsAsync(string? status = null);
    Task<List<RentalRequestItem>> GetOutgoingRentalsAsync(string? status = null);

}