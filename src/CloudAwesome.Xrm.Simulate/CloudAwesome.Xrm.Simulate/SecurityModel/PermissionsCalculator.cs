using System.Reflection;
using CloudAwesome.Xrm.Simulate.Interfaces;
using Microsoft.Xrm.Sdk;

namespace CloudAwesome.Xrm.Simulate.SecurityModel;

public static class PermissionsCalculator
{
	/// <summary>
	/// Validates whether the defined security model has greater than 0 permissions for the defined message and entity
	/// Doesn't validate for the specific record but is intended to be used rather as a guard clause
	/// </summary>
	/// <param name="entityLogicalName">Entity logical name (e.g. contact)</param>
	/// <param name="message">SDK message to validate permissions against, e.g. Create, Assign, Delete</param>
	/// <param name="options">Simulator options potentially containing simulated security model</param>
	/// <returns>True if not blocked by security model; False if entity permission is set to none</returns>
	/// <exception cref="ArgumentException"></exception>
	public static bool ValidateEntityPermission(string entityLogicalName, string message, ISimulatorOptions? options)
	{
		// No security model define, just return true
		if (options?.SimulatedSecurityModel is null) return true;

		// Security model defined, is entity included
		var entityPermissions =
			options.SimulatedSecurityModel.EntityPermissions.SingleOrDefault(x => x.LogicalName == entityLogicalName);
		
		if (entityPermissions is null)
		{
			// Return whether configured to ignore this entity or not 
			return options.SimulatedSecurityModel.IgnoreMissingEntities;
		}

		var propertyInfo = typeof(EntityPermission).GetProperty(message, BindingFlags.Public | BindingFlags.Instance);
		if (propertyInfo == null || propertyInfo.PropertyType != typeof(PrivilegeDepthEnum))
		{
			throw new ArgumentException($"The message to validate is not recognised: {message}");
		}
		
		var permission = (PrivilegeDepthEnum) (propertyInfo.GetValue(entityPermissions) ?? PrivilegeDepthEnum.None);

		// If permission is none, return false
		if (permission == PrivilegeDepthEnum.None) return false;
		
		// Otherwise, return true
		return true;
	}

	public static bool ValidateEntityPermission(string entityLogicalName, string message, ISimulatorOptions? options,
		Entity entity)
	{
		// Handles create, retrieve, delete, assign, etc...
		return true;
	}

	public static bool ValidateEntityPermission(string entityLogicalName, string message, ISimulatorOptions? options,
		List<Entity> entities)
	{
		// Handles retrieve multiple (and later will handle other xMultiple messages)
		return true;
	}
}