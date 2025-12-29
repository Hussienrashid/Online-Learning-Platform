using System.ComponentModel.DataAnnotations;

public record Register(
    [Required, EmailAddress] string Email,
    [Required, MinLength(6)] string Password
);

public record Login(
    [Required, EmailAddress] string Email,
    [Required] string Password
);

public record Refresh(
    [Required] string RefreshToken
);

public record AuthResponse(
    string AccessToken,
    DateTime AccessTokenExpiresUtc,
    string RefreshToken,
    DateTime RefreshTokenExpiresUtc
);