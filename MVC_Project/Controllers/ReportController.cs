using IvanSusaninProject_Contracts.AdapterContracts;
using IvanSusaninProject_Contracts.BindingModels;
using IvanSusaninProject_Contracts.BusinessLogicsContracts;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using IvanSusaninProject_Contracts.Infrastructure;
using IvanSusaninProject_Contracts.BindingModels.ReportBindingModels;

namespace MVC_Project.Controllers;

public class ReportController : Controller 
{
    private readonly IReportContract _reportContract;
    private readonly IConfiguration _config;
    private readonly ITourAdapter _tourAdapter;
    private readonly ITripAdapter _tripAdapter;

    public ReportController(IReportContract reportContract, IConfiguration config, ITourAdapter tourAdapter, ITripAdapter tripAdapter)
    {
        _reportContract = reportContract;
        _config = config;
        _tourAdapter = tourAdapter;
        _tripAdapter = tripAdapter;
    }

    //---
    public IActionResult TourPlacesReport()
    {
        var userId = GetUserId();
        if (userId == null)
            return Unauthorized();

        var tours = _tourAdapter.GetList(userId);
        ViewBag.Tours = tours;

        return View();
    }
    [HttpPost]
    public async Task<IActionResult> TourPlacesReport(TourPlacesReportBindingModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Tours = _tourAdapter.GetList(GetUserId());
            return View(model);
        }

        var userId = GetUserId();
        if (userId == null)
        {
            ModelState.AddModelError("", "Не удалось определить пользователя.");
            return View(model);
        }
        try
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            Stream stream;

            if (model.FileFormat == ".docx")
            {
                stream = await _reportContract.CreateWordDocumentPlacesByTours(
                    model.SelectedTourIds, cts.Token);
            }
            else if (model.FileFormat == ".xlsx")
            {
                stream = await _reportContract.CreateExcelDocumentPlacesByTours(
                    model.SelectedTourIds, cts.Token);
            }
            else
            {
                throw new NotSupportedException($"Формат {model.FileFormat} не поддерживается");
            }

            // Возвращаем файл напрямую, без сохранения на диск
            var fileName = $"tour_places_report_{DateTime.Now:yyyyMMdd_HHmm}{model.FileFormat}";

            // Определяем content type
            var contentType = model.FileFormat == ".docx"
                ? "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
                : "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            // Сбрасываем позицию потока
            stream.Position = 0;

            return File(stream, contentType, fileName);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Ошибка при формировании отчета: {ex.Message}");
        }
    }
    //---

    public IActionResult TripExcursionsReport()

    {
        var userId = GetUserId();
        if (userId == null)
            return Unauthorized();

        var trips = _tripAdapter.GetList(userId);
        ViewBag.Trips = trips;

        return View();
    }
    [HttpPost]
    public async Task<IActionResult> TripExcursionsReport(TripExcursionsReportBindingModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Trips = _tripAdapter.GetList(GetUserId());
            return View(model);
        }

        var userId = GetUserId();
        if (userId == null)
        {
            ModelState.AddModelError("", "Не удалось определить пользователя.");
            return View(model);
        }
        try
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            Stream stream;

            if (model.FileFormat == ".docx")
            {
                stream = await _reportContract.CreateWordDocumentExcursionsByTrips(
                    model.SelectedTripIds, cts.Token);
            }
            else if (model.FileFormat == ".xlsx")
            {
                stream = await _reportContract.CreateExcelDocumentExcursionsByTrips(
                    model.SelectedTripIds, cts.Token);
            }
            else
            {
                throw new NotSupportedException($"Формат {model.FileFormat} не поддерживается");
            }

            // Возвращаем файл напрямую, без сохранения на диск
            var fileName = $"trip_excursions_report_{DateTime.Now:yyyyMMdd_HHmm}{model.FileFormat}";

            // Определяем content type
            var contentType = model.FileFormat == ".docx"
                ? "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
                : "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            // Сбрасываем позицию потока
            stream.Position = 0;

            return File(stream, contentType, fileName);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Ошибка при формировании отчета: {ex.Message}");
        }
    }
    //---

    public IActionResult TourReport()
    {
        return View(new RequestReportBindingModel() { StartDate = DateTime.Now.AddDays(-30), EndDate = DateTime.Now });
    }
    [HttpPost]
    public async Task<IActionResult> TourReport(RequestReportBindingModel model, [FromQuery] bool preview = false)
    {
        if (!ModelState.IsValid)

            return View(model);

        var workerId = GetUserId();
        if (workerId == null)
        {
            ModelState.AddModelError("", "Пользователь не авторизован.");
            return View(model);
        }

        try
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            var stream = await _reportContract.CreatePdfDocumentToursDetailsByPeriod(model.StartDate, model.EndDate, cts.Token);

            if (preview)
            {
                return File(stream, "application/pdf");
            }
            else
            {
                var fileName = $"Tour_report_{DateTime.Now:yyyyMMdd_HHmm}.pdf";
                var tempFolder = Path.Combine(Path.GetTempPath(), "TourReports");
                Directory.CreateDirectory(tempFolder);
                var tempPath = Path.Combine(tempFolder, fileName);

                using (var fileStream = System.IO.File.Create(tempPath))
                {
                    stream.Position = 0;
                    stream.CopyTo(fileStream);
                }

                return Json(new { fileName });
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Ошибка при формировании отчета: {ex.Message}");
        }
    }
    [HttpPost]
    public async Task<IActionResult> SendTourReportByEmail([FromBody] RequestReportBindingModel animalReportRequest)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var filePath = Path.Combine(Path.GetTempPath(), "TourReports", animalReportRequest.FileName);
            if (!System.IO.File.Exists(filePath))
                return NotFound("Файл отчета не найден");

            // Проверка типа файла
            var ext = Path.GetExtension(filePath).ToLower();
            if (ext != ".pdf")
                return BadRequest("Недопустимый формат файла");

            // Получаем настройки из конфигурации
            var emailSettings = new EmailSettings
            {
                SmtpServer = _config["EmailSettings:SmtpServer"],
                SmtpPort = int.Parse(_config["EmailSettings:SmtpPort"]),
                SmtpUsername = _config["EmailSettings:SmtpUsername"],
                SmtpPassword = _config["EmailSettings:SmtpPassword"],
                FromEmail = _config["EmailSettings:FromEmail"],
                FromName = _config["EmailSettings:FromName"]
            };

            // Генерация тела письма
            var emailBody = $@"
                    <h3>Отчет по турам</h3>
                    <p><small>Сообщение сгенерировано автоматически</small></p>
                ";

            await SendEmailWithAttachment(emailSettings, User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value, "Без темы", emailBody, filePath);

            return Ok(new { success = true, message = "Отчет успешно отправлен" });
        }
        catch (SmtpException ex)
        {
            return StatusCode(500, new { error = "Ошибка почтового сервера" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }
    //---
    public IActionResult TripReport()
    {
        return View(new RequestReportBindingModel() { StartDate = DateTime.Now.AddDays(-30), EndDate = DateTime.Now });
    }
    [HttpPost]
    public async Task<IActionResult> TripReport(RequestReportBindingModel model, [FromQuery] bool preview = false)
    {
        if (!ModelState.IsValid)

            return View(model);

        var workerId = GetUserId();
        if (workerId == null)
        {
            ModelState.AddModelError("", "Пользователь не авторизован.");
            return View(model);
        }

        try
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            var stream = await _reportContract.CreatePdfDocumentTripsDetailsByPeriod(model.StartDate, model.EndDate, cts.Token);

            if (preview)
            {
                return File(stream, "application/pdf");
            }
            else
            {
                var fileName = $"Trip_report_{DateTime.Now:yyyyMMdd_HHmm}.pdf";
                var tempFolder = Path.Combine(Path.GetTempPath(), "TripReports");
                Directory.CreateDirectory(tempFolder);
                var tempPath = Path.Combine(tempFolder, fileName);

                using (var fileStream = System.IO.File.Create(tempPath))
                {
                    stream.Position = 0;
                    stream.CopyTo(fileStream);
                }

                return Json(new { fileName });
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Ошибка при формировании отчета: {ex.Message}");
        }
    }
    [HttpPost]
    public async Task<IActionResult> SendTripReportByEmail([FromBody] RequestReportBindingModel animalReportRequest)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var filePath = Path.Combine(Path.GetTempPath(), "TripReports", animalReportRequest.FileName);
            if (!System.IO.File.Exists(filePath))
                return NotFound("Файл отчета не найден");

            // Проверка типа файла
            var ext = Path.GetExtension(filePath).ToLower();
            if (ext != ".pdf")
                return BadRequest("Недопустимый формат файла");

            // Получаем настройки из конфигурации
            var emailSettings = new EmailSettings
            {
                SmtpServer = _config["EmailSettings:SmtpServer"],
                SmtpPort = int.Parse(_config["EmailSettings:SmtpPort"]),
                SmtpUsername = _config["EmailSettings:SmtpUsername"],
                SmtpPassword = _config["EmailSettings:SmtpPassword"],
                FromEmail = _config["EmailSettings:FromEmail"],
                FromName = _config["EmailSettings:FromName"]
            };

            // Генерация тела письма
            var emailBody = $@"
                    <h3>Отчет по поездкам</h3>
                    <p><small>Сообщение сгенерировано автоматически</small></p>
                ";

            await SendEmailWithAttachment(emailSettings, User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value, "Без темы", emailBody, filePath);

            return Ok(new { success = true, message = "Отчет успешно отправлен" });
        }
        catch (SmtpException ex)
        {
            return StatusCode(500, new { error = "Ошибка почтового сервера" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    //---
    private string GetUserId()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            ModelState.AddModelError("", "Не удалось определить пользователя.");
            return null;
        }

        if (Guid.TryParse(userId, out var guidId))
        {
            return userId;
        }

        ModelState.AddModelError("", "Идентификатор пользователя имеет неверный формат.");
        return null;
    }
    private async Task SendEmailWithAttachment(EmailSettings settings, string emailTo, string subject, string body, string attachmentPath)
    {
        try
        {
            using var message = new MailMessage();
            message.From = new MailAddress(settings.FromEmail, settings.FromName);
            message.To.Add(emailTo);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            using var attachment = new Attachment(attachmentPath);
            message.Attachments.Add(attachment);

            using var smtpClient = new SmtpClient(settings.SmtpServer, settings.SmtpPort)
            {
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(settings.SmtpUsername, settings.SmtpPassword),
                Timeout = 5000
            };

            Console.WriteLine($"Попытка отправки через {settings.SmtpServer}:{settings.SmtpPort}");

            await smtpClient.SendMailAsync(message);

            Console.WriteLine("Письмо успешно отправлено");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка отправки: {ex}");
            throw;

        }
    }

}
