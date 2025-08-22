//using IvanSusaninProject_Contracts.AdapterContracts;
//using IvanSusaninProject_Contracts.BindingModels;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;

//namespace MVC_Project.Controllers;

//public class TourController : Controller
//{    
//    private readonly IGroupAdapter _groupAdapter;
//    private readonly ITourAdapter _tourAdapter;
//    private readonly IExcursionAdapter _excursionAdapter;


//    public TourController(ITourAdapter tourAdapter, IGroupAdapter groupAdapter, IExcursionAdapter excursionAdapter)
//    {
//        _tourAdapter = tourAdapter;
//        _groupAdapter = groupAdapter;
//        _excursionAdapter = excursionAdapter;
//    }

//    public IActionResult Index()
//    {
//        var tours = _tourAdapter.GetList();
//        return View(tours);
//    }

//    public IActionResult Create()
//    {
//        // Заполняем списки для выбора
//        ViewBag.Groups = new SelectList(_groupService.GetAllGroups(), "Id", "Name");
//        ViewBag.Excursions = new SelectList(_excursionService.GetAllExcursions(), "Id", "Name");

//        // Устанавливаем текущего пользователя
//        var model = new TourBindingModel
//        {
//            ExecutorId = GetUserId()
//        };

//        return View(model);
//    }

//    [HttpPost]
//    public IActionResult Create(TourBindingModel model, List<string> SelectedGroupIds, List<string> SelectedExcursionIds)
//    {
//        if (ModelState.IsValid)
//        {
//            // Преобразуем выбранные ID в соответствующие модели
//            model.Groups = SelectedGroupIds?.Select(groupId => new TourGroupBindingModel
//            {
//                GroupId = groupId,
//                TourId = model.Id
//            }).ToList();

//            model.Excursions = SelectedExcursionIds?.Select(excursionId => new TourExcursionBindingModel
//            {
//                ExcursionId = excursionId,
//                TourId = model.Id
//            }).ToList();

//            _tourService.CreateTour(model);
//            return RedirectToAction("Index");
//        }

//        // Если модель невалидна, повторно заполняем списки
//        ViewBag.Groups = new SelectList(_groupService.GetAllGroups(), "Id", "Name");
//        ViewBag.Excursions = new SelectList(_excursionService.GetAllExcursions(), "Id", "Name");

//        return View(model);
//    }
//}
