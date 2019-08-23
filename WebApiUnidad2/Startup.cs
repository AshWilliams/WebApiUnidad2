using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace WebApiUnidad2
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddSwaggerGen(c=> {
                c.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Version = "1806IFTE-1 - PROYECTO DE INTEGRACION III, UNIDAD 2",
                    Description = "Web Service Desarrollado por el grupo de Laura, José y Robert",
                    Contact = new Swashbuckle.AspNetCore.Swagger.Contact()
                    {
                        Name = "Robert Rozas Navarro",
                        Email = "robert.rozas@uniacc.edu"
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c=> {
                c.SwaggerEndpoint("v1/swagger.json","API Precios");
            });
        }
    }
}
