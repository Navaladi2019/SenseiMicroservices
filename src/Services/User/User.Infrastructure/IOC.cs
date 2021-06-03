using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Application;
using User.Application.IRepository;
using User.Infrastructure.Repositories;

namespace User.Infrastructure
{
   public static class IOC
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IMongoContext, MongoContext>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ISignInRepository, SignInRepository>();
            services.AddScoped<IAuthTokenSettingRepository, AuthTokenSettingRepository>();
            services.AddScoped<IForgetPasswordSettingsRepository, ForgetPasswordSettingsRepository>();
            return services;
        }
    }
}
