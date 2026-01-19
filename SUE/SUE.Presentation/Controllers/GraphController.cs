using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SUE.Presentation.Models;
using SUE.Services.Sensors.Contracts;

namespace SUE.Presentation.Controllers;

[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
public class GraphsController : Controller
{
    private readonly IGraphService _graphService;

    public GraphsController(IGraphService graphService)
    {
        _graphService = graphService;
    }

    public async Task<IActionResult> Electricity()
    {
        var model = new GraphsViewModel
        {
            ElectricityLast4Hours = await _graphService.GetElectricityLast4HoursAsync()
        };

        return View(model);
    }
    
    public async Task<IActionResult> Air()
    {
        var model = new GraphsViewModel
        {
            AqiLast4Hours = await _graphService.GetAqiLast4HoursAsync()
        };

        return View(model);
    }
}