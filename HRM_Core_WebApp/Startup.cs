using HRM_Application.Interfaces;
using HRM_Application.Services;
using HRM_Core_WebApp.HubService;
using HRM_Domain.Model;
using HRM_Infrastructure.HRMDataBaseContext;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace HRM_Core_WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllHeaders",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                    });

            });
            //signalR
            services.AddSignalR(options =>
            {
                options.EnableDetailedErrors = true;
            });
            //services.AddCors(options =>
            //{
            //    options.AddPolicy(
            //        name: "AllowOrigin",
            //        builder =>
            //        {
            //            builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            //        });
            //});

            services.Configure<FormOptions>(o =>
            {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });
            services.AddTransient<IPostJobServices, PostJobServices>();
            services.AddTransient<ICompanyRegistrationServices, CompanyRegistrationServices>();
            services.AddTransient<IEmployeeServices, EmployeeServices>();
            services.AddTransient<IDashboardServices, DashboardServices>();
            services.AddTransient<IJobApplicationService, JobApplicationService>();
            services.AddTransient<IEmployeeAttendanceServices, EmployeeAttendanceServices>();
            services.AddTransient<IEmailServices, EmailServices>();
            services.AddTransient<ICompanySetupServices, CompanySetupServices>();
            services.AddTransient<IEmployeeDashboardServices, EmployeeDashboardServices>();
            services.AddTransient<ISubscription, SubscriptionServices>();
            services.AddTransient<ITicketServices, TicketServices>();
            services.AddTransient<IchatService, chatServices>();

            services.AddControllers();
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 3;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
            });

            // For Entity Framework  
            services.AddDbContext<AppDBContext>(options => options.UseSqlServer(Configuration.GetConnectionString("IdentityConnection")));
            services.AddDbContext<HRMContexts>(options => options.UseSqlServer(Configuration.GetConnectionString("IdentityConnection")));
            services.AddMemoryCache();

            // For Identity  
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDBContext>()
                .AddDefaultTokenProviders();

            // Adding Authentication  
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })

            // Adding Jwt Bearer  
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = Configuration["JWT:ValidAudience"],
                    ValidIssuer = Configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))
                };
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            });
            //services.AddSwaggerGen();

            //using session 
            services.AddDistributedMemoryCache(); // Use in-memory cache for session state

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(180); // Set session timeout
                options.Cookie.HttpOnly = true; // Ensure the session cookie is not accessible via JavaScript
                options.Cookie.IsEssential = true; // Make the session cookie essential
            });
            services.AddScoped<SubscriptionActionFilter>();
            //for custom filters
            //services.AddControllersWithViews(options =>
            //{
            //    options.Filters.Add(typeof(SubscriptionActionFilter));
            //});
            //services.AddControllers(options =>
            //{
            //    options.Filters.Add(typeof(SubscriptionActionFilter));
            //});
            services.AddHttpContextAccessor();
            //services.AddTransient<SubscriptionMiddleware>();

            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)  
        {
            app.UseSwagger();
            app.UseDeveloperExceptionPage();
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "/swagger/{documentName}/swagger.json";
            });
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));

            app.Use(async (ctx, next) =>
            {
                await next();
                if (ctx.Response.StatusCode == 204)
                {
                    ctx.Response.ContentLength = 0;
                }
            });
            app.UseCors("AllowAllHeaders");
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
                RequestPath = new PathString("/Resources")
            });
            //if (env.IsDevelopment())
            //{
                
            //}
            app.UseRouting();
            
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
            // app.UseMiddleware<SubscriptionMiddleware>();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ChatHub>("/chatHub"); //this will change before deploying this in a live enviornment
                endpoints.MapControllers();
            });
            app.UseWebSockets();

        }
    }
}
