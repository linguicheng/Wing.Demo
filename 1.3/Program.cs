using Sample.Auth;
using Wing;
using Wing.UI;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddWing(builder => builder.AddConsul());

builder.Services.AddControllers();

builder.Services.AddWing()
                 .AddWingUI(FreeSql.DataType.SqlServer)
                 .AddJwt();

var app = builder.Build();
UserContext.UserLoginAfter = user =>
{
    user.Token = app.Services.GetService<IAuth>().GetToken();
    return user;
};

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
