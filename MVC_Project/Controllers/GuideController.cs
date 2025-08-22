using IvanSusaninProject_Contracts.AdapterContracts;
using IvanSusaninProject_Contracts.BindingModels;
using IvanSusaninProject_Contracts.ViewModels;
using Microsoft.AspNetCore.Mvc;
using MVC_Project.Adapters;
using System.Security.Claims;

namespace MVC_Project.Controllers;

public class GuideController : Controller
{
    private readonly IGuideAdapter _guideAdapter;
    private readonly IExcursionAdapter _excursionAdapter;

    public GuideController(IGuideAdapter guideAdapter, IExcursionAdapter excursionAdapter)
    {
        _guideAdapter = guideAdapter;
        _excursionAdapter = excursionAdapter;
    }

    public IActionResult Index()
    {
        var groups = _guideAdapter.GetList(GetUserId());
        return View(groups);
    }

    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Create(GuideBindingModel guideBindingModel)
    {
        var userId = GetUserId();
        guideBindingModel.UserId = userId;
        _guideAdapter.RegisterGuide(guideBindingModel);

        return RedirectToAction("Index");
    }

    public IActionResult Edit(string id)
    {
        var userId = GetUserId();
        var animal = _guideAdapter.GetElement(userId, id);
        if (animal == null)
        {
            return NotFound();
        }
        return View(animal);
    }

    [HttpPost]
    public IActionResult Edit(GuideBindingModel groupBindingModel)
    {
        var userId = GetUserId();
        groupBindingModel.UserId = userId;
        _guideAdapter.ChangeGuideInfo(groupBindingModel);
        return RedirectToAction("Index");
    }

    public IActionResult Details(string id)
    {
        var userId = GetUserId();
        var guide = _guideAdapter.GetElement(userId, id);
        if (guide == null)
        {
            return NotFound();
        }
        return View(guide);
    }

    public IActionResult Delete(string id)
    {
        var userId = GetUserId();
        _guideAdapter.RemoveGuide(userId, id);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Bind()
    {
        var userId = GetUserId();

        var viewModel = new LinkGuideToExcursionViewModel()
        {
            Guides = _guideAdapter.GetList(userId),
            Excursions = _excursionAdapter.GetList()
        };

        return View(viewModel);
    }

    [HttpPost]
    public IActionResult Bind(LinkGuideToExcursionViewModel model)
    {
        var userId = GetUserId();
        if (ModelState.IsValid)
        {
            _guideAdapter.LinkGuideToExcursion(userId, model.SelectedGuideId, model.SelectedExcursionId);

            return RedirectToAction("Index");
        }

        // если ошибка — подгрузи списки снова
        model.Guides = _guideAdapter.GetList(userId);
        model.Excursions = _excursionAdapter.GetList();

        return View(model);
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
