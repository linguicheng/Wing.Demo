using Wing;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddWing(builder => builder.AddConsul());

builder.Services.AddWing()
                    .AddJwt()
                    .AddPersistence()
                    .AddGateWay(null,new WebSocketOptions
                    {
                        KeepAliveInterval = TimeSpan.FromMinutes(2)
                    });

var app = builder.Build();
app.Run();


