using Reklaim_frontend.Models;

namespace Reklaim_frontend.Services;

public interface IClaimService
{
    Task<ClaimRequestDto?> SubmitAsync(CreateClaimRequest request);
    Task<IReadOnlyList<ClaimRequestDto>> GetPendingForReviewAsync();
    Task<IReadOnlyList<ClaimRequestDto>> GetMineAsync();
    Task<bool> ReviewAsync(int claimId, string status);
}
