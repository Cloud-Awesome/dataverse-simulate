using System;
using System.Collections.Generic;
using CloudAwesome.Xrm.Simulate.Interfaces;
using CloudAwesome.Xrm.Simulate.SecurityModel;
using FluentAssertions;
using NUnit.Framework;

namespace CloudAwesome.Xrm.Simulate.Test.SecurityModelTests;

[TestFixture]
public class PermissionsCalculatorTests
{
	[Test]
	public void ValidateEntityPermission_Returns_True_If_No_Model_Is_Defined()
	{
		var options = new SimulatorOptions();
		var result = PermissionsCalculator.ValidateEntityPermission("contact", "Create", options);

		result.Should().Be(true);
	}
	
	[Test]
	public void ValidateEntityPermission_Returns_True_If_Entity_Not_Defined_But_Ignored()
	{
		var options = new SimulatorOptions
		{
			SimulatedSecurityModel = new SimulatedSecurityModel
			{
				IgnoreMissingEntities = true
			}
		};
		var result = PermissionsCalculator.ValidateEntityPermission("contact", "Create", options);

		result.Should().Be(true);
	}
	
	[Test]
	public void ValidateEntityPermission_Returns_False_If_Entity_Not_Defined_And_Not_Ignored()
	{
		var options = new SimulatorOptions
		{
			SimulatedSecurityModel = new SimulatedSecurityModel
			{
				IgnoreMissingEntities = false
			}
		};
		var result = PermissionsCalculator.ValidateEntityPermission("contact", "Create", options);

		result.Should().Be(false);
	}
	
	[Test]
	[TestCase(PrivilegeDepthEnum.User)]
	[TestCase(PrivilegeDepthEnum.BusinessUnit)]
	[TestCase(PrivilegeDepthEnum.ParentChild)]
	[TestCase(PrivilegeDepthEnum.Organization)]
	public void ValidateEntityPermission_Returns_True_If_Entity_Is_Defined_And_Has_Privilege(PrivilegeDepthEnum depth)
	{
		var options = new SimulatorOptions
		{
			SimulatedSecurityModel = new SimulatedSecurityModel
			{
				EntityPermissions =
				[
					new EntityPermission
					{
						LogicalName = "contact",
						Create = depth
					}
				]
			}
		};
		var result = PermissionsCalculator.ValidateEntityPermission("contact", "Create", options);

		result.Should().Be(true);
	}
	
	[Test]
	public void ValidateEntityPermission_Returns_False_If_Entity_Is_Defined_But_Depth_Is_None()
	{
		var options = new SimulatorOptions
		{
			SimulatedSecurityModel = new SimulatedSecurityModel
			{
				EntityPermissions =
				[
					new EntityPermission
					{
						LogicalName = "contact",
						Create = PrivilegeDepthEnum.None
					}
				]
			}
		};
		var result = PermissionsCalculator.ValidateEntityPermission("contact", "Create", options);

		result.Should().Be(false);
	}
	
	[Test]
	public void ValidateEntityPermission_Throws_Exception_If_Message_Not_Recognised()
	{
		var options = new SimulatorOptions
		{
			SimulatedSecurityModel = new SimulatedSecurityModel
			{
				EntityPermissions =
				[
					new EntityPermission
					{
						LogicalName = "contact",
						Create = PrivilegeDepthEnum.None
					}
				]
			}
		};
		var sut = () => PermissionsCalculator.ValidateEntityPermission("contact", "Dummy", options);

		sut.Should()
			.Throw<ArgumentException>()
			.WithMessage("The message to validate is not recognised: Dummy");
	}
}