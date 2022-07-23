using AutenticaB.Constants;
using AutenticaB.Data;
using Microsoft.EntityFrameworkCore;

namespace AutenticaB
{
    public static class SetUp
    {
        public static IServiceCollection SetUpInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

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

            return services;
        }
    }
}
