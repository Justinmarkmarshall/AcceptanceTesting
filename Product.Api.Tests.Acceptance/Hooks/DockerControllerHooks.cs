using System.Net;
using BoDi;
using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Services;
using Microsoft.Extensions.Configuration;
using TechTalk.SpecFlow;
//use hooks to wire up, fire up and tear down Docker
namespace Product.Api.Tests.Acceptance.Hooks;

[Binding]
public class DockerControllerHooks
{
    //use fluent docker to setup docker
    private static ICompositeService _compositeService = default!;
    //DI container in SF
    private IObjectContainer _objectContainer;

    public DockerControllerHooks(IObjectContainer objectContainer)
    {
        _objectContainer = objectContainer;
    }
    
    [BeforeTestRun]
    public static void DockerComposeUp()
    {
        //get the config 
        var config = LoadConfiguration();
        var dockerComposeFileName = config["DockerComposeFileName"];
        var dockerComposePath = GetDockerComposeLocation(dockerComposeFileName ?? string.Empty);
        
        //make a composite server out of the file
        var confirmationUrl = config["Product.Api:BaseAddress"];
        _compositeService = new Builder()
            .UseContainer()
            .UseCompose()
            .FromFile(dockerComposePath)
            .RemoveOrphans() //make sure that the docker container is up and responding with OK
            .WaitForHttp("webapi", $"{confirmationUrl}/products",
                continuation: (response, _) => response.Code != HttpStatusCode.OK ? 2000 : 0)
            .Build().Start();
    }

    [AfterTestRun]
    public static void DockerComposeDown()
    {
        //tear down, stop and delete
        _compositeService.Stop();
        _compositeService.Dispose();
    }
    
    //register HttpClient in the objectContainer so that before every scenario we have a clitn
    [BeforeScenario()]
    public void AddHttpClient()
    {
        var config = LoadConfiguration();
        var httpClient = new HttpClient
        {
            BaseAddress = new Uri(config["Product.Api:BaseAddress"])
        };
        _objectContainer.RegisterInstanceAs(httpClient);
    }
    
    //other more granular controls are available here for example
    //[AfterFeature]
    //[AfterScenario]

    
    //2 Utility methods
    //one loads the settings
    private static IConfiguration LoadConfiguration()
    {
        return new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
    }

    //second locates docker file based on the same specified in the settings
    private static string GetDockerComposeLocation(string dockerComposeFileName)
    {
        //directory comes as C drive
        var directory = Directory.GetCurrentDirectory();
        while (!Directory.EnumerateFiles(directory, "*.yml").Any(s => s.EndsWith(dockerComposeFileName)))
        {
            directory = directory.Substring(0, directory.LastIndexOf((Path.DirectorySeparatorChar)));
        }
        return Path.Combine(directory, dockerComposeFileName);
    }
}