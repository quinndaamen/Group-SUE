using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace SUE.Presentation.Controllers;

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