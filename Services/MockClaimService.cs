using Reklaim_frontend.Models;

namespace Reklaim_frontend.Services;

public class MockClaimService : IClaimService
{
    private readonly IPostService posts;
    private readonly List<ClaimRequestDto> claims = new();
    private readonly object sync = new();
    private int nextId = 1;

    public MockClaimService(IPostService posts)
    {
        this.posts = posts;
    }

    public async Task<ClaimRequestDto?> SubmitAsync(CreateClaimRequest request)
    {
        var post = await posts.GetByIdAsync(request.PostId);
        if (post is null || !string.Equals(post.Status, "Open", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        lock (sync)
        {
            if (claims.Any(claim => claim.PostId == request.PostId && claim.Status.Equals("Pending", StringComparison.OrdinalIgnoreCase)))
            {
                return null;
            }

            var claim = new ClaimRequestDto
            {
                Id = nextId++,
                PostId = post.Id,
                ClaimerUserId = 1,
                ProofDescription = request.ProofDescription.Trim(),
                Status = "Pending",
                PostTitle = post.Title,
                ClaimerName = "Current student",
                DateSubmitted = DateTime.UtcNow
            };
            claims.Add(claim);
            return Clone(claim);
        }
    }

    public Task<IReadOnlyList<ClaimRequestDto>> GetPendingForReviewAsync()
    {
        lock (sync)
        {
            return Task.FromResult<IReadOnlyList<ClaimRequestDto>>(
                claims.Where(claim => claim.Status.Equals("Pending", StringComparison.OrdinalIgnoreCase))
                    .Select(Clone)
                    .OrderByDescending(claim => claim.DateSubmitted)
                    .ToList());
        }
    }

    public Task<IReadOnlyList<ClaimRequestDto>> GetMineAsync()
    {
        lock (sync)
        {
            return Task.FromResult<IReadOnlyList<ClaimRequestDto>>(
                claims.Where(claim => claim.ClaimerUserId == 1).Select(Clone).ToList());
        }
    }

    public async Task<bool> ReviewAsync(int claimId, string status)
    {
        if (!status.Equals("Approved", StringComparison.OrdinalIgnoreCase)
            && !status.Equals("Denied", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        ClaimRequestDto? claim;
        lock (sync)
        {
            claim = claims.FirstOrDefault(item => item.Id == claimId);
            if (claim is null || !claim.Status.Equals("Pending", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            claim.Status = status;
            if (status.Equals("Approved", StringComparison.OrdinalIgnoreCase))
            {
                claim.ClaimerPhoneNumber = "020 000 0000";
                claim.FinderPhoneNumber = "024 000 0000";
            }
        }

        if (status.Equals("Approved", StringComparison.OrdinalIgnoreCase))
        {
            await posts.UpdateStatusAsync(claim.PostId, "Claimed");
        }

        return true;
    }

    private static ClaimRequestDto Clone(ClaimRequestDto source) => new()
    {
        Id = source.Id,
        PostId = source.PostId,
        ClaimerUserId = source.ClaimerUserId,
        ProofDescription = source.ProofDescription,
        Status = source.Status,
        PostTitle = source.PostTitle,
        ClaimerName = source.ClaimerName,
        ClaimerPhoneNumber = source.ClaimerPhoneNumber,
        FinderPhoneNumber = source.FinderPhoneNumber,
        DateSubmitted = source.DateSubmitted
    };
}
