using CloudAwesome.Xrm.Simulate.Interfaces;

namespace CloudAwesome.Xrm.Simulate.SecurityModel;

public class SimulatedSecurityModel: ISecurityModel
{
	public bool IgnoreMissingEntities { get; set; } = true;
	public List<IEntityPermission> EntityPermissions { get; set; } = new List<IEntityPermission>();
}