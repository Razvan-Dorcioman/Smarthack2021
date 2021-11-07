using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Smarthack2021.Core.BusinessObject;
using Smarthack2021.Core.LoginAbstractions;
using Smarthack2021.Data;
using Smarthack2021.Core;
using Smarthack2021.Core.CryptoAbstractions;
using Smarthack2021.Core.CryptoAlgorithms;
using Smarthack2021.MapperProfiles;

namespace Smarthack2021
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
            services.AddTransient<ICryptoOrchestrator, CryptoOrchestrator>();
            services.AddTransient<IKeyVaultService, KeyVaultService>();
            services.AddTransient<IRSAEncryption, RSAEncryption>();
            services.AddTransient<IUserRepository, UserRepository>();
            
            Environment.SetEnvironmentVariable("AZURE_CLIENT_ID", "92a26559-fe8a-4f53-a71f-c285c6580945");
            Environment.SetEnvironmentVariable("AZURE_TENANT_ID", "7e9f0b36-b938-4828-a039-b4f52b4d8e51");
            Environment.SetEnvironmentVariable("AZURE_CLIENT_SECRET", "NzY9yb4Y3yhBJFa5~dufNYD0OOokBgfxUv");

            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddCors(options =>
            {
                options.AddPolicy("EnableCORS", builder =>
                {
                    builder.AllowAnyOrigin()
                       .AllowAnyHeader()
                       .AllowAnyMethod();
                });
            });

            services.AddDbContext<UserContext>(o => {
                o.UseSqlServer(Configuration.GetConnectionString("RealConnection"));
            });

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<UserContext>();

            services.AddAutoMapper(typeof(Startup), typeof(CryptoProfile));

            services.AddAuthentication(opt => {
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
                    ValidIssuer = "http://localhost:5000",
                    ValidAudience = "http://localhost:5000",
                    //TODO: Change SecurityKey retrieval
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("SymetricSecurityKey").Value))
                };
            });

            services.AddSingleton<ITokenLogic, TokenLogic>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors("EnableCORS");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}