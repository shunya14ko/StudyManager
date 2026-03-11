using System.Threading.Channels;

namespace TaskManager.Services;

public class SampleCreateBounded
{
    public void Create()
    {
        // Create a bounded channel with a capacity of 1
        var channel = Channel.CreateBounded<string>(1);

        channel.Writer.WriteAsync("Hello, World!");
        channel.Reader.ReadAllAsync();
    }
}
