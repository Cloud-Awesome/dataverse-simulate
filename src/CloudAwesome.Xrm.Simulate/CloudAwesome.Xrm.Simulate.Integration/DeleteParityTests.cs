using System;
using System.ServiceModel;
using FluentAssertions;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using NUnit.Framework;

namespace CloudAwesome.Xrm.Simulate.Gather;

[TestFixture]
[Category("ParitySmoke")]
public sealed class DeleteParityTests : IntegrationBaseFixture
{
    private const string AccountLogicalName = "account";

    [Test]
    public void Delete_Missing_Record_Returns_Dataverse_Not_Found_Fault()
    {
        var missingId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        var service = DataverseConnectionManager.Instance.GetConnection();

        var fault = CaptureDeleteFault(service, AccountLogicalName, missingId);

        fault.ExceptionType.Should().Be(typeof(FaultException<OrganizationServiceFault>).FullName);
        fault.Message.Should().Be("Entity 'Account' With Id = aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa Does Not Exist");
        fault.FaultType.Should().Be(typeof(OrganizationServiceFault).FullName);
        fault.FaultMessage.Should().Be("Entity 'Account' With Id = aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa Does Not Exist");
        fault.ErrorCode.Should().Be(-2147220969);
    }

    [Test]
    public void DeleteRequest_Missing_Record_Returns_Dataverse_Not_Found_Fault()
    {
        var missingId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        var service = DataverseConnectionManager.Instance.GetConnection();

        var fault = CaptureDeleteRequestFault(service, AccountLogicalName, missingId);

        fault.ExceptionType.Should().Be(typeof(FaultException<OrganizationServiceFault>).FullName);
        fault.Message.Should().Be("Entity 'Account' With Id = aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa Does Not Exist");
        fault.FaultType.Should().Be(typeof(OrganizationServiceFault).FullName);
        fault.FaultMessage.Should().Be("Entity 'Account' With Id = aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa Does Not Exist");
        fault.ErrorCode.Should().Be(-2147220969);
    }

    private static DataverseFaultSnapshot CaptureDeleteFault(
        IOrganizationService service,
        string logicalName,
        Guid id)
    {
        try
        {
            service.Delete(logicalName, id);
        }
        catch (Exception exception)
        {
            return DataverseFaultSnapshot.From(exception);
        }

        throw new AssertionException("Delete unexpectedly succeeded.");
    }

    private static DataverseFaultSnapshot CaptureDeleteRequestFault(
        IOrganizationService service,
        string logicalName,
        Guid id)
    {
        try
        {
            service.Execute(new DeleteRequest
            {
                Target = new EntityReference(logicalName, id)
            });
        }
        catch (Exception exception)
        {
            return DataverseFaultSnapshot.From(exception);
        }

        throw new AssertionException("DeleteRequest unexpectedly succeeded.");
    }

    private sealed record DataverseFaultSnapshot(
        string ExceptionType,
        string Message,
        string? FaultType,
        int? ErrorCode,
        string? FaultMessage)
    {
        public static DataverseFaultSnapshot From(Exception exception)
        {
            if (exception is FaultException<OrganizationServiceFault> faultException)
            {
                return new DataverseFaultSnapshot(
                    exception.GetType().FullName!,
                    exception.Message,
                    faultException.Detail.GetType().FullName,
                    faultException.Detail.ErrorCode,
                    faultException.Detail.Message);
            }

            return new DataverseFaultSnapshot(
                exception.GetType().FullName!,
                exception.Message,
                null,
                null,
                null);
        }
    }
}
