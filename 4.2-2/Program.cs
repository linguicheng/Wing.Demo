using Sample.Saga.Client.Grpc;
using Sample.Saga.Client.Grpc.Services;
using Wing;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddWing(builder => builder.AddConsul());

builder.Services.AddGrpc(options =>
{
    options.MaxReceiveMessageSize = 1 * 1024 * 1024; // 1 MB
    options.MaxSendMessageSize = 1 * 1024 * 1024; // 1 MB
});

builder.Services.AddWing().AddPersistence().AddSaga().AddEventBus().AddJwt();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapGrpcService<GreeterService>();
app.MapGrpcService<HealthCheck>();
app.MapGrpcService<MyTranRetryService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
