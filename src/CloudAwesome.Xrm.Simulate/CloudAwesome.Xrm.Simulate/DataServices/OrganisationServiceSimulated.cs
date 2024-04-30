namespace CloudAwesome.Xrm.Simulate.DataServices;

public class OrganisationServiceSimulated
{
    private readonly MockedEntityDataService _dataService;

    public OrganisationServiceSimulated(MockedEntityDataService dataService)
    {
        _dataService = dataService;
    }
    
    public MockedEntityDataService Data()
    {
        return _dataService;
    }
}