using Wing;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddWing(builder => builder.AddConsul());

builder.Services.AddControllers();

builder.Services.AddWing()
                    .AddPersistence()
                    .AddGateWay();
                    //.AddEventBus() // �������ʹ��EventBus��¼������־������ɾ�����д���;


var app = builder.Build();

app.Run();
