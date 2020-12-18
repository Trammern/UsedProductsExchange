using System;
using System.IO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using UsedProductExchange.Core.Application;
using UsedProductExchange.Core.Application.Implementation;
using UsedProductExchange.Core.Domain;
using UsedProductExchange.Core.Entities;
using UsedProductExchange.Infrastructure.Context;
using UsedProductExchange.Infrastructure.DBInitializer;
using UsedProductExchange.Infrastructure.Repositories;

namespace UsedProductExchange.UI
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Create a byte array with random values. This byte array is used
            // to generate a key for signing JWT tokens.
            byte[] secretBytes = new byte[40];
            var rand = new Random();
            rand.NextBytes(secretBytes);
            
            // Add JWT based authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretBytes),
                    ValidateLifetime = true, // validate the expiration and not before values in the token
                    ClockSkew = TimeSpan.FromMinutes(5) // 5 minute tolerance for the expiration date
                };
            });
            
            if (Environment.IsDevelopment())
            {
                // SqLite database:
                services.AddDbContext<UsedProductExchangeContext>(opt =>
                    opt.UseSqlite("Data Source=UsedProductExchange.db"));
                // Register SqLite database initializer for dependency injection.
                services.AddTransient<IDbInitializer, DbInitializer>();
            }
            else
            {
                // Azure SQL database:
                services.AddDbContext<UsedProductExchangeContext>(opt =>
                    opt.UseSqlServer(Configuration.GetConnectionString("defaultConnection")));
                // Register SQL Server database initializer for dependency injection.
                services.AddTransient<IDbInitializer, SqlDbInitializer>();
            }
            
            services.Configure<FormOptions>(o => {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
                    new Microsoft.OpenApi.Models.OpenApiInfo
                    {
                        Title = "Swagger UPE",
                        Description = "Swagger for UsedProductExchange",
                    });
            });

           
            services.AddScoped<IService<Category>, CategoryService>();
            services.AddScoped<IRepository<Category>, CategoryRepository>();

            services.AddScoped<IService<User>, UserService>();
            services.AddScoped<IRepository<User>, UserRepository>();
            
            services.AddScoped<IService<Product>, ProductService>();
            services.AddScoped<IRepository<Product>, ProductRepository>();

            services.AddScoped<IService<Bid>, BidService>();
            services.AddScoped<IRepository<Bid>, BidRepository>();
            
            services.AddSingleton<ILoginService>(new LoginService(secretBytes));

            // Configure the default CORS policy.
            services.AddCors(options =>
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins("http://www.ministuff.azurewebsites.net", "https://www.ministuff.azurewebsites.net")
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                    })
            );

            // Ignore JSON model relations loop
            services.AddControllers().AddNewtonsoftJson(o =>
            {
                o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                //o.SerializerSettings.MaxDepth = 5;
            });
            
            services.AddDbContext<UsedProductExchangeContext>(opt => 
                    opt.UseSqlite("Data Source=UsedProductExchange.db"),ServiceLifetime.Transient);
            
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
    
                    if (context != null)
                    {
                        context.Database.EnsureDeleted();
                        context.Database.EnsureCreated();
                    }
    
                    var services = scope.ServiceProvider;
                    var dbInitializer = services.GetService<IDbInitializer>();
                    dbInitializer.Initialize(context);
    
                }

                app.UseDeveloperExceptionPage();
            }
            
            // Initialize the database.
            using (var scope = app.ApplicationServices.CreateScope())
            {
                // Initialize the database
                var services = scope.ServiceProvider;
                var dbContext = services.GetService<UsedProductExchangeContext>();
                var dbInitializer = services.GetService<IDbInitializer>();
                dbInitializer.Initialize(dbContext);
            }

            app.UseHttpsRedirection();
            
            app.UseStaticFiles();
            
            Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), @"Resources", @"Images", @"Products")); 
            
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
                RequestPath = new PathString("/Resources")
            });
            
            app.UseRouting();
            
            app.UseCors();
            
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger Demo");
            });
        }
    }
}
