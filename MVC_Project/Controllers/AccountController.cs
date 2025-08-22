using IvanSusaninProject_Contracts.AdapterContracts;
using IvanSusaninProject_Contracts.BindingModels;
using IvanSusaninProject_Contracts.BusinessLogicsContracts;
using IvanSusaninProject_Contracts.DataModels;
using IvanSusaninProject_Contracts.Enums;
using IvanSusaninProject_Contracts.Exceptions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_Project.Models;
using System.Security.Claims;
using System.Text;

namespace MVC_Project.Controllers;

public class AccountController : Controller
{
    private readonly IUserAdapter _userAdapter;
    private readonly ILogger<AccountController> _logger;

    public AccountController(IUserAdapter userAdapter, ILogger<AccountController> logger)
    {
        _userAdapter = userAdapter;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View(); // Показывает форму логина
    }

    [HttpPost]
    public async Task<IActionResult> Login(UserLoginBindingModel model)
    {
        _logger.LogInformation(model.Login + " Starting login");

        if (!ModelState.IsValid)
        {
            return View(model); // Если модель невалидна, возвращаем форму с ошибками
        }

        _logger.LogInformation(model.Login + " Model is valid");

        // Пытаемся найти пользователя по логину
        var user = _userAdapter.Login(model.Login, model.Password);

        if (user == null)
        {
            _logger.LogWarning("Login failed for user: {Login}", model.Login);
            ModelState.AddModelError("", "Неверный логин или пароль");
            return View(model);
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Login),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email), // ← вот это добавляем
            new Claim(ClaimTypes.Role, user.UserRole.ToString())
        };
        _logger.LogInformation("Login successful for user: {Login}", model.Login);
        var identity = new ClaimsIdentity(claims, "AuthAppCookie");
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync("AuthAppCookie", principal);

        return RedirectToAction("Index", "Home"); //Dashboard -> Index
    }
    [Authorize]
    [HttpGet]
    public IActionResult Profile()
    {
        // Получаем Id пользователя из Claims
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
        {
            return Unauthorized(); // Или RedirectToAction("Login");
        }

        var user = _userAdapter.GetElementById(userIdClaim.Value);

        if (user == null)
        {
            return NotFound();
        }



        // Создаем модель для передачи во View
        var model = new UserRegisterBindingModel
        {
            Login = user.Login,
            Email = user.Email,
            Password = "",
            Role = user.Role
        };

        return View(model);
    }
    [Authorize]
    [HttpGet]
    public IActionResult ChangeData()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || string.IsNullOrWhiteSpace(userIdClaim.Value))
        {
            return Unauthorized();
        }

        var user = _userAdapter.GetElementById(userIdClaim.Value); // передаем строку напрямую
        if (user == null)
        {
            return NotFound();
        }

        var model = new UserRegisterBindingModel
        {
            Login = user.Login,
            Email = user.Email,
            Password = "" // пользователь сам введёт новый пароль при необходимости
        };

        return View(model);
    }
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> ChangeData(UserRegisterBindingModel model)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || string.IsNullOrWhiteSpace(userIdClaim.Value))
        {
            return Unauthorized();
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            // Передаем строковый Id напрямую
            _userAdapter.Update(model, userIdClaim.Value);

            // Получаем обновлённые данные пользователя
            var updatedUser = _userAdapter.Login(model.Login, model.Password);
            if (updatedUser == null)
            {
                return NotFound();
            }

            // Обновляем claims (Id остаётся в строковом формате)
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, updatedUser.Login),
            new Claim(ClaimTypes.NameIdentifier, updatedUser.Id.ToString()), // Предполагая, что Id уже Guid
            new Claim(ClaimTypes.Email, updatedUser.Email)
        };

            var identity = new ClaimsIdentity(claims, "AuthAppCookie");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("AuthAppCookie", principal);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при обновлении данных пользователя");
            ModelState.AddModelError("", "Произошла ошибка при сохранении данных");
            return View(model);
        }

        return RedirectToAction("Profile");
    }


    [HttpGet]
    public IActionResult Register()
    {
        return View(); // Показывает форму регистрации
    }

    [HttpPost]
    public IActionResult Register(UserRegisterBindingModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model); // Если модель невалидна, возвращаем форму с ошибками
        }

        // Можно добавить дополнительную проверку уникальности логина или почты
        _logger.LogInformation("Registering user with login: {Login}", model.Login);
        var existingUser = _userAdapter.CheckLoginExists(model.Login);
        if (existingUser)
        {
            ModelState.AddModelError("", "Этот логин уже занят.");
            _logger.LogInformation("Login already exists: {Login}", model.Login);
            return View(model);
        }
        _logger.LogInformation("Registering user with email: {Email}", model.Email);
        _userAdapter.Register(model);
        return RedirectToAction("Login");
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("AuthAppCookie");
        return RedirectToAction("Login");
    }

    public IActionResult AccessDenied()
    {
        return View(); // Страница при запрете доступа
    }
}