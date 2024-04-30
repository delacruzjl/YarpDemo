using DidItAgain.Proxy;
using Yarp.ReverseProxy.Configuration;

var webAppBuilder = WebApplication.CreateBuilder(args);

webAppBuilder.Services.AddTransient<IProxyConfigProvider, YarpProxyConfigProvider>();
webAppBuilder.Services.AddReverseProxy();
// .LoadFromConfig(webAppBuilder.Configuration.GetSection("Yarp"));

var app = webAppBuilder.Build();

app.MapReverseProxy();
app.MapGet("/", () => "Hello World!");

app.Run();
