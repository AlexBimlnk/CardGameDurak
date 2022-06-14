using Serilog;
using CardGameDurak.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddGameCoorditanor();
builder.Host.UseSerilog((ctx, cfg) => cfg.ReadFrom.Configuration(ctx.Configuration));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();



app.UseAuthorization();

app.MapControllers();

app.Run();
