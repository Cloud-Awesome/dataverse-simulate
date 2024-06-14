using CloudAwesome.Xrm.Simulate;
using CloudAwesome.Xrm.Simulate.ServiceProviders;
using FluentAssertions;
using Microsoft.Xrm.Sdk;

namespace CloudAwesome.Xrm.SamplePlugins.Test;

[TestFixture]
public class FollowupPluginTests
{
	private readonly IServiceProvider _serviceProvider = null!;

	private readonly SimulatorOptions _options = new SimulatorOptions
	{
		PluginExecutionContextMock = new PluginExecutionContextMock
		{
			InputParameters = [new("Target", new Entity("account", Guid.NewGuid()))],
			PrimaryEntityName = "account",
			OutputParameters = [new("id", Guid.NewGuid())]
		},
		ClockSimulator = new MockSystemTime(new DateTime(2023, 06, 14, 13, 00, 00))
	};
	
	[Test]
	public void Follow_Up_Task_Is_Created()
	{
		var serviceProvider = _serviceProvider.Simulate(_options);
		
		var sut = new FollowupPlugin();
		sut.Execute(serviceProvider);
		
		var tasks = serviceProvider.Simulated().Data().Get("task");

		tasks.Count.Should().Be(1);
	}
	
	[Test]
	public void Follow_Up_Task_Has_Valid_Attributes_Set()
	{
		var serviceProvider = _serviceProvider.Simulate(_options);
		
		var sut = new FollowupPlugin();
		sut.Execute(serviceProvider);
		
		var tasks = serviceProvider.Simulated().Data().Get("task");

		tasks.Count.Should().Be(1);
		var task = tasks.SingleOrDefault()!;

		task.Attributes["subject"].Should().Be("Send e-mail to the new customer.");
		task.Attributes["description"].Should().Be("Follow up with the customer. Check if there are any new issues that need resolution.");
		task.Attributes["scheduledstart"].Should().NotBeNull();
		task.Attributes["scheduledend"].Should().NotBeNull();
		task.Attributes["createdon"].Should().Be(new DateTime(2023, 06, 14, 13, 00, 00));
		task.Attributes["modifiedon"].Should().Be(new DateTime(2023, 06, 14, 13, 00, 00));
		task.Attributes["category"].Should().Be("account");
		
	}

	[Test]
	public void If_Target_Is_Not_An_Account_Plugin_Gracefully_Exists()
	{
		var options = new SimulatorOptions
		{
			PluginExecutionContextMock = new PluginExecutionContextMock
			{
				InputParameters = [new("Target", new Entity("contact", Guid.NewGuid()))],
				PrimaryEntityName = "account",
				OutputParameters = [new("id", Guid.NewGuid())]
			}
		};
		var serviceProvider = _serviceProvider.Simulate(options);
		
		var sut = new FollowupPlugin();
		sut.Execute(serviceProvider);
		
		var tasks = serviceProvider.Simulated().Data().Get("task");

		tasks.Count.Should().Be(0);
	}
	
	[Test]
	public void Plugin_Traces_Are_Correctly_Logged()
	{
		var serviceProvider = _serviceProvider.Simulate(_options);
		
		var sut = new FollowupPlugin();
		sut.Execute(serviceProvider);

		var logs = serviceProvider.Simulated().Logs().Get();

		logs.Count.Should().Be(1);
		logs.SingleOrDefault()!.Should().Be("FollowupPlugin: Successfully created the task activity.");
	}
	
	
	// TODO - Task is related to the Account
	// TODO - OrganizationServiceFault is handled correctly
	// TODO - Generic Exception is handled correctly
}