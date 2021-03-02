using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using MyCourse.Models.Services.Application;
using MyCourse.Models.Services.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MyCourse.Models.Options;
using Microsoft.Extensions.Caching.Memory;

namespace MyCourse
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup (IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
             services.AddTransient<ICourseService, AdoNetCourseService>();
            //services.AddTransient<ICourseService, EfCoreCourseService>();
            services.AddTransient<IDatabaseAccessor, SqliteDatabaseAccessor>();
            services.AddTransient<ICachedCourseService, MemoryCacheCourseService>();

            // services.AddScoped<MyCourseDbContext>();
            // services.AddDbContext<MyCourseDbContext>();
            services.AddDbContextPool<MyCourseDbContext>(optionsBuilder =>
            {
                string connectionString = Configuration.GetSection("ConnectionStrings").GetValue<string>("Default");
                optionsBuilder.UseSqlite(connectionString);
            });

            #region Config servizio cache distribuita
            // services.AddStackExchangeRedisCache(options =>
            // {
            //     Configuration.Bind("DistributedCache:Redis", options);
            // });
            // services.AddDistributedSqlServerCache(option =>
            // {
            //     Configuration.Bind("DistributedCache:SqlServer",option)
            // }
            // );
            //services.AddDistributedMemoryCache();
            #endregion

            //options
            services.Configure<CoursesOptions>(Configuration.GetSection("Courses"));
            services.Configure<ConnectionStringsOptions>(Configuration.GetSection("ConnectionStrings"));
            services.Configure<MemoryCacheOptions>(Configuration.GetSection("MemoryCache"));
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime lifetime)
        {
            //if (env.IsDevelopment())
            if (env.IsEnvironment("Development"))
            {
                app.UseDeveloperExceptionPage();

                //Aggiorniamo un file per notificare a BrowserSync che devo aggiornare la pagina
                lifetime.ApplicationStarted.Register(() =>
                {
                    string filePath = Path.Combine(env.ContentRootPath,"bin/reload.txt");
                    File.WriteAllText(filePath,DateTime.Now.ToString());
                });
            }
            else{
                app.UseExceptionHandler("/Error");
            }
            //per usare i file in wwwroot
            app.UseStaticFiles();
            
            /*app.Run(async (context) =>
            {
                string nome = context.Request.Query["nome"];
                if (nome==null)
                {
                    nome="SCONOSCIUTO";
                }
                //nome=nome.ToUpper();

                await context.Response.WriteAsync($"CIAO {nome.ToUpper()}!");
                


            });
            */
            //app.UseMvcWithDefaultRoute();
            app.UseMvc(routbuilder =>
            {
                routbuilder.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
