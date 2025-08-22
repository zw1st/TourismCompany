using IvanSusaninProject_Contracts.AdapterContracts;
using IvanSusaninProject_Contracts.BindingModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC_Project.Adapters;
using System.Security.Claims;

namespace MVC_Project.Controllers;

public class ExcursionController : Controller
{
    private readonly IExcursionAdapter _excursionAdapter;
    private readonly IGuideAdapter _guideAdapter;
    public ExcursionController(IExcursionAdapter excursionAdapter, IGuideAdapter guideAdapter)
    {
        _excursionAdapter = excursionAdapter;
        _guideAdapter = guideAdapter;
    }
    public IActionResult Index()
    {

        var exc = _excursionAdapter.GetList(GetUserId(), null,  null);
        return View(exc);
    }

    public IActionResult Create()
    {
        var userId = GetUserId();
        var guides = _guideAdapter.GetList(); 
        ViewBag.Guides = new SelectList(guides, "Id", "Fio");
        return View();
    }
    [HttpPost]
    public IActionResult Create(ExcursionBindingModel excursionBindingModel)
    {
        var userId = GetUserId();
        excursionBindingModel.UserId = userId;
        _excursionAdapter.RegisterExcursion(excursionBindingModel);

        return RedirectToAction("Index");
    }

    public IActionResult Details(string id)
    {
        var userId = GetUserId();
        var group = _excursionAdapter.GetElement(userId, id);
        if (group == null)
        {
            return NotFound();
        }
        return View(group);
    }

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
}

