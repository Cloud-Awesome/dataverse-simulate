using System;
using System.ServiceModel;
using CloudAwesome.Xrm.Simulate.Test.EarlyBoundEntities;
using FluentAssertions;
using Microsoft.Xrm.Sdk;
using NUnit.Framework;

namespace CloudAwesome.Xrm.Simulate.Test.BasicServiceRequestTests;

[TestFixture]
public class DeleteTests
{
    private IOrganizationService _organizationService = null!;

    [SetUp]
    public void SetUp()
    {
        _organizationService = _organizationService.Simulate();
    }

    [Test]
    public void Delete_Existing_Record_Removes_Record_From_Data_Store()
    {
        var accountId = Guid.NewGuid();
        _organizationService.Simulated().Data().Add(new Account(accountId)
        {
            Name = "Account to delete"
        });

        _organizationService.Delete(Account.EntityLogicalName, accountId);

        _organizationService.Simulated()
            .Data()
            .Get(Account.EntityLogicalName)
            .Should()
            .BeEmpty();
    }

    [Test]
    public void Delete_Missing_Record_Throws_Dataverse_Not_Found_Fault()
    {
        var existingAccountId = Guid.NewGuid();
        var missingAccountId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        _organizationService.Simulated().Data().Add(new Account(existingAccountId)
        {
            Name = "Existing Account"
        });

        var deleteMissingAccount = () =>
            _organizationService.Delete(Account.EntityLogicalName, missingAccountId);

        var exception = deleteMissingAccount.Should()
            .Throw<FaultException<OrganizationServiceFault>>()
            .WithMessage("Entity 'Account' With Id = aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa Does Not Exist")
            .Which;

        exception.Detail.ErrorCode.Should().Be(-2147220969);
        exception.Detail.Message.Should().Be("Entity 'Account' With Id = aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa Does Not Exist");
    }

    [Test]
    public void Delete_Missing_Record_When_Store_Has_No_Records_Throws_Dataverse_Not_Found_Fault()
    {
        var missingAccountId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");

        var deleteMissingAccount = () =>
            _organizationService.Delete(Account.EntityLogicalName, missingAccountId);

        var exception = deleteMissingAccount.Should()
            .Throw<FaultException<OrganizationServiceFault>>()
            .WithMessage("Entity 'Account' With Id = aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa Does Not Exist")
            .Which;

        exception.Detail.ErrorCode.Should().Be(-2147220969);
        exception.Detail.Message.Should().Be("Entity 'Account' With Id = aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa Does Not Exist");
    }
}
