using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace SUE.Presentation.Controllers;

[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
public class GraphController : Controller
{
    public GraphController()
    {
    }
    
    public IActionResult ElectricityGraph()
    {
        return View();
    }

    public IActionResult AirGraph()
    {
        return View();
    }
}