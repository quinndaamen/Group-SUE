using Microsoft.AspNetCore.Mvc;

namespace SUE.Presentation.Controllers;

public class AuthController : Controller
{
    // GET
    public IActionResult Register()
    {
        return View();
    }

    public IActionResult Login()
    {
        return View();
    }
}
