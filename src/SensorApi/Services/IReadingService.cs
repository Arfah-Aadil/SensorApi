using SensorApi.Models;

namespace SensorApi.Services;

public interface IReadingService
{
    AddReadingResult AddReading(string bioreactorId, CreateReadingRequest request);
    IReadOnlyList<SensorReading> GetReadings(string bioreactorId);
    ReadingSummary? GetSummary(string bioreactorId, SensorType type);
}