using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsedProductExchange.Core.Application;
using UsedProductExchange.Core.Application.Implementation;
using UsedProductExchange.Core.Domain;
using UsedProductExchange.Core.Entities;
using UsedProductExchange.Infrastructure;
using UsedProductExchange.Infrastructure.Context;
using UsedProductExchange.Infrastructure.Repositories;

namespace UsedProductExchange.UI
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
            
            services.AddDbContext<UsedProductExchangeContext>(opt => opt.UseSqlite("Data Source=GameShop.db"),
            ServiceLifetime.Transient);

            services.AddScoped<IService<Category>, CategoryService>();
            services.AddScoped<IRepository<Category>, CategoryRepository>();

            services.AddScoped<IService<User>, UserService>();
            services.AddScoped<IRepository<User>, UserRepository>();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                using (var scope = app.ApplicationServices.CreateScope())
                {
                    var context = scope.ServiceProvider.GetService<UsedProductExchangeContext>();

                    context.Database.EnsureDeleted();
                    context.Database.EnsureCreated();

                    var userRepository = scope.ServiceProvider.GetService<IRepository<User>>();
                    var categoryRepository = scope.ServiceProvider.GetService<IRepository<Category>>();

                }
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
