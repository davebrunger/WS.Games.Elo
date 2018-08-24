using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using WS.Games.Elo.Lib.Elo;
using WS.Games.Elo.Lib.Repositories;
using WS.Games.Elo.Lib.Services;
using WS.Games.Elo.Web.Services;

namespace WS.Games.Elo.Web
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
            services.AddMvc();

            var baseDirectory = "/Users/davidbrunger/Documents/Visual Studio Code Projects/WS.Games.Elo/Data";
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("a secret that needs to be at least 16 characters long"));
            var securityServiceConfiguration = new SecurityServiceConfiguration(secretKey, "issuer", "audience", 28);

            services.AddSingleton<PlayerService>();
            services.AddSingleton<IRepositoryFactory>(new JsonRepositoryFactory(baseDirectory));
            services.AddSingleton<IPlayerServiceConfiguration, Configuration>();
            services.AddSingleton<GameService>();
            services.AddSingleton<EloCalculator>(new EloCalculator(32));
            services.AddSingleton<IGameServiceConfiguration, Configuration>();
            services.AddSingleton<SecurityService>();
            services.AddSingleton<UserService>();
            services.AddSingleton<ISecurityServiceConfiguration>(securityServiceConfiguration);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "JwtBearer";
                options.DefaultChallengeScheme = "JwtBearer";
            }).AddJwtBearer("JwtBearer", jwtBearerOptions =>
            {
                jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = securityServiceConfiguration.SecretKey,

                    ValidateIssuer = true,
                    ValidIssuer = securityServiceConfiguration.Issuer,

                    ValidateAudience = true,
                    ValidAudience = securityServiceConfiguration.Audience,

                    ValidateLifetime = true, //validate the expiration and not before values in the token

                    ClockSkew = TimeSpan.FromMinutes(5) //5 minute tolerance for the expiration date
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });
        }
    }
}
