using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using MyCarearApi.Data;
using MyCarearApi.Entities;
using MyCarearApi.Repositories;
using MyCarearApi.Services;
using MyCarearApi.Services.JwtServices.Interfaces;
using MyCarearApi.Services.JwtServices;
using MyCarearApi.Services.JobServices.Interfaces;
using MyCarearApi.Services.JobServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    //options.UseSqlite("Data Source = Data.db;");
     options.UseInMemoryDatabase("TestDb");
});

builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IFileHelper, FileHelper>();

builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = false;
}).AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddAuthentication(opt => {
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    Console.WriteLine("JwtBearerDefaults.AuthenticationScheme: " + JwtBearerDefaults.AuthenticationScheme);
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:JwtSecretKey"]))
        };
    });

builder.Services.AddTransient<IJwtService, JwtService>();


builder.Services.AddTransient<ICompanyService, CompanyService>();
builder.Services.AddTransient<IJwtService, JwtService>();
builder.Services.AddTransient<IJobSkillsService, JobSkillsService>();
builder.Services.AddTransient<IJobLanguagesService, JobLanguageService>();
builder.Services.AddTransient<IJobService, JobService>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")),
    RequestPath = ""
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
