using MediatR;
using StajyerTakip.Application.Common;
using StajyerTakip.Domain.Entities;

namespace StajyerTakip.Application.Interns.Commands;

public sealed record CreateInternCommand(Intern Model) : IRequest<Result<int>>;
