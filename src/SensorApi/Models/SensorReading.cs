namespace SensorApi.Models;

public enum SensorType
{
    Temperature,
    Ph,
    DissolvedOxygen
}

public class SensorReading
{
    public Guid Id { get; set; }
    public string BioreactorId { get; set; } = string.Empty;
    public SensorType Type { get; set; }
    public double Value { get; set; }
    public DateTime RecordedAt { get; set; }
}