using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Net.Http;
using VOD.Common.Entities;
using VOD.Common.Services;
using VOD.Database.Contexts;
using VOD.Database.Services;

namespace VOD.Admin
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
            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            //    options.CheckConsentNeeded = context => true;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //});

            services.AddDbContext<VODContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<VODUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<VODContext>();

            services.AddControllersWithViews();

            services.AddHttpClient("AdminClient", client =>
            {
                client.BaseAddress = new Uri("http://localhost:6600");
                client.Timeout = new TimeSpan(0, 0, 30);
                client.DefaultRequestHeaders.Clear();
            }).ConfigurePrimaryHttpMessageHandler(handler =>
                new HttpClientHandler()
                {
                    AutomaticDecompression = System.Net.DecompressionMethods.GZip
                });

            //services.AddAutoMapper(); // Version 6.0.0: AutoMapper.Extensions.Microsoft.DependencyInjection

            // Version 6.1.0: AutoMapper.Extensions.Microsoft.DependencyInjection
            services.AddAutoMapper(typeof(Startup), typeof(Instructor), typeof(Course), typeof(Module), typeof(Video), typeof(Download));    
            services.AddScoped<IDbReadService, DbReadService>();
            services.AddScoped<IDbWriteService, DbWriteService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAdminService, AdminEFService>();
            //services.AddScoped<IAdminService, AdminAPIService>();
            services.AddScoped<IHttpClientFactoryService, HttpClientFactoryService>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseDatabaseErrorPage();  // Obsoleto 3.1
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            //app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseEndpoints(configure => { });

        }
    }
}
