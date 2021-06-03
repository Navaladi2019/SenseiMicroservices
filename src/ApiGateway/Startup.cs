using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ocelot.Provider.Consul;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using ConsolServiceDiscovery;
using InterServiceCommunication.User;
using ApiGateway.Authentication;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace ApiGateway
{
    public class Startup
    {
    
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddOcelot().AddConsul();
            services.AddMvcCore().AddApiExplorer();
            services.AddConsul(Configuration.GetConsolServiceConfig());
            services.AddHttpClient();
            services.AddScoped<IUserInterService, UserInterService>();
            services.AddJwtAuthentication(Configuration);
            congigureswagger(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                 app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerForOcelotUI(opt =>
            {
                opt.PathToSwaggerGenerator = "/swagger/docs";
                opt.DownstreamSwaggerHeaders = new[]
                    {
                        new KeyValuePair<string, string>("AuthId", "AuthValue"),
                    };

            });
            app.UseRouting();
          //  app.UseAuthentication();
           app.UseAuthorization();

            var configration = new OcelotPipelineConfiguration
            {
                AuthorizationMiddleware = async (ctx, next) =>
                {
                    if (this.Authorize(ctx))
                    {
                        await next.Invoke();
                    }
                    else
                    {
                        ctx.Items.SetError(new UnauthorisedError($"Fail to authorize"));
                    }
                }
            };

            await app.UseOcelot(configration);
        }


        private bool Authorize(HttpContext ctx)
        {
            var aa = ctx.Items.DownstreamRoute();
            if (ctx.Items.DownstreamRoute()?.AuthenticationOptions?.AuthenticationProviderKey == null) return true;
            else
            {

                bool auth = false;
                Claim[] claims = ctx.User.Claims.ToArray<Claim>();
                Dictionary<string, string> required = ctx.Items.DownstreamRoute().RouteClaimsRequirement;
                Regex reor = new Regex(@"[^,\s+$ ][^\,]*[^,\s+$ ]");
                MatchCollection matches;

                Regex reand = new Regex(@"[^&\s+$ ][^\&]*[^&\s+$ ]");
                MatchCollection matchesand;
                int cont = 0;
                foreach (KeyValuePair<string, string> claim in required)
                {
                    matches = reor.Matches(claim.Value);
                    foreach (Match match in matches)
                    {
                        matchesand = reand.Matches(match.Value);
                        cont = 0;
                        foreach (Match m in matchesand)
                        {
                            foreach (Claim cl in claims)
                            {
                                if (cl.Type == claim.Key)
                                {
                                    if (cl.Value == m.Value)
                                    {
                                        cont++;
                                    }
                                }
                            }
                        }
                        if (cont == matchesand.Count)
                        {
                            auth = true;
                            break;
                        }
                    }
                }
                return auth;
            }
        }


        void  congigureswagger(IServiceCollection services)
        {
            services.AddSwaggerForOcelot(Configuration, (o) =>
            {
                o.GenerateDocsForGatewayItSelf = true;
                o.GenerateDocsDocsForGatewayItSelf((c) =>
                {
                    //  c.OperationFilter<ExamplesOperationFilter>();
                    c.AddSecurityDefinition("Bearer",
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                            Description = "Please enter  the word 'Bearer' followed by a space and JWT",
                            Name = "Authorization",
                            Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
                        });
                    c.AddSecurityRequirement(new OpenApiSecurityRequirement(){
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                },

                                Name = "Bearer",
                                In = ParameterLocation.Header,

                            },
                            new List<string>()
                        }
                    });
                });
            });
        }


    }
}
