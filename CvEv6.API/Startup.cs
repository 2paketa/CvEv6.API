using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CvEv6.API.Entities;
using CvEv6.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CvEv6.API
{
    public class Startup
    {
        public static IConfiguration Configuration { get; private set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            var connectionString = Startup.Configuration["connectionString:CvEv6DB2ConnectionString"];
            services.AddDbContext<CvEContext>(o => o.UseSqlServer(connectionString));
            services.AddScoped<ICvERepository, CvERepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, CvEContext cvEContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            cvEContext.EnsureSeedDataForContext();

            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Entities.Domain, Models.DomainWithoutDocumentsDto>();
                cfg.CreateMap<Entities.Domain, Models.DomainDto>();
                cfg.CreateMap<Entities.Document, Models.DocumentDto>();
                cfg.CreateMap<Models.DocumentForCreationDto, Entities.Document>();
                cfg.CreateMap<Models.DocumentForUpdateDto, Entities.Document>();
                cfg.CreateMap<Entities.Document, Models.DocumentForUpdateDto>();
                cfg.CreateMap<Entities.Title, Models.TitleDto>();
                cfg.CreateMap<Models.DomainForCreationDto, Entities.Domain>();
                cfg.CreateMap<Models.TitleForCreationDto, Entities.Title>();
            });

            app.UseStatusCodePages();

            app.UseMvc();

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
