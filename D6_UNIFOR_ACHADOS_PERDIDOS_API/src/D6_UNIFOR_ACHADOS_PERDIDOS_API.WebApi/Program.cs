using D6_UNIFOR_ACHADOS_PERDIDOS_API.Application.Interfaces;
using D6_UNIFOR_ACHADOS_PERDIDOS_API.Application.Services;
using D6_UNIFOR_ACHADOS_PERDIDOS_API.Infrastructure.Data;
using D6_UNIFOR_ACHADOS_PERDIDOS_API.Infrastructure.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

builder.Services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();
builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped<IItemService, ItemService>();

var app = builder.Build();

app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.MapControllers();
app.MapGet("/Ping", () => "Pong");
app.MapHealthChecks("/health");

app.Run();