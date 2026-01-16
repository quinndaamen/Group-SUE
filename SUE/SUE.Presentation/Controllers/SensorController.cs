using Microsoft.AspNetCore.Mvc;
using SUE.Data;

public class SensorsController : Controller
{
    private readonly AppDbContext _context;

    public SensorsController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Test()
    {
        var data = _context.Measurements
            .OrderByDescending(m => m.Timestamp)
            .Take(50)
            .ToList();

        return View(data);
    }
}