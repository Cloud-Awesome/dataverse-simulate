using System;
using System.Linq;
using CloudAwesome.Xrm.Simulate.Test.EarlyBoundEntities;
using CloudAwesome.Xrm.Simulate.Test.TestEntities;
using FluentAssertions;
using Microsoft.Xrm.Sdk;
using NUnit.Framework;

namespace CloudAwesome.Xrm.Simulate.Test.DocumentationCode;

[TestFixture]
public class QuickStartOrgServiceTests
{
	// Reference a null IOrganisationService which can be consumed by all unit tests.
	// This enables the fluent simulation API, it doesn't hold any functionality itself.
	// It could be included in a global class if preferred as 
	//    all the functionality is created from the `.Simulate()' method.
	private IOrganizationService _organizationService = null!;
	
	[Test]
	public void Create_Contact_Saves_Record_To_Data_Store()
	{
		// Create a mock of the org service
		// Each call to `.Simulate` creates a fresh new mock
		_organizationService = _organizationService.Simulate();
    
		// Thereafter you can use any SDK methods on the org service as usual,
		//    or inject it into your code under test (sut)
		var contactId = _organizationService.Create(Arthur.Contact());
    
		// And use any testing frameworks you like to run assertions
		// (Here I'm using FluentAssertions, which I love and highly recommend!)
		contactId.Should().NotBeEmpty();

		// Instead of executing a query against the org service, 
		//    you can retrieve, query and interact the in memory data
		//    directly using the `.Simulated` extension
		var contacts = _organizationService.Simulated().Data().Get("contact");
    
		// And run your assertions on that data
		contacts.Count.Should().Be(1);
		contacts.SingleOrDefault()?.Id.Should().Be(contactId);
	}

	[Test]
	public void Quick_Start_IOrganizationService_With_Simulator_Options()
	{
		var userId = Guid.NewGuid();
		var systemTime = new DateTime(2020, 8, 16);

		// Create a new `SimulatorOptions` instance. This can include
		//		Mocking SystemTime, authenticated user, required test data
		// View documentation for more options!
		var options = new SimulatorOptions
		{
			// Inject a mocked system time to allow for date-based tests and assertions
			// (You can use the provided MockSystemTime, or create your own implementation 
			//		of `IClockSimulator` if you need any additional functionality)
			ClockSimulator = new MockSystemTime(systemTime),
    
			// Set the current authenticated user. 
			// This can use early or late-bound entities (i.e. `Entity` or `SystemUser`) 
			AuthenticatedUser = new Entity("systemuser", userId)
			{
				Attributes =
				{
					["fullname"] = "Lynda Archer"
				}
			}
		};

		// Pass the options when you simulate the org service
		_organizationService = _organizationService.Simulate(options);

		// Thereafter you can use org service methods as usual,
		//    or inject it into your code under test (sut)
		var contactId = _organizationService.Create(Arthur.Contact());
		
		// As before, you can quickly retrieve data from the in-memory store instead of
		//		executing retrieve requests, using the `.Simulated().Data()` methods.
		var contacts = _organizationService.Simulated().Data().Get("contact");
    
		// And assert on configuration as expected
		contacts.Count.Should().Be(1);
		var createdContact = (Contact) contacts.SingleOrDefault()!;

		// Code is executed under the authenticated user
		createdContact.CreatedBy.Id.Should().Be(userId);
		createdContact.ModifiedBy.Id.Should().Be(userId);

		// Code is executed using the injected system time
		createdContact.CreatedOn.Should().Be(systemTime);
		createdContact.ModifiedOn.Should().Be(systemTime);
	}
}