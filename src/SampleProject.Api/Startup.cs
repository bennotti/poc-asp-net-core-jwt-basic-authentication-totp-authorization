using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SampleProject.Core.Settings;
using SampleProject.Infrastructure.Authentication;
using SampleProject.Infrastructure.Authentication.Handle;
using SampleProject.Infrastructure.Authorization;
using SampleProject.Infrastructure.Authorization.Handle;
using SampleProject.Infrastructure.Authorization.Requirements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleProject.Api
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
            services.AddControllers();
            services.AddTransient<Random>();

            var jwtSettings = new JwtSettings();
            new ConfigureFromConfigurationOptions<JwtSettings>(Configuration.GetSection("JwtSettings")).Configure(jwtSettings);
            var jwtBuilder = new JwtBuilder(jwtSettings);
            services.AddSingleton(jwtSettings);
            services.AddSingleton(jwtBuilder);

            services.AddAuthorization(options =>
            {
                options.AddPolicy("TotpValidate", (policy) => {
                    policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                    policy.AuthenticationSchemes.Add("Basic");
                    policy.RequireAuthenticatedUser();
                    policy.Requirements.Add(new TotpAuthorizationRequirement());
                });

                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .AddAuthenticationSchemes("Basic", "Bearer")
                    .AddRequirements(new JwtAuthorizationRequirement())
                    .AddRequirements(new BasicAuthorizationRequirement())
                    .Build();
            });

            services.AddHttpContextAccessor();

            services.AddSingleton<IAuthorizationHandler, JwtAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, TotpAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, BasicAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, ValidIdentityAuthorizationHandler>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.IncludeErrorDetails = true;
                    options.TokenValidationParameters = jwtBuilder.BuildTokenValidationParameters();
                    options.Events = new JwtCustomEvents();
                })
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("Basic", null);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
