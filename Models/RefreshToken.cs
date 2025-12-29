using HW.Models;
using System.ComponentModel.DataAnnotations;

public class RefreshToken
{
    public int Id { get; set; }

    [Required]
    public string Token { get; set; } = string.Empty;

    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public DateTime ExpiresUtc { get; set; }
    public DateTime? RevokedUtc { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser? User { get; set; }

    public bool IsExpired => DateTime.UtcNow >= ExpiresUtc;
    public bool IsActive => RevokedUtc == null && !IsExpired;
}
