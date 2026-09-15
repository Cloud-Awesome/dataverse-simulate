using System;
using System.ServiceModel;
using CloudAwesome.Xrm.Simulate.Test.EarlyBoundEntities;
using FluentAssertions;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using NUnit.Framework;

namespace CloudAwesome.Xrm.Simulate.Test.ServiceRequestsTests.OrganizationRequestsTests;

[TestFixture]
public class DirectMethodEquivalentRequestTests
{
    private IOrganizationService _organizationService = null!;

    [SetUp]
    public void SetUp()
    {
        _organizationService = _organizationService.Simulate();
    }

    [Test]
    public void Execute_RetrieveRequest_Returns_Requested_Record()
    {
        var contactId = Guid.NewGuid();
        _organizationService.Simulated().Data().Add(new Contact(contactId)
        {
            FirstName = "Ada",
            LastName = "Lovelace"
        });

        var response = (RetrieveResponse)_organizationService.Execute(new RetrieveRequest
        {
            Target = new EntityReference(Contact.EntityLogicalName, contactId),
            ColumnSet = new ColumnSet(Contact.Fields.FirstName)
        });

        response.ResponseName.Should().Be("Retrieve");
        response.Entity.Id.Should().Be(contactId);
        response.Entity[Contact.Fields.FirstName].Should().Be("Ada");
        response.Entity.Attributes.Should().NotContainKey(Contact.Fields.LastName);
    }

    [Test]
    public void Execute_UpdateRequest_Persists_Incoming_Attributes()
    {
        var accountId = Guid.NewGuid();
        _organizationService.Simulated().Data().Add(new Account(accountId)
        {
            Name = "Original Account",
            AccountNumber = "A-001"
        });

        var response = (UpdateResponse)_organizationService.Execute(new UpdateRequest
        {
            Target = new Account(accountId)
            {
                Name = "Updated Account"
            }
        });

        var storedAccount = _organizationService.Simulated().Data().Get<Account>(accountId);

        response.ResponseName.Should().Be("Update");
        storedAccount.Name.Should().Be("Updated Account");
        storedAccount.AccountNumber.Should().Be("A-001");
    }

    [Test]
    public void Execute_DeleteRequest_Removes_Record_From_Data_Store()
    {
        var accountId = Guid.NewGuid();
        _organizationService.Simulated().Data().Add(new Account(accountId)
        {
            Name = "Account to delete"
        });

        var response = (DeleteResponse)_organizationService.Execute(new DeleteRequest
        {
            Target = new EntityReference(Account.EntityLogicalName, accountId)
        });

        response.ResponseName.Should().Be("Delete");
        _organizationService.Simulated().Data().Get(Account.EntityLogicalName).Should().BeEmpty();
    }

    [Test]
    public void Execute_DeleteRequest_For_Missing_Record_Throws_Dataverse_Not_Found_Fault()
    {
        var missingAccountId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");

        var deleteMissingAccount = () => _organizationService.Execute(new DeleteRequest
        {
            Target = new EntityReference(Account.EntityLogicalName, missingAccountId)
        });

        var exception = deleteMissingAccount.Should()
            .Throw<FaultException<OrganizationServiceFault>>()
            .WithMessage("Entity 'Account' With Id = aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa Does Not Exist")
            .Which;

        exception.Detail.ErrorCode.Should().Be(-2147220969);
        exception.Detail.Message.Should().Be("Entity 'Account' With Id = aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa Does Not Exist");
    }
}
