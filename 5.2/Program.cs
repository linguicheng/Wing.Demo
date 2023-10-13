using _5._2.Service;
using FreeSql;
using Wing;
using Wing.ServiceProvider;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddWing(builder => builder.AddConsul());

builder.Services.AddControllers();

var fsql = new FreeSqlBuilder()
                        .UseConnectionString(DataType.SqlServer, builder.Configuration["ConnectionStrings:Wing.Demo"])
                        .UseAutoSyncStructure(true) // �Զ�ͬ��ʵ��ṹ�����ݿ⣬FreeSql����ɨ����򼯣�ֻ��CRUDʱ�Ż����ɱ�
                        .Build();
builder.Services.AddSingleton(typeof(IFreeSql), serviceProvider => fsql);
builder.Services.AddScoped<IFreeSqlDemoService, FreeSqlDemoService>();
builder.Services.AddWing()
        .AddPersistence()
        .AddAPM(x => x.AddFreeSql().Build(fsql));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
