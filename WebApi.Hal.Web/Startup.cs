using System.Reflection;
using DbUp;
using DbUp.SQLite.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WebApi.Hal.Web.Data;

namespace WebApi.Hal.Web
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<BeerDbContext>((oa) => oa.UseSqlite("Data Source=beer.db"));
            services.AddScoped<IBeerDbContext>(provider => provider.GetService<BeerDbContext>());
            services.AddScoped<IRepository, BeerRepository>();

            services.AddMvc()
                .AddJsonHalFormatters();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<BeerDbContext>();
                DeployChanges.To
                    .SQLiteDatabase("Data Source=beer.db")
                    .WithScriptsEmbeddedInAssembly(typeof(Startup).GetTypeInfo().Assembly)
                    .LogToConsole()
                    .Build()
                    .PerformUpgrade();
            }
        }
    }
}
