using System;
using CloudAwesome.Xrm.Simulate.DataStores;
using CloudAwesome.Xrm.Simulate.ServiceProviders;
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
        var serviceProvider = _serviceProvider.Simulate();
        
        var executionContext = (IPluginExecutionContext) 
            serviceProvider.GetService(typeof(IPluginExecutionContext))!;

        executionContext.Should().NotBeNull();
        executionContext.UserId.Should().NotBeEmpty();
    }
    
    [Test]
    public void Faking_Service_Failure_With_Execution_Context_Returns_Null()
    {
        var options = new SimulatorOptions
        {
            FakeServiceFailureSettings = new FakeServiceFailureSettings
            {
                PluginExecutionContext = true
            }
        };

        var serviceProvider = _serviceProvider.Simulate(options);
        
        var executionContext = (IPluginExecutionContext)
            serviceProvider.GetService(typeof(IPluginExecutionContext))!;

        executionContext.Should().BeNull();
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
    public void Faking_Service_Failure_With_IOrganizationServiceFactory_Returns_Null()
    {
        var options = new SimulatorOptions
        {
            FakeServiceFailureSettings = new FakeServiceFailureSettings
            {
                OrganizationServiceFactory = true
            }
        };

        var serviceProvider = _serviceProvider.Simulate(options);
        
        var executionContext = (IOrganizationServiceFactory)
            serviceProvider.GetService(typeof(IOrganizationServiceFactory))!;

        executionContext.Should().BeNull();
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
    public void Faking_Service_Failure_With_ILogger_Returns_Null()
    {
        var options = new SimulatorOptions
        {
            FakeServiceFailureSettings = new FakeServiceFailureSettings
            {
                TelemetryService = true
            }
        };

        var serviceProvider = _serviceProvider.Simulate(options);
        
        var logger = (ILogger)
            serviceProvider.GetService(typeof(ILogger))!;

        logger.Should().BeNull();
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
    public void Faking_Service_Failure_With_ITracingService_Returns_Null()
    {
        var options = new SimulatorOptions
        {
            FakeServiceFailureSettings = new FakeServiceFailureSettings
            {
                TracingService = true
            }
        };

        var serviceProvider = _serviceProvider.Simulate(options);
        
        var executionContext = (ITracingService)
            serviceProvider.GetService(typeof(ITracingService))!;

        executionContext.Should().BeNull();
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

    [Test]
    public void DataService_From_Plugin_Correctly_Consumes_Organisation_Service_Requests()
    {
        var accountId = Guid.NewGuid();
        var options = new SimulatorOptions
        {
            PluginExecutionContextMock = new PluginExecutionContextMock
            {
                InputParameters = new ParameterCollection
                {
                    new ("Target", new Entity("account", accountId))
                },
                PrimaryEntityName = "account",
                OutputParameters = new ParameterCollection()
            }
        };
        
        var serviceProvider = _serviceProvider.Simulate(options);
        var tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
        var executionContext = (IPluginExecutionContext) serviceProvider.GetService(typeof(IPluginExecutionContext));
        
        var serviceFactory = 
            (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
        var service = serviceFactory.CreateOrganizationService(executionContext.UserId);
        
        Entity followup = new Entity("task");
        followup["subject"] = "Send e-mail to the new customer.";
        followup["description"] =
            "Follow up with the customer. Check if there are any new issues that need resolution.";
        followup["scheduledstart"] = DateTime.Now.AddDays(7);
        followup["scheduledend"] = DateTime.Now.AddDays(7);
        
        tracingService.Trace("FollowupPlugin: Successfully created the task activity.");
        service.Create(followup);
        
        var traces = serviceProvider.Simulated().Logs().Get();
        var tasks = serviceProvider.Simulated().Data().Get("task");
        
        traces.Count.Should().Be(1);
        tasks.Count.Should().Be(1);
    }
    
}