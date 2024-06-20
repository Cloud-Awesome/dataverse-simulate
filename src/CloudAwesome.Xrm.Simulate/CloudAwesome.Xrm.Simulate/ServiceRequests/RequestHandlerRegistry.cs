using CloudAwesome.Xrm.Simulate.Interfaces;
using Microsoft.Xrm.Sdk;

namespace CloudAwesome.Xrm.Simulate.ServiceRequests;

public class RequestHandlerRegistry
{
	private readonly Dictionary<Type, IRequestHandler> _handlers = new();

	public void RegisterHandler<TRequest>(IRequestHandler handler) where TRequest : OrganizationRequest
	{
		_handlers[typeof(TRequest)] = handler;
	}

	public IRequestHandler GetHandler(OrganizationRequest request)
	{
		return _handlers[request.GetType()];
	}
}