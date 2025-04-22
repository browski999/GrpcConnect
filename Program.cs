using GrpcConnect.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace GrpcConnect
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.WebHost.ConfigureKestrel(serverOptions =>
            {
                // Set the HTTP/2 port to 8080
                serverOptions.ListenAnyIP(8080, listenOptions =>
                {
                    listenOptions.Protocols = HttpProtocols.Http2;
                    listenOptions.UseHttps("certs/localhost.pfx", "Santiago1#");
                });
            });

            // Add services to the container.
            builder.Services.AddGrpc();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.MapGrpcService<GreeterService>();
            app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

            app.Run();
        }
    }
}