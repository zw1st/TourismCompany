using AutoMapper;
using IvanSusaninProject_Contracts.AdapterContracts;
using IvanSusaninProject_Contracts.BindingModels;
using IvanSusaninProject_Contracts.BusinessLogicsContracts;
using IvanSusaninProject_Contracts.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MVC_Project.Controllers;

public class GroupController : Controller
{

    private readonly IGroupAdapter _groupAdapter;
    private readonly IPlaceAdapter _placeAdapter;
    public GroupController(IGroupAdapter groupAdapter, IPlaceAdapter placeAdapter)
    {
        _groupAdapter = groupAdapter;
        _placeAdapter = placeAdapter;
    }

    public IActionResult Index()
    {
        var groups = _groupAdapter.GetList(GetUserId());
        return View(groups);
    }

    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Create(GroupBindingModel groupBindingModel)
    {
        var userId = GetUserId();
        groupBindingModel.UserId = userId;
        _groupAdapter.RegisterGroup(groupBindingModel);

        return RedirectToAction("Index");
    }

    public IActionResult Edit(string id)
    {
        var userId = GetUserId();
        var animal = _groupAdapter.GetElement(userId, id);
        if (animal == null)
        {
            return NotFound();
        }
        return View(animal);
    }

    [HttpPost]
    public IActionResult Edit(GroupBindingModel groupBindingModel)
    {

        var userId = GetUserId();
        groupBindingModel.UserId = userId;
        _groupAdapter.ChangeGroupInfo(groupBindingModel);
        return RedirectToAction("Index");
    }

    public IActionResult Details(string id)
    {
        var userId = GetUserId();
        var group = _groupAdapter.GetElement(userId, id);
        if (group == null)
        {
            return NotFound();
        }
        return View(group);
    }

    public IActionResult Delete(string id)
    {
        var userId = GetUserId();
        _groupAdapter.RemoveGroup(userId, id);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Bind()
    {
        var userId = GetUserId();

        var viewModel = new LinkGroupToPlaceViewModel()
        {
            Groups = _groupAdapter.GetList(userId),
            Places = _placeAdapter.GetList()
        };

        return View(viewModel);
    }

    [HttpPost]
    public IActionResult Bind(LinkGroupToPlaceViewModel model)
    {
        var userId = GetUserId();
        if (ModelState.IsValid)
        {
            _groupAdapter.LinkGroupWithPlace(userId, model.SelectedGroupId, model.SelectedPlaceId);

            return RedirectToAction("Index");
        }

        // если ошибка — подгрузи списки снова
        model.Groups = _groupAdapter.GetList(userId);
        model.Places = _placeAdapter.GetList();

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
