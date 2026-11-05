namespace SensorApi.Models;

public record CreateReadingRequest(SensorType Type, double Value, DateTime? RecordedAt);

public record ReadingSummary(SensorType Type, int Count, double Min, double Max, double Average);

public record AddReadingResult(bool Success, SensorReading? Reading, string? Error);