using System;
using System.Linq;
using CloudAwesome.Xrm.Simulate.ServiceProviders;
using CloudAwesome.Xrm.Simulate.Test.DocumentationCode.DependentCodeUnderTest;
using FluentAssertions;
using Microsoft.Xrm.Sdk;
using NUnit.Framework;

namespace CloudAwesome.Xrm.Simulate.Test.DocumentationCode;

[TestFixture]
public class QuickStartServiceProviderTests
{
	// Reference a null IServiceProvider which can be consumed by all unit tests.
	// This enables the fluent simulation API, it doesn't hold any functionality itself.
	// It could be included in a global class if preferred as 
	//    all the functionality is created from the `.Simulate()' method.
	private IServiceProvider _serviceProvider = null!;

	[Test]
	public void Follow_Up_Plugin_Creates_Activity_Record() 
	{
		// Simulate the service provider
		_serviceProvider = _serviceProvider.Simulate();
    
		// Create a reference to the plugin you want to test
		// If your plugin has a constructor, it can be used as normal
		var sut = new FollowUpPlugin();
    
		// The `IPlugin` interface enforces a `Execute(IServiceProvider serviceProvider)` method
		// Feed the mocked service provider and execute
		// All the other services you initialise in the plugin code will be mocked too 
		sut.Execute(_serviceProvider);
    
		// The `.Simulated()` extension exposes four mocked data services
		//		(Data, Logs, Telemetry, and Audits) to query outputs
		
		// Get all tasks from the database
		var tasks = _serviceProvider.Simulated().Data().Get("task");
		
		// Get all plugin traces created during this test
		var traces = _serviceProvider.Simulated().Logs().Get();
    
		// Then run your test assertions as usual
		tasks.Count.Should().Be(1);
		tasks.SingleOrDefault()!.Attributes["subject"].Should().Be("Follow up on your call");
    
		traces.Count.Should().Be(1);
	}
	
	[Test]
	public void Follow_Up_Plugin_Creates_Activity_Record_Respecting_Simulator_Options()
	{
		var userId = Guid.NewGuid();
		var targetAccountId = Guid.NewGuid();
		
		// Create a new `SimulatorOptions` instance. This can include
		//		Mocking SystemTime, authenticated user, required test data
		// View documentation for more options!
		var options = new SimulatorOptions
		{
			// Set the current authenticated user. 
			AuthenticatedUser = new Entity("systemuser", userId)
			{
				Attributes =
				{
					["fullname"] = "Bruce Purves"
				}
			},
        
			// Set the plugin execution context, including the target entity of the triggered plugin
			// All other members of the `IPluginExecutionContext` such as registered message, 
			//    entity images, and stage can be set in here
			PluginExecutionContextMock = new PluginExecutionContextMock
			{
				InputParameters = [ new("Target", new Entity("account", targetAccountId)) ],
				PrimaryEntityName = "account"
			}
		};

		// Pass the options when you simulate the service provider
		_serviceProvider = _serviceProvider.Simulate(options);
    
		// Create a reference to the plugin you want to test
		var sut = new FollowUpPlugin();
		sut.Execute(_serviceProvider);
    
		// The `.Simulated` extension exposes 4 mocked data services to query outputs
		var tasks = _serviceProvider.Simulated().Data().Get("task");
		var traces = _serviceProvider.Simulated().Logs().Get();
    
		// Then run your test assertions as usual
		tasks.Count.Should().Be(1);
		var followUpTask = tasks.SingleOrDefault()!;
    
		// Plugin has processed the injected target Account
		followUpTask.Attributes["regardingid"].Should().BeEquivalentTo(new EntityReference
		{
			Id = targetAccountId,
			LogicalName = "account"
		});
    
		// Plugin has executed under the injected user
		followUpTask.Attributes["createdby"].Should().BeEquivalentTo(new EntityReference
		{
			Id = userId,
			LogicalName = "systemuser"
		});
		
		// And plugin traces should be logged as before
		traces.Count.Should().Be(1);
		traces.SingleOrDefault()!.Should().Be("FollowUpPlugin: Successfully created the task activity.");
	}
}