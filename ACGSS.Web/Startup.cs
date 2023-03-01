using ACGSS.Application.Services;
using ACGSS.Domain.Services;
using ACGSS.Infrastructure.Database;
using ACGSS.Infrastructure.Extensions;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ACGSS.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblies(Assembly.GetExecutingAssembly().GetReferencedAssemblies().Select(Assembly.Load));

            services.AddDbContext<EFContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("EFContext")));

            services.SetupUnitOfWork();

            services.AddAutoMapper(Assembly.GetExecutingAssembly().GetReferencedAssemblies().Select(Assembly.Load));

            services.AddScoped<IUserService, UserService>();
        }
    }
}
