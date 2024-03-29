using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Smartfinance_server.Data;

namespace Smartfinance_server
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
            services.AddCors();
            services.AddControllers();
            services.AddScoped<QueryEngine>();
            services.AddHttpContextAccessor();
            services.Add(new ServiceDescriptor(typeof(DbContext), new DbContext(Configuration.GetConnectionString("DefaultConnection"))));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "smartfinance", Version = "v1" }) ;
            });

            services.AddAuthentication("CookieAuth")
                .AddCookie("CookieAuth", config =>
                {
                    config.Cookie.Name = "Identity.Cookie";
                    config.LoginPath = "/api/user/login";
                    config.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None;
                    config.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always;
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                if (env.IsDevelopment())
                    app.UseDeveloperExceptionPage();
                //else
                //app.UseExceptionHandler("/error");              

                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "smartfinance");
                    c.RoutePrefix = string.Empty;
                });
            }

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            // where do you want to go?
            app.UseRouting();

            // add cors headers
            app.UseCors(
                options => options.WithOrigins("https://malikotinas.com", "https://localhost:3000").AllowAnyHeader().AllowAnyMethod().AllowCredentials()
            );

            // // who are you?
            app.UseAuthentication();

            // // are you allowed to visit?
            app.UseAuthorization();

            // use controllers to handle the request
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
