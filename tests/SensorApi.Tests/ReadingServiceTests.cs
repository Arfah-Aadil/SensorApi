using SensorApi.Models;
using SensorApi.Repositories;
using SensorApi.Services;
using Xunit;

namespace SensorApi.Tests;

public class ReadingServiceTests
{
    private readonly ReadingService _service = new(new InMemoryReadingRepository());

    [Fact]
    public void AddReading_ValidValue_StoresReading()
    {
        var result = _service.AddReading("BR-01", new CreateReadingRequest(SensorType.Ph, 7.0, null));

        Assert.True(result.Success);
        Assert.NotNull(result.Reading);
        Assert.Single(_service.GetReadings("BR-01"));
    }

    [Theory]
    [InlineData(SensorType.Ph, 5.5)]
    [InlineData(SensorType.Ph, 8.5)]
    [InlineData(SensorType.Temperature, 50)]
    [InlineData(SensorType.DissolvedOxygen, -1)]
    public void AddReading_OutOfRange_IsRejected(SensorType type, double value)
    {
        var result = _service.AddReading("BR-01", new CreateReadingRequest(type, value, null));

        Assert.False(result.Success);
        Assert.Empty(_service.GetReadings("BR-01"));
    }

    [Fact]
    public void AddReading_NotANumber_IsRejected()
    {
        var result = _service.AddReading("BR-01", new CreateReadingRequest(SensorType.Ph, double.NaN, null));

        Assert.False(result.Success);
    }

    [Fact]
    public void AddReading_BlankBioreactorId_IsRejected()
    {
        var result = _service.AddReading("  ", new CreateReadingRequest(SensorType.Ph, 7.0, null));

        Assert.False(result.Success);
        Assert.Equal("Bioreactor ID is required.", result.Error);
    }

    [Fact]
    public void GetSummary_CalculatesMinMaxAndAverage()
    {
        _service.AddReading("BR-01", new CreateReadingRequest(SensorType.Ph, 6.8, null));
        _service.AddReading("BR-01", new CreateReadingRequest(SensorType.Ph, 7.0, null));
        _service.AddReading("BR-01", new CreateReadingRequest(SensorType.Ph, 7.2, null));

        var summary = _service.GetSummary("BR-01", SensorType.Ph);

        Assert.NotNull(summary);
        Assert.Equal(3, summary.Count);
        Assert.Equal(6.8, summary.Min);
        Assert.Equal(7.2, summary.Max);
        Assert.Equal(7.0, summary.Average);
    }

    [Fact]
    public void GetSummary_NoReadings_ReturnsNull()
    {
        Assert.Null(_service.GetSummary("BR-99", SensorType.Ph));
    }
}