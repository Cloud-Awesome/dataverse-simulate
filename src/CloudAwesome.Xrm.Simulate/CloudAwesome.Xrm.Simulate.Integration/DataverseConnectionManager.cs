using System;
using Microsoft.Extensions.Configuration;
using Microsoft.PowerPlatform.Dataverse.Client;

namespace CloudAwesome.Xrm.Simulate.Gather;

public class DataverseConnectionManager
{
    private IConfiguration _configuration;
    
    private static DataverseConnectionManager? _instance;

    private ServiceClient? _serviceClient;
    
    private static object syncLock = new object();

    public static DataverseConnectionManager Instance
    {
        get
        {
            lock (syncLock)
            {
                return _instance ??= new DataverseConnectionManager();
            }
        }
    }

    public ServiceClient GetConnection()
    {
        if (_serviceClient != null) return _serviceClient;
        
        var path = 
            Environment.GetEnvironmentVariable("DATAVERSE_APPSETTINGS") ?? "appsettings.local.json";

        _configuration = new ConfigurationBuilder()
            .AddJsonFile(path, optional: false, reloadOnChange: true)
            .Build();
        
        _serviceClient = new ServiceClient(_configuration.GetConnectionString("default"));

        return _serviceClient;
    }
    
    public void DisposeConnection()
    {
        if (_serviceClient != null)
        {
            _serviceClient.Dispose();
            _serviceClient = null;
        }
    }
}