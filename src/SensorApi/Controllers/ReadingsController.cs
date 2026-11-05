using Microsoft.AspNetCore.Mvc;
using SensorApi.Models;
using SensorApi.Services;

namespace SensorApi.Controllers;

[ApiController]
[Route("api/bioreactors/{bioreactorId}/readings")]
public class ReadingsController : ControllerBase
{
    private readonly IReadingService _service;

    public ReadingsController(IReadingService service)
    {
        _service = service;
    }

    [HttpPost]
    public IActionResult Add(string bioreactorId, CreateReadingRequest request)
    {
        var result = _service.AddReading(bioreactorId, request);

        if (!result.Success)
            return BadRequest(new { error = result.Error });

        return CreatedAtAction(nameof(GetAll), new { bioreactorId }, result.Reading);
    }

    [HttpGet]
    public ActionResult<IReadOnlyList<SensorReading>> GetAll(string bioreactorId) =>
        Ok(_service.GetReadings(bioreactorId));

    [HttpGet("summary")]
    public IActionResult Summary(string bioreactorId, [FromQuery] SensorType type)
    {
        var summary = _service.GetSummary(bioreactorId, type);
        return summary is null ? NotFound() : Ok(summary);
    }
}