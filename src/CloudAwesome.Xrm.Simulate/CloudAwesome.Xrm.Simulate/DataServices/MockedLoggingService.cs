using CloudAwesome.Xrm.Simulate.DataStores;

namespace CloudAwesome.Xrm.Simulate.DataServices;

public class MockedLoggingService
{
    /// <summary>
    /// Adds a new log message to the in memory logging store
    /// </summary>
    /// <param name="message"></param>
    public void Add(string message)
    {
        MockedLoggingStore.Instance.Logs.Add(message);
    }

    public void Add(string message, params object[] args)
    {
        MockedLoggingStore.Instance.Logs.Add(string.Format(message, args));
    }

    /// <summary>
    /// Clears all logs from the in memory store.
    /// Call this during test set up if the test requires a fresh run. 
    /// </summary>
    public void Clear()
    {
        MockedLoggingStore.Instance.Logs.Clear();
    }
    
    /// <summary>
    /// Returns all logs saved to the in-memory store.
    /// </summary>
    /// <returns></returns>
    public List<string> Get()
    {
        return MockedLoggingStore.Instance.Logs;
    }
    
    /// <summary>
    /// Returns a list of logs matching an exact input string.
    /// </summary>
    /// <param name="containing">String to search for in the logs</param>
    /// <returns>List of string logs</returns>
    public List<string> Get(string containing)
    {
        var logs = MockedLoggingStore.Instance.Logs
            .Where(x => x.Contains(containing))
            .ToList();
        
        return logs;
    }
}