using SensorApi.Models;

namespace SensorApi.Repositories;

public interface IReadingRepository
{
    void Add(SensorReading reading);
    IReadOnlyList<SensorReading> GetByBioreactor(string bioreactorId);
}