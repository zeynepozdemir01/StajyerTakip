using MediatR;
using Microsoft.AspNetCore.Mvc;


using StajyerTakip.Application.Interns.Queries;
using StajyerTakip.Application.Interns.Commands;
using StajyerTakip.Api.Contracts.Interns;
using StajyerTakip.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

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
        var res = await _mediator.Send(new GetInternsQuery(q, status, page, pageSize, sortField, sortOrder));
        if (!res.Succeeded || res.Value is null) return Problem(res.Error, statusCode: 400);

        var items = res.Value.Items
            .Select(i => new InternDto(i.Id, i.FirstName, i.LastName, i.Email, i.Phone))
            .ToList();

        var dto = new PaginatedInternListDto(
            Items: items,
            Page: res.Value.Page,
            PageSize: res.Value.PageSize,
            TotalCount: res.Value.TotalCount
        );

        return Ok(dto);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<InternDto>> GetById(int id)
    {
        var res = await _mediator.Send(new GetInternByIdQuery(id));
        if (!res.Succeeded || res.Value is null) return NotFound(res.Error);

        var e = res.Value;
        return Ok(new InternDto(e.Id, e.FirstName, e.LastName, e.Email, e.Phone));
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

        var dto = new InternDto(createRes.Value, entity.FirstName, entity.LastName, entity.Email, entity.Phone);
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
