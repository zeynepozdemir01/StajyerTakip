namespace StajyerTakip.Application.Interns.Queries.GetInterns;

public sealed record InternListItemDto(
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
