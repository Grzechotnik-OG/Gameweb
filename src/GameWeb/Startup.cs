using System;
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
                AccessTokenExpiration = 5,
                RefreshTokenExpiration = 10,
                Issuer = "https://localhost:5001/",
                Audience = "https://localhost:5001/",
                Secret = "SWRlYWx5IHNhIGphayBnd2lhemR5IC0gbmllIG1vem5hIGljaCBvc2lhZ25hYywgYWxlIG1vem5hIHNpZSBuaW1pIGtpZXJvd2FjLg0K"
            };
            services.AddControllers();

            services.AddDbContext<Context>(opt => opt.UseSqlite("Data Source=gameweb.db"));

            services.AddScoped<IGamesRepository, GamesRepository>();
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IGameGenresRepository, GameGenresRepository>();
            services.AddScoped<IDevelopersRepository, DevelopersRepository>();

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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
