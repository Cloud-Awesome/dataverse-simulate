using CloudAwesome.Xrm.Simulate.Interfaces;

namespace CloudAwesome.Xrm.Simulate.SecurityModel;

public class EntityPermission: IEntityPermission
{
	public string LogicalName { get; set; }
	
	public PrivilegeDepthEnum Create { get; set; } = PrivilegeDepthEnum.None;
	public PrivilegeDepthEnum Read { get; set; } = PrivilegeDepthEnum.None;
	public PrivilegeDepthEnum Write { get; set; } = PrivilegeDepthEnum.None;
	public PrivilegeDepthEnum Delete { get; set; } = PrivilegeDepthEnum.None;
	public PrivilegeDepthEnum Append { get; set; } = PrivilegeDepthEnum.None;
	public PrivilegeDepthEnum AppendTo { get; set; } = PrivilegeDepthEnum.None;
	public PrivilegeDepthEnum Assign { get; set; } = PrivilegeDepthEnum.None;
	public PrivilegeDepthEnum Share { get; set; } = PrivilegeDepthEnum.None;
}