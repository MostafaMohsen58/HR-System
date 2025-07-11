using System;
using System.Text;
using System.Text.Json.Serialization;
using Hangfire;
using HRManagementSystem.BL.Interfaces;
using HRManagementSystem.BL.Mapping;
using HRManagementSystem.BL.Services;
using HRManagementSystem.DAL.Data.Context;
using HRManagementSystem.DAL.Interfaces;
using HRManagementSystem.DAL.Models;
using HRManagementSystem.DAL.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;


namespace HRManagementSystem.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string myPolicy = "";
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddDbContext<HRContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("HRDatabase")));

            builder.Services.AddIdentity<ApplicationUser, Role>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
            }).AddEntityFrameworkStores<HRContext>();



            builder.Services.AddAuthentication(option => option.DefaultAuthenticateScheme = "mySchema")
                .AddJwtBearer("mySchema", option =>
                {
                    var key = "ylsOukyEWbHPzMPH4eT0fHvD2JWagme2sQWBW+m+P38=";
                    var secertkey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));

                    option.TokenValidationParameters = new TokenValidationParameters()
                    {
                        IssuerSigningKey = secertkey,
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            builder.Services.AddScoped<IUserService, UserService>();
            //builder.Services.AddScoped<IRoleService, RoleService>();


            builder.Services.AddAutoMapper(typeof(mappingConfig));

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("myPolicy",
                    builder =>
                    {
                        //builder.WithOrigins("https://localhost:7053")
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });


            builder.Services.AddScoped<IOfficialHolidayRepository, OfficialHolidayRepository>();
            builder.Services.AddScoped<IOfficialHolidayService, OfficialHolidayService>();

            builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            builder.Services.AddScoped<IDepartmentService, DepartmentService>();
            builder.Services.AddScoped<IPermissionService, PermissionService>();
            builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();


            builder.Services.AddScoped<IAttendanceRepository, AttendanceRepository>();
            builder.Services.AddScoped<IAttendanceService, AttendanceService>();

            builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            builder.Services.AddScoped<IEmployeeService, EmployeeService>();

            builder.Services.AddScoped<IPayRollRepository, PayRollRepository>();
            builder.Services.AddScoped<IPayRollService, PayRollService>();

            builder.Services.AddHangfire(configuration =>
            {
                configuration.UseSqlServerStorage(builder.Configuration.GetConnectionString("HRDatabase"));
            });
            builder.Services.AddHangfireServer();


            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseCors("myPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseHangfireDashboard("/dashborad");

            using (var scope = app.Services.CreateScope())
            {
                var jobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();

                var egyptTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");

                jobManager.AddOrUpdate<IPayRollService>(
                    "calculate-payroll",
                    service => service.FinalizeHolidaySalariesAsync(DateTime.Now.Month, DateTime.Now.Year),
                    Cron.Daily(23,59),
                    options: new RecurringJobOptions
                    {
                        TimeZone = egyptTimeZone
                    }
                );
            }
            app.MapControllers();

            app.Run();
        }
    }
}
