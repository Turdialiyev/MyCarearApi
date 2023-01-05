#pragma warning disable
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
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
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

    //options.UseSqlServer("Data Source=(localDb)\\MSSQLLocalDB; Database=MyCareerDatabase;");
    //options.UseSqlite("Data Source = Data.sqlite;");

    options.UseInMemoryDatabase("TestDb");
});

builder.Services.AddDbContext<ChatDbContext>(options => options.UseInMemoryDatabase("ChatDatabase"));

builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789._@+!#$%";
    options.SignIn.RequireConfirmedEmail = false;
}).AddEntityFrameworkStores<AppDbContext>().AddTokenProvider<TwoFactorTokenProvider<AppUser>>("Default");

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    Console.WriteLine("JwtBearerDefaults.AuthenticationScheme: " + JwtBearerDefaults.AuthenticationScheme);
}).AddJwtBearer(options =>
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
/*.AddGoogle(googleOptions =>
{
    googleOptions.ClientId = builder.Configuration.GetSection("Google")["client_id"];
    googleOptions.ClientSecret = builder.Configuration.GetSection("Google")["client_secret"];
    googleOptions.CallbackPath = "/signin-google";
    googleOptions.SignInScheme = IdentityConstants.ExternalScheme;
    Console.WriteLine("\n\n****IdentityConstants.ExternalScheme : " + IdentityConstants.ExternalScheme + "****\n\n");
}).AddOAuth("github", github =>
{
    github.ClientId = "c48925d14cde0345c9c5";
    github.ClientSecret = "55229101dd1a1ba516121b7ae7d3ab79d840f269";
    github.AuthorizationEndpoint = "https://github.com/login/oauth/authorize";
    github.CallbackPath = "/github-cb";
}).AddFacebook(facebook =>
{

})*/;



builder.Services.AddAuthorization(options =>
{
});
builder.Services.AddScoped<IGetInformationService, GetInformationService>();
builder.Services.AddScoped<IPortfolioService, PortfolioService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
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
builder.Services.AddTransient<IConnectionService, ConnectionService>();
builder.Services.AddTransient<IMailSender, MailSender>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddTransient<IUserIdProvider, UserIdProvider>();
builder.Services.AddScoped<IGetInformationService, GetInformationService>();
builder.Services.AddScoped<IOfferService, OfferService>();
builder.Services.AddSingleton<IUserTwoFactorTokenProvider<AppUser>, TwoFactorTokenProvider<AppUser>>();

builder.Services.AddSignalR();

builder.Services.AddCors(x => x.AddPolicy("EnableCORS", w => w.AllowAnyOrigin()
                                                              .AllowAnyHeader()
                                                              .SetIsOriginAllowed((x) => true)
                                                              .AllowAnyMethod()));

var app = builder.Build();



// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI();
// }

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
