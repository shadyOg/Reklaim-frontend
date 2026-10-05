using System.ComponentModel.DataAnnotations;

namespace Reklaim_frontend.Models;

public class CreateClaimRequest
{
    public int PostId { get; set; }

    [Required(ErrorMessage = "Please describe a detail that proves this item belongs to you.")]
    [StringLength(1000, MinimumLength = 20, ErrorMessage = "Proof must be between 20 and 1000 characters.")]
    public string ProofDescription { get; set; } = string.Empty;
}
