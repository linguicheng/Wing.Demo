using Sample.Auth;
using Wing;
using Wing.Saga.Server;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddWing(builder => builder.AddConsul());

builder.Services.AddControllers();

builder.Services.AddWing()
        .AddJwt()
        .AddPersistence()
       // .AddEventBus()
        .AddSaga(serviceProvider =>
        {
            var token = $"Bearer {serviceProvider.GetRequiredService<IAuth>().GetToken()}";
            return new SagaOptions
            {
                Headers = new Dictionary<string, string> { { "Authorization", token } }
            };
        });

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
