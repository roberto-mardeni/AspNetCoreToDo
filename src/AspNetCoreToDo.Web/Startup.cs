namespace AspNetCoreToDo.Web
{
    using System.Reflection;
    using AspNetCoreToDo.Web.Models;
    using AutoMapper;
    using MediatR;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //var connection = @"Server=.\SQLEXPRESS2014;Database=AspNetCoreToDo;Trusted_Connection=True;";
            var connection = Configuration.GetConnectionString("ToDoContext");

            services.AddDbContext<ToDoContext>(options => options.UseSqlServer(connection));

            services.AddSession();
            services.AddMvc();
            services.AddAutoMapper(GetExecutingAssembly());
            services.AddMediatR(GetExecutingAssembly());
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private static Assembly GetExecutingAssembly()
        {
            return typeof(Startup).GetTypeInfo().Assembly;
        }
    }
}
