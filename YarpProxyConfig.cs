using System.Collections.ObjectModel;
using Microsoft.Extensions.Primitives;
using Yarp.ReverseProxy.Configuration;

namespace DidItAgain.Proxy;

public class YarpProxyConfig : IProxyConfig
{
    private const string CLUSTER_ID = "SampleCluster";
    private const string DESTINATION_SERVICE_NAME = "SampleEndpoint";
    private const string ROUTE_ID = "SampleRoute";

    private readonly CancellationTokenSource _cts = new();
    private readonly CancellationChangeToken _changeToken;

    public YarpProxyConfig()
    {
        _cts = new CancellationTokenSource();
        _changeToken = new CancellationChangeToken(_cts.Token);
    }

    public IReadOnlyList<ClusterConfig> Clusters =>
    [
        new()
            {
                ClusterId = CLUSTER_ID,
                Destinations = new Dictionary<string, DestinationConfig>(StringComparer.OrdinalIgnoreCase)
                {
                    { DESTINATION_SERVICE_NAME, new DestinationConfig { Address = "https://httpstat.us" } }
                }
            }
    ];

    public IReadOnlyList<RouteConfig> Routes =>
        [
            new()
            {
                RouteId = ROUTE_ID,
                ClusterId = CLUSTER_ID,
                Match = new RouteMatch
                {
                    Path = "/api/sample/{**catchall}"
                },
                Transforms = new List<ReadOnlyDictionary<string, string>>()
                {
                    new(new Dictionary<string, string>{ { "PathSet", "/200" }})
                }
            }
        ];

    public IChangeToken ChangeToken => _changeToken;
}
