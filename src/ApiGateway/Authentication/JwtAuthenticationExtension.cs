using InterServiceCommunication.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain;

namespace ApiGateway.Authentication
{
    public  static class JwtAuthenticationExtension
    {
        /// <summary>
        /// Validates Json Token against token validation parameter.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        /// 
        public static AuthenticationBuilder AddJwtAuthentication(this IServiceCollection services, IConfiguration Configuration)
        {
            try
            {
                
               // string jwttoken = httpcontext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last(); ;
               
                return services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                    
                })
                .AddJwtBearer("JWTKeyValidation", x =>
                {
                    //byte[] Symmetrickey = Encoding.ASCII.GetBytes("GGJKGHGJFHKKLSDGHFJHKLJSFFGRRGHVJLKJFERSDFXVVJHGJH456578DFGJHJKGGDFRDHGBJHRFRFGHGBJH");
                    //SecurityKey SecurityKey = new SymmetricSecurityKey(Symmetrickey);

                    //SecurityToken validatedToken;
                    //JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                    //var res = handler.ValidateToken("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiJjMDBmZmVmZC1kYTY0L" +
                    //           "TQ5NjQtYjJiZi0wZWM0ZjZiZGE5OTEiLCJuYW1laWQiOiI2MGI0YTIzZjgzNDM3ODY4ODhkMzQ5MDEiLCJBdXRoS" +
                    //           "WQiOiI2MGI0N2E2OWM1MTE1M2Q4ZGY4MjM4NjQiLCJpYXQiOiIxNjIyNDUwNzYwIiwiUm9sZSI6IlN0dWRlbnQiL" +
                    //           "CJuYmYiOjE2MjI0NTA3NjAsImV4cCI6MTYzODI2MTk1MX0.dd4QKjIAkXm_MG-yMjMBQ4B3-kodSYKFEqmyNAS1NK8", new TokenValidationParameters()
                    //           {
                    //               ValidateLifetime = true,
                    //               IssuerSigningKey = SecurityKey,
                    //               ValidateIssuerSigningKey = false,
                    //               ValidateActor = false,
                    //               ValidateIssuer = false,
                                   
                    //               ClockSkew = TimeSpan.Zero,
                    //               ValidateAudience = false
                    //           }, out validatedToken); ; ;


                    //x.TokenValidationParameters = new TokenValidationParameters()
                    //{
                    //    ValidateLifetime = true,
                    //    IssuerSigningKey = SecurityKey,
                    //    ClockSkew = TimeSpan.Zero
                    //};
                    x.SaveToken = true;

                    x.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = async  (context) =>
                        {
                            context.Token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                            var tokenClaims = new JwtSecurityTokenHandler().ReadJwtToken(context.Token);
                            string authsettingId = tokenClaims.Claims.Where(x => x.Type == "AuthId").FirstOrDefault()?.Value;

                            IUserInterService userInterService = services.BuildServiceProvider().GetRequiredService<IUserInterService>();
                            AuthTokenSetting AuthTokenSetting = await userInterService.GethashKeyForAuthentication(authsettingId);
                            if (AuthTokenSetting == null || AuthTokenSetting.IsActive == false)
                                throw new UnauthorizedAccessException("Session expired. Please login again.");
                            byte[] Symmetrickey = Encoding.ASCII.GetBytes(AuthTokenSetting.SecretKey);
                            SecurityKey SecurityKey = new SymmetricSecurityKey(Symmetrickey);
                            x.TokenValidationParameters = new TokenValidationParameters()
                            {
                                ValidateLifetime = true,
                                IssuerSigningKey = SecurityKey,
                                ValidateIssuerSigningKey = false,
                                ValidateActor = false,
                                ValidateIssuer = false,

                                ClockSkew = TimeSpan.Zero,
                                ValidateAudience = false
                            };

                            return ;
                        },
                        OnTokenValidated = context =>
                        {
                            return Task.CompletedTask;
                        },
                       



                    };
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;

                });

            }
            catch (Exception)
            {
                throw;
            }

        }

    }
}
