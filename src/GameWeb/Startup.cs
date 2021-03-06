using System;
using System.IO;
using System.Reflection;
using System.Text;
using GameWeb.Models;
using GameWeb.Repositories;
using GameWeb.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace GameWeb
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            JWTConfig jwtConfig = new JWTConfig() //Zrób to inaczej
            {
                AccessTokenExpiration = Convert.ToInt32(Environment.GetEnvironmentVariable("JwtAccessTokenExpiration")),
                RefreshTokenExpiration = Convert.ToInt32(Environment.GetEnvironmentVariable("JwtRefreshTokenExpiration")),
                Issuer = Environment.GetEnvironmentVariable("JwtIssuer"),
                Audience = Environment.GetEnvironmentVariable("JwtAudience"),
                Secret = Environment.GetEnvironmentVariable("JwtSecret")
            };
            services.AddControllers();

            services.AddDbContext<Context>(opt => opt.UseSqlite("Data Source=gameweb.db"));

            services.AddScoped<IGamesRepository, GamesRepository>();
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IGameGenresRepository, GameGenresRepository>();
            services.AddScoped<IDevelopersRepository, DevelopersRepository>();

            services.AddSwaggerGen(c =>{
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddScoped<IAuthService, AuthService>();
            services.AddSingleton<JWTConfig>(jwtConfig);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtConfig.Issuer,
                    ValidAudience = jwtConfig.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Secret)),
                    ClockSkew = TimeSpan.Zero
                };
            });
            services.AddAuthorization(config =>
            {
                config.AddPolicy(Policies.Admin, Policies.AdminPolicy());
                config.AddPolicy(Policies.User, Policies.UserPolicy());
                config.AddPolicy(Policies.Mod, Policies.ModPolicy());
                config.AddPolicy(Policies.RefreshToken, Policies.RefreshTokenPolicy());
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");

            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
