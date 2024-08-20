using Wing;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddWing(builder => builder.AddConsul());

builder.Services.AddControllers();

builder.Services.AddWing()
                    .AddPersistence(FreeSql.DataType.SqlServer)
                    .AddGateWay();
                    //.AddEventBus() // �������ʹ��EventBus��¼������־������ɾ�����д���;


var app = builder.Build();

app.Run();
