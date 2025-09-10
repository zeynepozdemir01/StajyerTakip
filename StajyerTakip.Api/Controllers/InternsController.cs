using MediatR;
using Microsoft.AspNetCore.Mvc;

using StajyerTakip.Application.Interns.Queries;
using StajyerTakip.Application.Interns.Commands;
using StajyerTakip.Domain.Entities;
using StajyerTakip.Api.Contracts.Interns;

namespace StajyerTakip.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InternsController : ControllerBase
{
    private readonly IMediator _mediator;
    public InternsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<PaginatedInternListDto>> Get(
        [FromQuery] string? q,
        [FromQuery] string? status,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string sortField = "LastName",
        [FromQuery] string sortOrder = "asc")
    {
        var res = await _mediator.Send(new GetInternsQuery(q, status, page, pageSize, sortField, sortOrder));
        if (!res.Succeeded || res.Value is null)
            return Problem(res.Error ?? "Kayıtlar getirilemedi.", statusCode: 400);

        var dto = new PaginatedInternListDto(
            Items: res.Value.Items.Select(MapToDto).ToList(),
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
        if (!res.Succeeded) return NotFound(res.Error);
        if (res.Value is null) return NotFound();

        return Ok(MapToDto(res.Value));
    }

    
    [HttpPost]
    [ProducesResponseType(typeof(InternDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
            StartDate  = req.StartDate ?? DateOnly.FromDateTime(DateTime.UtcNow),
            EndDate    = req.EndDate,
            Status     = string.IsNullOrWhiteSpace(req.Status) ? "Aktif" : req.Status
        };

        var res = await _mediator.Send(new CreateInternCommand(entity));
        if (!res.Succeeded) return Problem(res.Error, statusCode: 400);

        var dto = new InternDto(res.Value, entity.FirstName, entity.LastName, entity.Email, entity.Phone);
        return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);   
    }


    [HttpPut("{id:int}")]
    public async Task<ActionResult<InternDto>> Update(int id, [FromBody] UpdateInternRequest req)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        if (id != req.Id) return BadRequest("Rota Id ile gövde Id uyuşmuyor.");

        var existing = await _mediator.Send(new GetInternByIdQuery(id));
        if (!existing.Succeeded) return NotFound(existing.Error);
        if (existing.Value is null) return NotFound();

        var e = existing.Value;
        e.FirstName  = req.FirstName;
        e.LastName   = req.LastName;
        e.NationalId = req.NationalId;
        e.Email      = req.Email;
        e.Phone      = req.Phone;
        e.School     = req.School;
        e.Department = req.Department;
        e.Status     = req.Status;
        e.StartDate  = req.StartDate;
        e.EndDate    = req.EndDate;

        var res = await _mediator.Send(new UpdateInternCommand(e));
        if (!res.Succeeded)
            return Problem(res.Error ?? "Kayıt güncellenemedi.", statusCode: 400);

        return Ok(MapToDto(e));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var existing = await _mediator.Send(new GetInternByIdQuery(id));
        if (!existing.Succeeded) return NotFound(existing.Error);
        if (existing.Value is null) return NotFound();

        var res = await _mediator.Send(new DeleteInternCommand(id));
        if (!res.Succeeded)
            return Problem(res.Error ?? "Kayıt silinemedi.", statusCode: 400);

        return NoContent();
    }

    private static InternDto MapToDto(Intern e) =>
        new(e.Id, e.FirstName, e.LastName, e.Email, e.Phone);
}
