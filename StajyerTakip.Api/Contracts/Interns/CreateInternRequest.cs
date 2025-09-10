namespace StajyerTakip.Api.Contracts.Interns;

public record CreateInternRequest(
    string FirstName,
    string LastName,
    string NationalId,
    string Email,
    string? Phone,
    string? School,
    string? Department,
    DateOnly? StartDate,
    DateOnly? EndDate,
    string? Status
);
