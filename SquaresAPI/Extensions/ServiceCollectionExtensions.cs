using BusinessLogic.Services;
using DataAccessLayer.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SquaresAPI.BusinessLogic;
using SquaresAPI.DataAcccessLayer.Repository;
using SquaresAPI.Filters;
using SquaresAPI.Services;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

namespace SquaresAPI
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors();
            services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddEndpointsApiExplorer();

            #region Swagger Configuration
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "SquareAPI",
                    Version = "v1",

                });
                c.ExampleFilters();
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });
            services.AddSwaggerExamplesFromAssemblyOf<SquareResponseExample>();
            services.AddSwaggerExamplesFromAssemblyOf<PointExample>();
            services.AddSwaggerExamplesFromAssemblyOf<BadRequestExample>();
            services.AddSwaggerExamplesFromAssemblyOf<NotFoundExample>();
            services.AddSwaggerExamplesFromAssemblyOf<UnAuthorizedExample>();
            #endregion

            #region JWT Configuration
            services.Configure<JwtSettings>(configuration.GetSection("AppSettings"));
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false; options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration.GetValue<string>("AppSettings:Secret"))),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            } );
            #endregion

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IPointService, PointService>();
            services.AddTransient<LogAttribute>();
            services.AddDbContext<AppDBContext>(x => x.UseSqlServer(configuration.GetConnectionString("DefaultConnectionString")));
            services.AddScoped<IPointRepository, PointRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddAuthorization();
            return services;
        }
    }
}
