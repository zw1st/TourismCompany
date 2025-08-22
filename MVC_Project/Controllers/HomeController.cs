using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC_Project.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace MVC_Project.Controllers;

[Authorize]
public class HomeController : Controller
{

    private readonly ILogger<HomeController> _logger;


    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }


    public IActionResult Privacy()
    {
        return View();
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }


    [Authorize]
    public IActionResult Index()
    {
        var role = User.FindFirst(ClaimTypes.Role)?.Value;
        return View(model: role);
    }


    [Authorize(Roles = "User")]
    public IActionResult A()
    {
        return View();
    }


    [Authorize(Roles = "Guarantor")]
    public IActionResult X()
    {
        return View();
    }
}

