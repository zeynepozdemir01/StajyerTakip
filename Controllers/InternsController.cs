using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using ClosedXML.Excel;

using MediatR;
using StajyerTakip.Domain.Entities;
using StajyerTakip.Models.ViewModels;

using StajyerTakip.Application.Interns.Queries;
using StajyerTakip.Application.Interns.Commands;

namespace StajyerTakip.Controllers
{
    [Authorize]
    public class InternsController : Controller
    {
        private readonly IMediator _mediator;
        public InternsController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> Index(
            string? q,
            string? status,
            int page = 1,
            int pageSize = 10,
            string sortField = "LastName",
            string sortOrder = "asc")
        {
            var res = await _mediator.Send(new GetInternsQuery(q, status, page, pageSize, sortField, sortOrder));
            if (!res.Succeeded) return Problem(res.Error);

            var vm = new InternListVm
            {
                Items      = res.Value!.Items,
                Page       = res.Value.Page,
                PageSize   = res.Value.PageSize,
                TotalCount = res.Value.TotalCount,
                Query      = q,
                Status     = status,
                SortField  = sortField,
                SortOrder  = sortOrder
            };

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var res = await _mediator.Send(new GetInternByIdQuery(id));
            if (!res.Succeeded) return NotFound(res.Error);
            if (res.Value is null) return NotFound();
            return View(res.Value);
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Intern model)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _mediator.Send(new CreateInternCommand(model));
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", result.Error ?? "Kayıt oluşturulamadı");
                return View(model);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var res = await _mediator.Send(new GetInternByIdQuery(id));
            if (!res.Succeeded) return NotFound(res.Error);
            if (res.Value is null) return NotFound();
            return View(res.Value);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Intern model)
        {
            if (id != model.Id) return BadRequest();
            if (!ModelState.IsValid) return View(model);

            var res = await _mediator.Send(new UpdateInternCommand(model));
            if (!res.Succeeded)
            {
                ModelState.AddModelError("", res.Error ?? "Kayıt güncellenemedi");
                return View(model);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var res = await _mediator.Send(new GetInternByIdQuery(id));
            if (!res.Succeeded) return NotFound(res.Error);
            if (res.Value is null) return NotFound();
            return View(res.Value);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var res = await _mediator.Send(new DeleteInternCommand(id));
            if (!res.Succeeded) return NotFound(res.Error);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> ExportCsv(
            string? q, string? status,
            string sortField = "LastName",
            string sortOrder = "asc")
        {
            const int Huge = 1_000_000;
            var res = await _mediator.Send(new GetInternsQuery(q, status, 1, Huge, sortField, sortOrder));
            if (!res.Succeeded) return Problem(res.Error);

            static string CsvEsc(string? s)
            {
                if (string.IsNullOrEmpty(s)) return "";
                return $"\"{s.Replace("\"", "\"\"")}\"";
            }

            var sb = new StringBuilder();
            sb.AppendLine("Id,FirstName,LastName,NationalId,Email,Phone,School,Department,StartDate,EndDate,Status");

            foreach (var i in res.Value!.Items)
            {
                var start = i.StartDate.ToString("yyyy-MM-dd");
                var end   = i.EndDate.HasValue ? i.EndDate.Value.ToString("yyyy-MM-dd") : "";

                sb.AppendLine(string.Join(",",
                    i.Id,
                    CsvEsc(i.FirstName),
                    CsvEsc(i.LastName),
                    CsvEsc(i.NationalId),
                    CsvEsc(i.Email),
                    CsvEsc(i.Phone),
                    CsvEsc(i.School),
                    CsvEsc(i.Department),
                    start,
                    end,
                    CsvEsc(i.Status)
                ));
            }

            var bytes = new UTF8Encoding(true).GetBytes(sb.ToString());
            var fileName = $"stajyerler_{DateTime.Now:yyyyMMdd_HHmm}.csv";
            return File(bytes, "text/csv; charset=utf-8", fileName);
        }

        [HttpGet]
        public async Task<IActionResult> ExportXlsx(
            string? q, string? status,
            string sortField = "LastName",
            string sortOrder = "asc")
        {
            const int Huge = 1_000_000;
            var res = await _mediator.Send(new GetInternsQuery(q, status, 1, Huge, sortField, sortOrder));
            if (!res.Succeeded) return Problem(res.Error);

            using var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Stajyerler");

            var headers = new[]
            {
                "Id","Ad","Soyad","TC Kimlik","Email","Telefon",
                "Okul","Bölüm","Başlangıç","Bitiş","Durum"
            };
            for (int c = 0; c < headers.Length; c++)
                ws.Cell(1, c + 1).Value = headers[c];

            int r = 2;
            foreach (var i in res.Value!.Items)
            {
                ws.Cell(r, 1).Value = i.Id;
                ws.Cell(r, 2).Value = i.FirstName;
                ws.Cell(r, 3).Value = i.LastName;
                ws.Cell(r, 4).Value = i.NationalId;
                ws.Cell(r, 5).Value = i.Email;
                ws.Cell(r, 6).Value = i.Phone ?? "";
                ws.Cell(r, 7).Value = i.School ?? "";
                ws.Cell(r, 8).Value = i.Department ?? "";
                ws.Cell(r, 9).Value = i.StartDate.ToDateTime(TimeOnly.MinValue);
                ws.Cell(r, 9).Style.DateFormat.Format = "yyyy-mm-dd";

                if (i.EndDate.HasValue)
                {
                    ws.Cell(r, 10).Value = i.EndDate.Value.ToDateTime(TimeOnly.MinValue);
                    ws.Cell(r, 10).Style.DateFormat.Format = "yyyy-mm-dd";
                }
                else
                {
                    ws.Cell(r, 10).Value = "";
                }

                ws.Cell(r, 11).Value = i.Status;
                r++;
            }

            var rng = ws.Range(1, 1, r - 1, headers.Length);
            rng.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            rng.Style.Border.InsideBorder  = XLBorderStyleValues.Dotted;
            ws.Row(1).Style.Font.Bold = true;
            ws.SheetView.FreezeRows(1);
            ws.RangeUsed()?.SetAutoFilter();
            ws.Columns().AdjustToContents();

            using var ms = new MemoryStream();
            wb.SaveAs(ms);
            var bytes = ms.ToArray();
            var fileName = $"stajyerler_{DateTime.Now:yyyyMMdd_HHmm}.xlsx";
            return File(bytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult DownloadCsvTemplate()
        {
            var header = "FirstName,LastName,NationalId,Email,Phone,School,Department,StartDate,EndDate,Status\n";
            var bytes = new UTF8Encoding(true).GetBytes(header);
            return File(bytes, "text/csv; charset=utf-8", "stajyer_sablon.csv");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult UploadCsv() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UploadCsv(IFormFile? file)
        {
            var result = new ImportResultVm();

            if (file is null || file.Length == 0)
            {
                ModelState.AddModelError("", "Lütfen bir CSV dosyası seçin.");
                return View(result);
            }

            var allowed = new[] { "text/csv", "application/vnd.ms-excel", "application/octet-stream" };
            if (!allowed.Contains(file.ContentType) && !file.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError("", "Yalnızca .csv dosyaları yükleyin.");
                return View(result);
            }

            using var stream = file.OpenReadStream();
            using var reader = new StreamReader(stream, Encoding.UTF8, detectEncodingFromByteOrderMarks: true);

            // header
            if (!reader.EndOfStream) await reader.ReadLineAsync();

            string? line;
            int lineNo = 1;

            while (!reader.EndOfStream)
            {
                line = await reader.ReadLineAsync();
                lineNo++;
                if (string.IsNullOrWhiteSpace(line)) continue;

                result.TotalRows++;

                var parts = SplitCsvLine(line);
                if (parts.Length < 10)
                {
                    result.Skipped++;
                    result.Errors.Add($"Satır {lineNo}: Beklenen 10 sütun, bulunan {parts.Length}.");
                    continue;
                }

                var model = new Intern
                {
                    FirstName  = parts[0]?.Trim() ?? "",
                    LastName   = parts[1]?.Trim() ?? "",
                    NationalId = parts[2]?.Trim() ?? "",
                    Email      = parts[3]?.Trim() ?? "",
                    Phone      = EmptyToNull(parts[4]),
                    School     = EmptyToNull(parts[5]),
                    Department = EmptyToNull(parts[6]),
                    Status     = string.IsNullOrWhiteSpace(parts[9]) ? "Aktif" : parts[9]!.Trim()
                };

                if (!TryParseDateOnly(parts[7], out var start))
                {
                    result.Skipped++;
                    result.Errors.Add($"Satır {lineNo}: Başlangıç tarihi geçersiz (yyyy-MM-dd).");
                    continue;
                }
                model.StartDate = start;

                if (string.IsNullOrWhiteSpace(parts[8]))
                {
                    model.EndDate = null;
                }
                else if (TryParseDateOnly(parts[8], out var end))
                {
                    model.EndDate = end;
                }
                else
                {
                    result.Skipped++;
                    result.Errors.Add($"Satır {lineNo}: Bitiş tarihi geçersiz (yyyy-MM-dd).");
                    continue;
                }

                var ctx = new ValidationContext(model);
                var validationResults = new List<ValidationResult>();
                if (!Validator.TryValidateObject(model, ctx, validationResults, validateAllProperties: true))
                {
                    result.Skipped++;
                    var msgs = string.Join("; ", validationResults.Select(v => v.ErrorMessage));
                    result.Errors.Add($"Satır {lineNo}: {msgs}");
                    continue;
                }

                var createRes = await _mediator.Send(new CreateInternCommand(model));
                if (!createRes.Succeeded)
                {
                    result.Skipped++;
                    result.Errors.Add($"Satır {lineNo}: {createRes.Error}");
                }
                else
                {
                    result.Inserted++;
                }
            }

            return View(result);

            static string? EmptyToNull(string? s) => string.IsNullOrWhiteSpace(s) ? null : s;

            static bool TryParseDateOnly(string? s, out DateOnly value)
            {
                value = default;
                if (string.IsNullOrWhiteSpace(s)) return false;

                if (DateTime.TryParseExact(
                        s.Trim(),
                        "yyyy-MM-dd",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out var dt))
                {
                    value = DateOnly.FromDateTime(dt);
                    return true;
                }
                return false;
            }

            static string[] SplitCsvLine(string line)
            {
                var raw = line.Split(',');
                for (int i = 0; i < raw.Length; i++)
                {
                    var cell = raw[i].Trim();
                    if (cell.StartsWith("\"") && cell.EndsWith("\"") && cell.Length >= 2)
                        cell = cell[1..^1].Replace("\"\"", "\"");
                    raw[i] = cell;
                }
                return raw;
            }
        }
    }
}
