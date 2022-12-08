using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using MyCarearApi.Data;
using MyCarearApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite("Data Source = Data.db;");
});

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
