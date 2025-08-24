using IvanSusaninProject_Contracts.AdapterContracts;
using IvanSusaninProject_Contracts.BindingModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC_Project.Adapters;
using System.Security.Claims;

namespace MVC_Project.Controllers;

public class TripController : Controller
{
    private readonly IPlaceAdapter _placeAdapter;
    private readonly ITripAdapter _tripAdapter;
    private readonly IGuideAdapter _guideAdapter;

    public TripController(IPlaceAdapter placeAdapter, ITripAdapter tripAdapter, IGuideAdapter guideAdapter)
    {
        _placeAdapter = placeAdapter;
        _tripAdapter = tripAdapter;
        _guideAdapter = guideAdapter;
    }

    public IActionResult Index()
    {
        var trips = _tripAdapter.GetListWithDetails();
        return View(trips);
    }

    public IActionResult Create()
    {
        ViewBag.Guides = new SelectList(_guideAdapter.GetList(), "Id", "Fio");
        ViewBag.Places = new SelectList(_placeAdapter.GetList(), "Id", "Name");

        var model = new TripBindingModel
        {
            UserId = GetUserId()
        };

        return View(model);
    }

    [HttpPost]
    public IActionResult Create(TripBindingModel model)
    {
        if (ModelState.IsValid)
        {
            try
            {
                _tripAdapter.RegisterTripWithRelations(
                    model,
                    model.SelectedPlacesIds,
                    model.SelectedGuidesIds
                );

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Ошибка при создании тура: {ex.Message}");
            }
        }

        ViewBag.Places = new SelectList(_placeAdapter.GetList(), "Id", "Name");
        ViewBag.Guides = new SelectList(_guideAdapter.GetList(), "Id", "Fio");

        return View(model);
    }

    public IActionResult Details(string id)
    {
        var userId = GetUserId();
        var trip = _tripAdapter.GetElement(null, id);
        if (trip == null)
        {
            return NotFound();
        }
        return View(trip);
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
