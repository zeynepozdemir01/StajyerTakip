using MediatR;
using Microsoft.AspNetCore.Mvc;

using StajyerTakip.Application.Interns.Queries;
using StajyerTakip.Application.Interns.Commands;
using StajyerTakip.Api.Contracts.Interns;
using StajyerTakip.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using StajyerTakip.Application.Interns.Queries.GetInterns;

namespace StajyerTakip.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class InternsController : ControllerBase
{
    private readonly IMediator _mediator;
    public InternsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<PaginatedInternListDto>> List(
        [FromQuery] string? q,
        [FromQuery] string? status,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string sortField = "LastName",
        [FromQuery] string sortOrder = "asc")
    {
        var res = await _mediator.Send(new GetInternsQuery
        {
            Search   = q,
            Status   = status,
            Page     = page,
            PageSize = pageSize,
            SortBy   = sortField,
            SortDir  = sortOrder
        });

        if (!res.Succeeded || res.Value is null)
            return Problem(res.Error, statusCode: 400);

        var items = res.Value.Items.Select(i => new InternDto(
            Id:             i.Id,
            FirstName:      i.FirstName,
            LastName:       i.LastName,
            IdentityNumber: i.IdentityNumber,
            Email:          i.Email,
            Phone:          i.Phone ?? "",
            School:         i.School ?? "",
            Department:     i.Department ?? "",
            StartDate:      i.StartDate,
            EndDate:        i.EndDate,
            Status:         i.Status
        )).ToList();

        var dto = new PaginatedInternListDto(
            Items: items,
            Page: res.Value.Page,
            PageSize: res.Value.PageSize,
            TotalCount: res.Value.Total
        );

        return Ok(dto);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<InternDto>> GetById(int id)
    {
        var res = await _mediator.Send(new GetInternByIdQuery(id));
        if (!res.Succeeded || res.Value is null) return NotFound(res.Error);

        var e = res.Value;
        var dto = new InternDto(
            Id:             e.Id,
            FirstName:      e.FirstName,
            LastName:       e.LastName,
            IdentityNumber: e.NationalId,
            Email:          e.Email,
            Phone:          e.Phone ?? "",
            School:         e.School ?? "",
            Department:     e.Department ?? "",
            StartDate:      e.StartDate.ToDateTime(TimeOnly.MinValue),
            EndDate:        e.EndDate.HasValue ? e.EndDate.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null,
            Status:         e.Status
        );

        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<InternDto>> Create([FromBody] CreateInternRequest req)
    {
        var entity = new Intern
        {
            FirstName  = req.FirstName,
            LastName   = req.LastName,
            NationalId = req.NationalId,
            Email      = req.Email,
            Phone      = req.Phone,
            School     = req.School,
            Department = req.Department,
            Status     = string.IsNullOrWhiteSpace(req.Status) ? "Aktif" : req.Status!,
            StartDate  = req.StartDate ?? DateOnly.FromDateTime(DateTime.UtcNow),
            EndDate    = req.EndDate
        };

        var createRes = await _mediator.Send(new CreateInternCommand(entity));
        if (!createRes.Succeeded) return Problem(createRes.Error, statusCode: 400);

        var dto = new InternDto(
            Id:             createRes.Value,
            FirstName:      entity.FirstName,
            LastName:       entity.LastName,
            IdentityNumber: entity.NationalId,
            Email:          entity.Email,
            Phone:          entity.Phone ?? "",
            School:         entity.School ?? "",
            Department:     entity.Department ?? "",
            StartDate:      entity.StartDate.ToDateTime(TimeOnly.MinValue),
            EndDate:        entity.EndDate.HasValue ? entity.EndDate.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null,
            Status:         entity.Status
        );

        return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateInternRequest req)
    {
        var entity = new Intern
        {
            Id         = id,
            FirstName  = req.FirstName,
            LastName   = req.LastName,
            NationalId = req.NationalId,
            Email      = req.Email,
            Phone      = req.Phone,
            School     = req.School,
            Department = req.Department,
            Status     = string.IsNullOrWhiteSpace(req.Status) ? "Aktif" : req.Status!,
            StartDate  = req.StartDate,
            EndDate    = req.EndDate
        };

        var updRes = await _mediator.Send(new UpdateInternCommand(entity));
        if (!updRes.Succeeded) return Problem(updRes.Error, statusCode: 400);

        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var delRes = await _mediator.Send(new DeleteInternCommand(id));
        if (!delRes.Succeeded) return Problem(delRes.Error, statusCode: 400);
        return NoContent();
    }
}
