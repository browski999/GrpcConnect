using Grpc.Core;
using GrpcConnect;

namespace GrpcConnect.Services
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            TimeZoneInfo londonTime = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");

            return Task.FromResult(new HelloReply
            {                
                Message = "Hello " + request.Name + ", the time is " + TimeZoneInfo.ConvertTime(DateTime.UtcNow, londonTime).ToString("hh:mm tt") + " in London."
            });
        }
    }
}
