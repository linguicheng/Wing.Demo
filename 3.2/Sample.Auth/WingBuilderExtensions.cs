using System;
using Microsoft.Extensions.DependencyInjection;
using Sample.Auth;
using Wing.Configuration.ServiceBuilder;

namespace Wing
{
    public static class WingBuilderExtensions
    {
        public static IWingServiceBuilder AddJwt(this IWingServiceBuilder wingBuilder)
        {
            var config = App.GetConfig<JwtSetting>("Jwt");
            if (config == null)
            {
                throw new ArgumentNullException(nameof(JwtSetting));
            }

            wingBuilder.Services.AddSingleton(typeof(IAuth), new JwtAuth(config));
            return wingBuilder;
        }
    }
}
