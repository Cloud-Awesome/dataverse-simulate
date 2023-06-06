using System;
using System.Linq;
using FluentAssertions;
using Microsoft.Xrm.Sdk;
using NUnit.Framework;

namespace CloudAwesome.Xrm.Simulate.Test;

[TestFixture]
public class OrganisationServiceSimulatorTests
{
    private IOrganizationService _organizationService;
    
    [Test]
    public void Simulate_Org_Service_Returns_Mocked_Instance()
    {
        var orgService = _organizationService.Simulate();
        orgService.Should().NotBeNull();
    }

    [Test]
    public void Simulate_Org_Service_Should_Initiate_Data_Store()
    {
        var orgService = _organizationService.Simulate();
        orgService.Data().Should().NotBeNull();
    }

    [Test]
    public void Initialising_With_An_Authenticated_User_Should_Store_Reference()
    {
        var options = new SimulatorOptions
        {
            AuthenticatedUser = new Entity("systemuser", Guid.NewGuid())
            {
                Attributes =
                {
                    ["fullname"] = "Gemma Armstrong"
                }
            }
        };

        var orgService = _organizationService.Simulate(options);

        orgService.Data().AuthenticatedUser.Should().NotBeNull();
        orgService.Data().AuthenticatedUser.Id.Should().Be(options.AuthenticatedUser.Id);
    }

    [Test]
    public void Initialising_With_An_Authenticated_User_Should_Add_To_User_Data_Store()
    {
        var options = new SimulatorOptions
        {
            AuthenticatedUser = new Entity("systemuser", Guid.NewGuid())
            {
                Attributes =
                {
                    ["fullname"] = "Gemma Armstrong"
                }
            }
        };

        var orgService = _organizationService.Simulate(options);

        var users = orgService.Data().Get("systemuser");
        users.Count.Should().Be(1);
        users.FirstOrDefault().Attributes["fullname"].Should().Be("Gemma Armstrong");
    }
}