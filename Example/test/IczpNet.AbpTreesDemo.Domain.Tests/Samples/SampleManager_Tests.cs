using System.Threading.Tasks;
using Xunit;

namespace IczpNet.AbpTreesDemo.Samples;

public class SampleManager_Tests : AbpTreesDemoDomainTestBase
{
    //private readonly SampleManager _sampleManager;

    public SampleManager_Tests()
    {
        //_sampleManager = GetRequiredService<SampleManager>();
    }

    [Fact]
    public async Task Method1Async()
    {
        await Task.Yield();
    }
}
