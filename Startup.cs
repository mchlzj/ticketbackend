using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ticketsystem_backend.Data;
using ticketsystem_backend.Migrations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace ticketsystem_backend
{
    public class Startup
    {
        readonly string MySpecificOrigin = "EnableCORS";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddCors();

            services.AddControllers();

            services.AddCors(options =>
            {
                options.AddPolicy(name: MySpecificOrigin, builder =>
                {
                    builder
                    //.WithOrigins("https://epic-northcutt-0cee3d.netlify.app", "https://www.hetfeld.name", "http://localhost")
                    //.WithOrigins("https://epic-northcutt-0cee3d.netlify.app", "http://localhost:3000")
                    .AllowAnyOrigin()
                    //.SetIsOriginAllowed(origin => true)
                    //.WithHeaders("Content-Type", "application/json")
                    //.WithMethods("PUT", "DELETE", "GET", "POST", "OPTIONS")
                    //.AllowCredentials()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    ;
                });
            });


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ticketsystem_backend", Version = "v1" });
            });

            services.AddDbContext<TicketSystemDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("TicketContext")));
            services.AddTransient<DbSeedData>();

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    
                    //ValidIssuer = "https://epic-northcutt-0cee3d.netlify.app",
                    ValidIssuer = "*",
                    ValidAudience = "*",
                    //ValidAudience = "https://epic-northcutt-0cee3d.netlify.app",
                    IssuerSigningKey = new SymmetricSecurityKey(Base64UrlEncoder.DecodeBytes("MBcCT4UEs67vh3shK683Lxhn33t2LTtH"))
                };
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DbSeedData seeder)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ticketsystem_backend v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(MySpecificOrigin);

            //app.UseCors(x => x
            //    .AllowAnyMethod()
            //    .AllowAnyHeader()
            //    .SetIsOriginAllowed(origin => true)
            //    .AllowCredentials());


            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();




                //endpoints.MapGet("/echo",
                //context => context.Response.WriteAsync("echo"))
                //.RequireCors(MySpecificOrigin);

                //endpoints.MapControllers()
                //         .RequireCors(MySpecificOrigin);

                //endpoints.MapGet("/echo2",
                //    context => context.Response.WriteAsync("echo2"));

                //endpoints.MapRazorPages();
            });
            seeder.EnsureSeedData();
        }
    }
}
