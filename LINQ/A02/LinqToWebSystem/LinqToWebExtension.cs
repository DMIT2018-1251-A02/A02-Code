using LinqToWebSystem.BLL;
using LinqToWebSystem.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqToWebSystem
{
    public static class LinqToWebExtension
    {
        // This is an extension method that extends the IServiceCollection interface.
        // It is typically used in ASP.NET Core applications to configure and register services.

        // The method name can be anything, but it must match the name used when calling it in
        // your Program.cs file using builder.Services.XXXX(options => ...).
        public static void AddBackendDependencies(this IServiceCollection services,
            Action<DbContextOptionsBuilder> options)
        {
            // Register the LinqToWebContext class, which is the DbContext for your application,
            // with the service collection. This allows the DbContext to be injected into other
            // parts of your application as a dependency.

            // The 'options' parameter is an Action<DbContextOptionsBuilder> that typically
            // configures the options for the DbContext, including specifying the database
            // connection string.

            services.AddDbContext<LinqToWebContext>(options);

            //  adding any services that you create in the class library (BLL)
            //  using .AddTransient<t>(...)
            //  working versions
            services.AddTransient<ArtistService>((ServiceProvider) =>
            {
                //  Retrieve an instance of LinqToWebContext from the service provider.
                var context = ServiceProvider.GetService<LinqToWebContext>();

                // Create a new instance of ArtistService,
                //   passing the LinqToWebContext instance as a parameter.
                return new ArtistService(context);
            });
        }
    }
}
