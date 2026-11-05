using SensorApi.Models;
using SensorApi.Repositories;

namespace SensorApi.Services;

public class ReadingService : IReadingService
{
    private static readonly Dictionary<SensorType, (double Min, double Max)> AllowedRanges = new()
    {
        [SensorType.Temperature] = (20, 45),
        [SensorType.Ph] = (6.0, 8.0),
        [SensorType.DissolvedOxygen] = (0, 100)
    };

    private readonly IReadingRepository _repository;

    public ReadingService(IReadingRepository repository)
    {
        _repository = repository;
    }

    public AddReadingResult AddReading(string bioreactorId, CreateReadingRequest request)
    {
        if (string.IsNullOrWhiteSpace(bioreactorId))
            return new AddReadingResult(false, null, "Bioreactor ID is required.");

        if (!AllowedRanges.TryGetValue(request.Type, out var range))
            return new AddReadingResult(false, null, "Unknown sensor type.");

        if (!double.IsFinite(request.Value) || request.Value < range.Min || request.Value > range.Max)
            return new AddReadingResult(false, null,
                $"{request.Type} value {request.Value} is outside the allowed range {range.Min} to {range.Max}.");

        var reading = new SensorReading
        {
            Id = Guid.NewGuid(),
            BioreactorId = bioreactorId,
            Type = request.Type,
            Value = request.Value,
            RecordedAt = request.RecordedAt ?? DateTime.UtcNow
        };

        _repository.Add(reading);
        return new AddReadingResult(true, reading, null);
    }

    public IReadOnlyList<SensorReading> GetReadings(string bioreactorId) =>
        _repository.GetByBioreactor(bioreactorId);

    public ReadingSummary? GetSummary(string bioreactorId, SensorType type)
    {
        var values = _repository.GetByBioreactor(bioreactorId)
            .Where(r => r.Type == type)
            .Select(r => r.Value)
            .ToList();

        if (values.Count == 0)
            return null;

        return new ReadingSummary(type, values.Count, values.Min(), values.Max(),
            Math.Round(values.Average(), 2));
    }
}