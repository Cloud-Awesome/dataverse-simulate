using System;
using CloudAwesome.Xrm.Simulate.DataServices;
using FluentAssertions;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.PluginTelemetry;
using NUnit.Framework;

namespace CloudAwesome.Xrm.Simulate.Test;

[TestFixture]
public class ServiceProviderSimulatorTests
{
    private readonly IServiceProvider _serviceProvider = null!;
    
    [Test]
    public void Simulate_IServiceProvider_Returns_Mocked_Service()
    {
        var serviceProvider = _serviceProvider.Simulate();
        serviceProvider.Should().NotBeNull();
    }

    [Test]
    public void GetService_Can_Return_Mocked_IPluginExecutionContext()
    {
        var dataService = new MockedEntityDataService();
        var serviceProvider = _serviceProvider.Simulate();
        
        var executionContext = (IPluginExecutionContext) 
            serviceProvider.GetService(typeof(IPluginExecutionContext))!;

        executionContext.Should().NotBeNull();
        executionContext.UserId.Should().Be(dataService.AuthenticatedUser.Id);
    }

    [Test]
    public void GetService_Can_Return_Mocked_IOrganizationServiceFactory()
    {
        var serviceProvider = _serviceProvider.Simulate();

        var factory = (IOrganizationServiceFactory)
            serviceProvider.GetService(typeof(IOrganizationServiceFactory))!;

        factory.Should().NotBeNull();
    }

    [Test]
    public void GetService_Can_Return_Mocked_ILogger()
    {
        var serviceProvider = _serviceProvider.Simulate();
        
        var logger = (ILogger)
            serviceProvider.GetService(typeof(ILogger))!;

        logger.Should().NotBeNull();
    }

    [Test]
    public void GetService_Can_Return_Mocked_ITracingService()
    {
        var serviceProvider = _serviceProvider.Simulate();

        var tracingService = (ITracingService)
            serviceProvider.GetService(typeof(ITracingService))!;

        tracingService.Should().NotBeNull();
    }

    [Test]
    public void GetService_Requesting_Unsupported_Service_Should_Throw_Error()
    {
        var serviceProvider = _serviceProvider.Simulate();

        var sut = () => serviceProvider.GetService(typeof(string));

        sut.Should()
            .Throw<ArgumentException>()
            .WithMessage("Type of Service requested is not supported");
    }
}