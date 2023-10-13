using Wing;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddWing(builder => builder.AddConsul());

builder.Services.AddControllers();

builder.Services.AddWing()
                    .AddPersistence()
                    .AddGateWay();
                    //.AddEventBus() // 如果不想使用EventBus记录请求日志，可以删除此行代码;


var app = builder.Build();

app.Run();
