using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PUC.LDSI.DataBase.Context;
using PUC.LDSI.Domain.Entities;

[assembly: HostingStartup(typeof(PUC.LDSI.ModuloAluno.Areas.Identity.IdentityHostingStartup))]
namespace PUC.LDSI.ModuloAluno.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<SecurityContext>(opc => opc.UseSqlServer(context.Configuration.GetConnectionString("Conexao"), x => x.MigrationsAssembly("PUC.LDSI.DataBase")));

                services.AddDefaultIdentity<Usuario>().AddEntityFrameworkStores<SecurityContext>();

                //Password Settings.
                services.Configure<IdentityOptions>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 6;
                    options.Password.RequiredUniqueChars = 0;
                });
            });
        }
    }
}