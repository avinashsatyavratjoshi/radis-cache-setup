using Microsoft.OpenApi.Models;
using RadisCache.Radis;
internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        
        builder.Services.ConfigureRadisCache(builder.Configuration.GetSection("RedisCache"));

        builder.Services.AddControllers();

        builder.Services.AddSwaggerGen(c=>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Radis Cache Demo API", Version = "v1" });
            c.CustomSchemaIds(x => x.FullName);
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Radis Cache Demo API");
        });

        app.Run();
    }
}

