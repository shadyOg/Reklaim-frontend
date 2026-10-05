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
        return response.IsSuccessStatusCode
            ? await response.Content.ReadFromJsonAsync<ClaimRequestDto>()
            : null;
    }

    public async Task<IReadOnlyList<ClaimRequestDto>> GetPendingForReviewAsync()
    {
        await AddAuthHeaderAsync();
        return await http.GetFromJsonAsync<List<ClaimRequestDto>>("api/claims/review") ?? [];
    }

    public async Task<IReadOnlyList<ClaimRequestDto>> GetMineAsync()
    {
        await AddAuthHeaderAsync();
        return await http.GetFromJsonAsync<List<ClaimRequestDto>>("api/claims/mine") ?? [];
    }

    public async Task<bool> ReviewAsync(int claimId, string status)
    {
        await AddAuthHeaderAsync();
        var response = await http.PatchAsync($"api/claims/{claimId}/status", JsonContent.Create(new { Status = status }));
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
