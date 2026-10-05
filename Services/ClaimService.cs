using System.Net.Http.Headers;
using System.Net.Http.Json;
using Reklaim_frontend.Models;

namespace Reklaim_frontend.Services;

public class ClaimService : IClaimService
{
    private readonly HttpClient http;
    private readonly AuthService auth;

    public ClaimService(HttpClient http, AuthService auth)
    {
        this.http = http;
        this.auth = auth;
    }

    public async Task<ClaimRequestDto?> SubmitAsync(CreateClaimRequest request)
    {
        await AddAuthHeaderAsync();
        var response = await http.PostAsJsonAsync("api/claims", request);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var result = await response.Content.ReadFromJsonAsync<ClaimSubmissionResponse>();
        return result is null ? null : new ClaimRequestDto
        {
            Id = result.Id,
            PostId = request.PostId,
            ClaimerUserId = 0,
            ProofDescription = request.ProofDescription,
            Status = "Pending",
            DateSubmitted = DateTime.UtcNow
        };
    }

    public async Task<IReadOnlyList<ClaimRequestDto>> GetPendingForReviewAsync()
    {
        await AddAuthHeaderAsync();
        return await http.GetFromJsonAsync<List<ClaimRequestDto>>("api/claims/on-my-posts") ?? [];
    }

    public async Task<IReadOnlyList<ClaimRequestDto>> GetMineAsync()
    {
        await AddAuthHeaderAsync();
        return await http.GetFromJsonAsync<List<ClaimRequestDto>>("api/claims/my-claims") ?? [];
    }

    public async Task<bool> ReviewAsync(int claimId, string status)
    {
        await AddAuthHeaderAsync();
        var response = await http.PostAsJsonAsync($"api/claims/{claimId}/review", new
        {
            approve = status.Equals("Approved", StringComparison.OrdinalIgnoreCase)
        });
        return response.IsSuccessStatusCode;
    }

    private async Task AddAuthHeaderAsync()
    {
        var token = await auth.GetTokenAsync();
        http.DefaultRequestHeaders.Authorization = string.IsNullOrWhiteSpace(token)
            ? null
            : new AuthenticationHeaderValue("Bearer", token);
    }
}
