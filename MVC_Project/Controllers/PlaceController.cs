using IvanSusaninProject_Contracts.AdapterContracts;
using IvanSusaninProject_Contracts.BindingModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC_Project.Adapters;
using System.Security.Claims;

namespace MVC_Project.Controllers;

public class PlaceController : Controller
{
    private readonly IPlaceAdapter _placeAdapter;
    private readonly IGroupAdapter _groupAdapter;
    public PlaceController(IPlaceAdapter placeAdapter, IGroupAdapter groupAdapter)
    {
        _placeAdapter = placeAdapter;
        _groupAdapter = groupAdapter;
    }

    public IActionResult Index()
    {

        var exc = _placeAdapter.GetList(GetUserId());
        return View(exc);
    }

    public IActionResult Create()
    {
        var userId = GetUserId();
        var groups = _groupAdapter.GetList();
        ViewBag.Groups = new SelectList(groups, "Id", "Name");
        return View();
    }
    [HttpPost]
    public IActionResult Create(PlaceBindingModel placeBindingModel)
    {
        var userId = GetUserId();
        placeBindingModel.UserId = userId;
        _placeAdapter.RegisterPlace(placeBindingModel);

        return RedirectToAction("Index");
    }

    public IActionResult Edit(string id)
    {
        var userId = GetUserId();
        var groupes = _groupAdapter.GetList();
        ViewBag.Groupes = new SelectList(groupes, "Id", "Name");
        var place = _placeAdapter.GetElement(userId, id);
        if (place == null)
        {
            return NotFound();
        }
        return View(place);
    }

    [HttpPost]
    public IActionResult Edit(PlaceBindingModel groupBindingModel)
    {

        var userId = GetUserId();
        groupBindingModel.UserId = userId;
        _placeAdapter.ChangePlaceInfo(groupBindingModel);
        return RedirectToAction("Index");
    }

    public IActionResult Details(string id)
    {
        var userId = GetUserId();
        var group = _placeAdapter.GetElement(userId, id);
        if (group == null)
        {
            return NotFound();
        }
        return View(group);
    }

    public IActionResult Delete(string id)
    {
        var userId = GetUserId();
        _placeAdapter.RemovePlace(userId, id);
        return RedirectToAction("Index");
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
