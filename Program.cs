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
using Microsoft.AspNetCore.SignalR;
using MyCarearApi.Services.Chat;
using MyCarearApi.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    // options.UseSqlite("Data Source = Data.db;");
     options.UseInMemoryDatabase("TestDb");
});

builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789._@+!#$%";
}).AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddAuthentication(opt =>
{
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
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:JwtSecretKey"]))
        };
    });
builder.Services.AddSingleton<IUserIdProvider, UserIdProvider>();
builder.Services.AddTransient<IJwtService, JwtService>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IFileHelper, FileHelper>();
builder.Services.AddScoped<IFreelancerService, FreelancerService>();
builder.Services.AddScoped<IEducationService, EducationService>();
builder.Services.AddScoped<IExperienceService, ExperienceService>();
builder.Services.AddScoped<ILanguageService, LanguageService>();
builder.Services.AddTransient<IContractService, ContractService>();
builder.Services.AddTransient<ICompanyService, CompanyService>();
builder.Services.AddTransient<IJwtService, JwtService>();
builder.Services.AddTransient<IJobSkillsService, JobSkillsService>();
builder.Services.AddTransient<IJobLanguagesService, JobLanguageService>();
builder.Services.AddTransient<IJobService, JobService>();
builder.Services.AddTransient<IMessageService, MessageService>();

builder.Services.AddSignalR();

builder.Services.AddCors(x => x.AddPolicy("EnableCORS", w => w.AllowAnyOrigin()
                                                              .AllowAnyHeader()
                                                              .AllowAnyMethod()));

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
app.UseCors("EnableCORS");
app.UseAuthentication();
app.UseAuthorization();
app.MapHub<ChatHub>("/chat");
app.MapControllers();

AppDbInitialize.Seed(app);

app.Run();
