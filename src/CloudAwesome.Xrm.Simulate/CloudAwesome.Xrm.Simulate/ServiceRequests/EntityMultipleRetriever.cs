using CloudAwesome.Xrm.Simulate.DataStores;
using CloudAwesome.Xrm.Simulate.Interfaces;
using CloudAwesome.Xrm.Simulate.QueryParsers;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Moq;

namespace CloudAwesome.Xrm.Simulate.ServiceRequests;

/// <summary>
/// 
/// </summary>
/// <remarks>
/// N.B. Filters and LinkEntity currently only work if you've included the attributes in the ColumnSet
/// </remarks>
public class EntityMultipleRetriever: IEntityMultipleRetriever
{
    public void MockRequest(IOrganizationService organizationService, 
        ISimulatorOptions? options = null)
    {
        // Handle QueryExpression
        Mock.Get(organizationService)
            .Setup(x => x.RetrieveMultiple(
                It.IsAny<QueryExpression>()))
            .Returns((QueryExpression query) =>
            {
                var results = QueryExpressionParser.Parse(query,
                    MockedEntityDataStore.Instance.Data);
                    
                return new EntityCollection(results.ToList());
            });
            
        // Handle FetchExpression
        Mock.Get(organizationService)
            .Setup(x => x.RetrieveMultiple(
                It.IsAny<FetchExpression>()))
            .Returns((FetchExpression query) =>
            {
                var results = FetchExpressionParser.Parse(query,
                    MockedEntityDataStore.Instance.Data);
                
                return new EntityCollection(results.ToList());
            });

        // Handle QueryByAttribute
        Mock.Get(organizationService)
            .Setup(x => x.RetrieveMultiple(
                It.IsAny<QueryByAttribute>()))
            .Returns((QueryByAttribute query) =>
            {
                var results = QueryByAttributeParser.Parse(query,
                    MockedEntityDataStore.Instance.Data);
                
                return new EntityCollection(results.ToList());
            });
    }
}