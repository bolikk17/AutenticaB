using AutenticaB.Constants;
using AutenticaB.Data;
using AutenticaB.Services;
using Microsoft.EntityFrameworkCore;

namespace AutenticaB
{
    public static class SetUp
    {
        public static IServiceCollection SetUpInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Database added
            services.AddDbContext<DataContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            // Allow reqests from localhost:4200
            services.AddCors(options =>
            {
                options.AddPolicy(name: Const.AngularOrigin,
                    builder =>
                    {
                        builder
                        .WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                    ;
                    });
            });

            // Service for files
            services.AddTransient<IFileManager, FileManagerService>();

            return services;
        }
    }
}
