using DataAccessLayer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Service.Services;
using Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public static class DIExtension
    {
            public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
            {
                services.AddScoped<IUserService, UserService>();
                services.AddScoped<IJwtService, JwtService>();

                services.AddDalServices(configuration);
                return services;
            }
        
    }
}
