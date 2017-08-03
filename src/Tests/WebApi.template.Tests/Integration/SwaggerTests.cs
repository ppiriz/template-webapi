using System.Linq;
using System.Threading.Tasks;
using DevOpsFlex.Tests.Core;
using Xunit;
using FluentAssertions;
using Newtonsoft.Json;

// ReSharper disable once CheckNamespace
public class SwaggerTests
{
    private const string TargetUrl = "/swagger/v1.0/swagger.json";

    [Fact, IsIntegration]
    public async Task Verify()
    {
        string swaggerResult;
        using (var client = new System.Net.WebClient())
        {
            swaggerResult = await client.DownloadStringTaskAsync("http://localhost:63300" + TargetUrl);
        }


        swaggerResult.Should().NotBeNull();

        var obj = JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(swaggerResult);
        var paths = obj["paths"];
        Assert.True(paths.Children().Count() > 1);
    }
}
