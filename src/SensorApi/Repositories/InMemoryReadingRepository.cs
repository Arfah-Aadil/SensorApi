using System.Collections.Concurrent;
using SensorApi.Models;

namespace SensorApi.Repositories;

public class InMemoryReadingRepository : IReadingRepository
{
    private readonly ConcurrentBag<SensorReading> _readings = new();

    public void Add(SensorReading reading) => _readings.Add(reading);

    public IReadOnlyList<SensorReading> GetByBioreactor(string bioreactorId) =>
        _readings
            .Where(r => r.BioreactorId == bioreactorId)
            .OrderBy(r => r.RecordedAt)
            .ToList();
}