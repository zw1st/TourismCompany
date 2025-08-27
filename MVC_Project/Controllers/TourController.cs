using IvanSusaninProject_Contracts.AdapterContracts;
using IvanSusaninProject_Contracts.BindingModels;
using IvanSusaninProject_Contracts.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC_Project.Adapters;
using System.Security.Claims;

namespace MVC_Project.Controllers;

public class TourController : Controller
{
    private readonly IGroupAdapter _groupAdapter;
    private readonly ITourAdapter _tourAdapter;
    private readonly IExcursionAdapter _excursionAdapter;


    public TourController(ITourAdapter tourAdapter, IGroupAdapter groupAdapter, IExcursionAdapter excursionAdapter)
    {
        _tourAdapter = tourAdapter;
        _groupAdapter = groupAdapter;
        _excursionAdapter = excursionAdapter;
    }

    public IActionResult Index()
    {
        var userId = GetUserId();
        var tours = _tourAdapter.GetListWithDetails(userId);
        return View(tours);
    }

    public IActionResult Create()
    {
        // Заполняем списки для выбора
        ViewBag.Groups = new SelectList(_groupAdapter.GetList(), "Id", "Name");
        ViewBag.Excursions = new SelectList(_excursionAdapter.GetList(), "Id", "Name");

        // Устанавливаем текущего пользователя
        var model = new TourBindingModel
        {
            UserId = GetUserId()
        };

        return View(model);
    }

    [HttpPost]
    public IActionResult Create(TourBindingModel model)
    {
        if (ModelState.IsValid)
        {
            try
            {
                _tourAdapter.RegisterTourWithRelations(
                    model,
                    model.SelectedGroupIds,
                    model.SelectedExcursionIds
                );

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Ошибка при создании тура: {ex.Message}");
            }
        }

        ViewBag.Groups = new SelectList(_groupAdapter.GetList(), "Id", "Name");
        ViewBag.Excursions = new SelectList(_excursionAdapter.GetList(), "Id", "Name");

        return View(model);
    }

    public IActionResult Details(string id)
    {
        var userId = GetUserId();
        var group = _tourAdapter.GetElement(null, id);
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
