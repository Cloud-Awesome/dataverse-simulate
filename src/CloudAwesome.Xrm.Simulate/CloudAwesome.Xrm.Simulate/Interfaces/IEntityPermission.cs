using CloudAwesome.Xrm.Simulate.SecurityModel;

namespace CloudAwesome.Xrm.Simulate.Interfaces;

public interface IEntityPermission
{
	public string LogicalName { get; set; }
	
	public PrivilegeDepthEnum Create { get; set; }
	public PrivilegeDepthEnum Read { get; set; }
	public PrivilegeDepthEnum Write { get; set; }
	public PrivilegeDepthEnum Delete { get; set; }
	public PrivilegeDepthEnum Append { get; set; }
	public PrivilegeDepthEnum AppendTo { get; set; }
	public PrivilegeDepthEnum Assign { get; set; }
	public PrivilegeDepthEnum Share { get; set; }
}