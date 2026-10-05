using System.ComponentModel.DataAnnotations;

namespace Reklaim_frontend.Models;

public class ClaimRequestDto
{
    public int ClaimantUserId { get; set; }

    // brief description of proof or ownership
    [Required(ErrorMessage = "Please provide a proof description.")]
    [StringLength(500, MinimumLength = 10, ErrorMessage = "Proof description must be at least 10 characters.")]
    public string? ProofDescription { get; set; }

    // optional contact info
    [StringLength(200, ErrorMessage = "Contact info is too long.")]
    public string? Contact { get; set; }
}
