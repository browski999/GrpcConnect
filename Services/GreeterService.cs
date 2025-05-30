using System.Threading.Channels;
using Grpc.Core;

namespace GrpcConnect.Services
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }        

        public override async Task SayHelloBiStreamAsync(IAsyncStreamReader<HelloRequest> requestStream, IServerStreamWriter<HelloReply> responseStream, ServerCallContext context)
        {
            var channel = Channel.CreateUnbounded<HelloReply>();

            var t = Task.Run(async () =>
            {
                await foreach (var helloReply in channel.Reader.ReadAllAsync())
                {
                    await responseStream.WriteAsync(helloReply);
                }
            });

            var tasks = new List<Task>();

            try
            {
                await foreach (var request in requestStream.ReadAllAsync())
                {
                    tasks.Add(WriteGreetingAsync(request.Name));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            await Task.WhenAll(tasks);
            channel.Writer.TryComplete();
            await channel.Reader.Completion;
            await t;

            async Task WriteGreetingAsync(string name)
            {
                await channel.Writer.WriteAsync(new HelloReply()
                {
                    Message = "Hello " + name
                });
            }
        }    
    }
}
