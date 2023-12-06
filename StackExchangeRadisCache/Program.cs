using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using RadisCache.Radis;
//using Microsoft.OpenApi.Models;

namespace SwaggerImplementation.Api
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

        //services.AddSwaggerGen(c =>
        //{
        //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Employee API", Version = "v1" });
        //    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        //    {
        //        In = ParameterLocation.Header,
        //        Description = "Please insert JWT with Bearer into field",
        //        Name = "Authorization",
        //        Type = SecuritySchemeType.ApiKey
        //    });
        //    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        //    {
        //      new OpenApiSecurityScheme
        //      {
        //        Reference = new OpenApiReference
        //        {
        //          Type = ReferenceType.SecurityScheme,
        //          Id = "Bearer"
        //        }
        //       },
        //       new string[] { }
        //     }
        //    });
        //    c.CustomSchemaIds(x => x.FullName);
        //});
        services.ConfigureRadisCache(Configuration.GetSection("RedisServer"));
            services.AddControllers();

        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            //app.UseSwagger();
            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Employee V1");
            //});
            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}