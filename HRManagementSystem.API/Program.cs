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
using System.Text;

namespace HRManagementSystem.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string myPolicy = "";
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
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

            builder.Services.AddScoped<ISettingRepository, SettingRepository>();
            builder.Services.AddScoped<ISettingService, SettingService>();


            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwaggerUI(op => op.SwaggerEndpoint("/openapi/v1.json", "v1"));
            }

            app.UseHttpsRedirection();

            app.UseCors("myPolicy");

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
