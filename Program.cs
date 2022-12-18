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
using MyCarearApi.Services.Chat.Interfaces;
using System.Text.Json;
using FluentValidation.AspNetCore;
using FluentValidation;
using System.Reflection;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite("Data Source = Data.sqlite;");
    //  options.UseInMemoryDatabase("TestDb");
});



builder.Services.AddDbContext<ChatDbContext>(options => options.UseInMemoryDatabase("ChatConnections"));


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
builder.Services.AddScoped<IGetInformationService, GetInformationService>();

builder.Services.AddTransient<ICompanyService, CompanyService>();
builder.Services.AddTransient<IJwtService, JwtService>();
builder.Services.AddTransient<IJobSkillsService, JobSkillsService>();
builder.Services.AddTransient<IJobLanguagesService, JobLanguageService>();
builder.Services.AddTransient<IJobService, JobService>();
builder.Services.AddTransient<IMessageService, MessageService>();
builder.Services.AddTransient<IConnectionService, ConnectionService>();


builder.Services.AddSignalR(options =>
{
    options.MaximumParallelInvocationsPerClient = 5;
    options.MaximumReceiveMessageSize= 512000;
    
    Console.WriteLine("************\n\n\n");
    Console.WriteLine(JsonSerializer.Serialize(options));
}).AddNewtonsoftJsonProtocol(options => options.PayloadSerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

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

AppDbInitialize.Seed(app).Wait();

app.UseHttpsRedirection();
app.UseCors("EnableCORS");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHub<ChatHub>("/chat", options =>
{
    options.WebSockets.CloseTimeout = TimeSpan.FromSeconds(300);
    options.LongPolling.PollTimeout = TimeSpan.FromSeconds(900);
    options.TransportMaxBufferSize = 512000;
    options.TransportSendTimeout = TimeSpan.FromSeconds(150);
    Console.WriteLine(options.Transports);
    Console.WriteLine("*********************\n\n\nOptions");
    Console.WriteLine(JsonSerializer.Serialize(options));
    
});


app.Run();
