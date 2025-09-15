namespace StajyerTakip.Api.Contracts.Interns;

public sealed record InternDto(
    int Id,
    string FirstName,
    string LastName,
    string IdentityNumber,
    string Email,
    string Phone,
    string School,
    string Department,
    DateTime? StartDate,
    DateTime? EndDate,
    string Status
);
