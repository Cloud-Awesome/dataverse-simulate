using System;
using CloudAwesome.Xrm.Simulate.DataServices;
using FluentAssertions;
using Microsoft.Xrm.Sdk.PluginTelemetry;
using NUnit.Framework;

namespace CloudAwesome.Xrm.Simulate.Test.ServiceProvidersTests;

[TestFixture]
public class TelemetrySimulatorTests
{
    private readonly IServiceProvider _serviceProvider = null!;
    private ILogger _logger = null!;

    private readonly MockedTelemetryService _telemetry = new MockedTelemetryService();

    private const string MessageFormat = "Record created with ID {0} at {1}";
    private readonly Guid _id = Guid.NewGuid();
    private readonly DateTime _dateTime = DateTime.Now;

    [Test]
    public void Standard_Log_Is_Correctly_Saved_To_Telemetry_Store()
    {
        var service = _serviceProvider.Simulate();
        _logger = (ILogger) service.GetService(typeof(ILogger))!;
        _telemetry.Clear();
        
        _logger.Log(LogLevel.Information, 
            MessageFormat, _id, _dateTime);

        var logs = _telemetry.Get(LogLevel.Information);

        logs.Count.Should().Be(1);
        logs[0].LogLevel.Should().Be(LogLevel.Information);
        logs[0].MessageFormat.Should().Be(MessageFormat);
        logs[0].Args.Length.Should().Be(2);
        logs[0].FormattedMessage.Should().Be($"Record created with ID {_id} at {_dateTime}");
    }
    
    [Test]
    public void LogCritical_Is_Correctly_Saved_To_Telemetry_Store()
    {
        var service = _serviceProvider.Simulate();
        _logger = (ILogger) service.GetService(typeof(ILogger))!;
        _telemetry.Clear();
        
        _logger.LogCritical(
            MessageFormat, _id, _dateTime);

        var logs = _telemetry.Get(LogLevel.Critical);
        
        logs.Count.Should().Be(1);
        logs[0].LogLevel.Should().Be(LogLevel.Critical);
        logs[0].MessageFormat.Should().Be(MessageFormat);
        logs[0].Args.Length.Should().Be(2);
        logs[0].FormattedMessage.Should().Be($"Record created with ID {_id} at {_dateTime}");
    }
    
    [Test]
    public void LogError_Is_Correctly_Saved_To_Telemetry_Store()
    {
        var service = _serviceProvider.Simulate();
        _logger = (ILogger) service.GetService(typeof(ILogger))!;
        _telemetry.Clear();
        
        _logger.LogError(
            MessageFormat, _id, _dateTime);

        var logs = _telemetry.Get(LogLevel.Error);
        logs.Count.Should().Be(1);
        logs[0].LogLevel.Should().Be(LogLevel.Error);
    }
    
    [Test]
    public void LogDebug_Is_Correctly_Saved_To_Telemetry_Store()
    {
        var service = _serviceProvider.Simulate();
        _logger = (ILogger) service.GetService(typeof(ILogger))!;
        _telemetry.Clear();
        
        _logger.LogDebug(
            MessageFormat, _id, _dateTime);

        var logs = _telemetry.Get(LogLevel.Debug);
        logs.Count.Should().Be(1);
        logs[0].LogLevel.Should().Be(LogLevel.Debug);
    }
    
    [Test]
    public void LogInformation_Is_Correctly_Saved_To_Telemetry_Store()
    {
        var service = _serviceProvider.Simulate();
        _logger = (ILogger) service.GetService(typeof(ILogger))!;
        _telemetry.Clear();
        
        _logger.LogInformation(
            MessageFormat, _id, _dateTime);

        var logs = _telemetry.Get(LogLevel.Information);
        logs.Count.Should().Be(1);
        logs[0].LogLevel.Should().Be(LogLevel.Information);
    }
    
    [Test]
    public void LogWarning_Is_Correctly_Saved_To_Telemetry_Store()
    {
        var service = _serviceProvider.Simulate();
        _logger = (ILogger) service.GetService(typeof(ILogger))!;
        _telemetry.Clear();
        
        _logger.LogWarning(
            MessageFormat, _id, _dateTime);

        var logs = _telemetry.Get(LogLevel.Warning);
        logs.Count.Should().Be(1);
        logs[0].LogLevel.Should().Be(LogLevel.Warning);
    }
}