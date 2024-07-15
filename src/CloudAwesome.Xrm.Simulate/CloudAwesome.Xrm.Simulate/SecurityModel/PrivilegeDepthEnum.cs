namespace CloudAwesome.Xrm.Simulate.SecurityModel;

public enum PrivilegeDepthEnum
{
	/// <summary>
	/// No permission for this privilege
	/// </summary>
	None = 0,
	
	/// <summary>
	/// aka Basic
	/// </summary>
	User = 1,
	
	/// <summary>
	/// aka Local
	/// </summary>
	BusinessUnit = 2,
	
	/// <summary>
	/// aka Deep
	/// </summary>
	ParentChild = 3,
	
	/// <summary>
	/// aka Global
	/// </summary>
	Organization = 4
}