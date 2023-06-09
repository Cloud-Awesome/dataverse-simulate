# dataverse-simulate
Mock framework to simulate Dataverse (Power Platform/Dynamics 365 CE) environments for unit testing

## Basic Usage

### IOrganizationService

`Xrm.Simulate` exposes two extension methods to the IOrganizationService which mocks standard org service functionality and messages

```csharp
private readonly IOrganizationService _organizationService = null!;

// Use .Simulate() to mock the IOrganizationService 
// Use .Data() to access the mocked (in memory) data store

[Test]
public void Create_Contact_Saves_Record_To_Data_Store()
{
    var orgService = _organizationService.Simulate();
    orgService.Data().Reinitialise();
    
    var contactId = orgService.Create(Arthur.Contact());
    contactId.Should().NotBeEmpty();

    var contacts = orgService.Data().Get(Arthur.Contact().LogicalName);
    contacts.Count.Should().Be(1);

    contacts.FirstOrDefault()?.Id.Should().Be(contactId);
}
```

Options can be injected to the mocked service to facilitate unit tests

```csharp
// Use ISimulatorOptions to inject 
var mockSystemTime = new MockSystemTime(new DateTime(2020, 8, 16));

var options = new SimulatorOptions
{
    ClockSimulator = mockSystemTime,
    AuthenticatedUser = new Entity("systemuser", Guid.NewGuid())
    {
        Attributes =
        {
            ["fullname"] = "Lynda Archer"
        }
    }
};

var orgService = _organizationService.Simulate(options);

```

### Plugins

In progress ...