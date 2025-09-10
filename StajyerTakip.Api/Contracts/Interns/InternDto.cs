namespace StajyerTakip.Api.Contracts.Interns;

public record InternDto(
    int Id,
    string FirstName,
    string LastName,
    string Email,
    string? Phone
);
