using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using JwtHelpers;
using UserService.DataAccess;

namespace UserService
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("Config/appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile("Config/jwtConfig.json", optional: true, reloadOnChange: true)
            .AddJsonFile("Config/databaseSettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile("Config/certificates.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();


            var trustedCerts = Configuration.GetSection("TrustedCertificates").Get<List<string>>();
            var jwtConfig = Configuration.GetSection("Jwt").Get<JwtConfig>();


            ConfigureBindings(services);


            services.AddAuthentication()
                .AddJwtAuthenticationWithKeyAndIssuer(jwtConfig.Key, jwtConfig.Issuer)
                .AddCertificate(GlobalConstantsAndExtensions.CERTIFICATE_AUTHENTICATION_POLICY,options =>
                {
                    options.AllowedCertificateTypes = Microsoft.AspNetCore.Authentication.Certificate.CertificateTypes.All;
                    options.Events.OnCertificateValidated += (context) =>
                    {
                        if(trustedCerts.Contains(context.ClientCertificate.Thumbprint)) 
                        {
                            context.Success();
                        }
                        else 
                        {
                            context.Fail("Untrusted Client Certificate");
                        }
                        return Task.CompletedTask;
                    };
                });
            services.AddAuthorization();

        }


        private void ConfigureBindings(IServiceCollection services) 
        {
            services.AddSingleton<IDatabaseHelper, MySQLHelper>();
            services.AddSingleton(Configuration.GetSection("DatabaseConfig").Get<DatabaseConfig>());
            services.AddSingleton<IGetCharacterData, DatabaseCharacterAccesor>();
            services.AddSingleton<ISetCharacterData, DatabaseCharacterAccesor>();
            services.AddSingleton<ActiveCharacterManager>();
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
