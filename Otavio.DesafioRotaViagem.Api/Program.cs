using Microsoft.EntityFrameworkCore;
using Otavio.DesafioRotaViagem.Api.Contexts;
using Otavio.DesafioRotaViagem.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
     {
         builder.AllowAnyOrigin()
             .AllowAnyMethod()
             .AllowAnyHeader();
     });
});

builder.Services.AddScoped<IRotaService, RotaService>();
builder.Services.AddDbContext<RotaDbContext>(options =>
{
    options.UseInMemoryDatabase("RotaViagem");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
