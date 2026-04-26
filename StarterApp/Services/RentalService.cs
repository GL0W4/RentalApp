using StarterApp.Repositories;

namespace StarterApp.Services;

public class RentalService : IRentalService
{
    private readonly IRentalRepository _rentalRepository;
    private readonly IAuthenticationService _authService;

    public RentalService(IRentalRepository rentalRepository, IAuthenticationService authService)
    {
        _rentalRepository = rentalRepository;
        _authService = authService;
    }

    public async Task SubmitRentalRequestAsync(CreateRentalRequest request)
    {
        var jwtToken = await GetRequiredTokenAsync();
        await _rentalRepository.SubmitRentalRequestAsync(request, jwtToken);
    }

    public async Task<List<RentalRequestItem>> GetIncomingRentalsAsync(string? status = null)
    {
        var jwtToken = await GetRequiredTokenAsync();
        return await _rentalRepository.GetIncomingRentalsAsync(jwtToken, status);
    }

    public async Task<List<RentalRequestItem>> GetOutgoingRentalsAsync(string? status = null)
    {
        var jwtToken = await GetRequiredTokenAsync();
        return await _rentalRepository.GetOutgoingRentalsAsync(jwtToken, status);
    }

    private async Task<string> GetRequiredTokenAsync()
    {
        var jwtToken = await _authService.GetValidTokenAsync();

        if (string.IsNullOrWhiteSpace(jwtToken))
        {
            throw new Exception("You must be logged in to access rental requests.");
        }

        return jwtToken;
    }
}