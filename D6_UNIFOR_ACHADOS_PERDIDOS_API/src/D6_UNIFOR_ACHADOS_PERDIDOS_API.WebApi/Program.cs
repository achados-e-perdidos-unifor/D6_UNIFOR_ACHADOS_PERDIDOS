using D6_UNIFOR_ACHADOS_PERDIDOS_API.Application.Interfaces;
using D6_UNIFOR_ACHADOS_PERDIDOS_API.Application.Services;
using D6_UNIFOR_ACHADOS_PERDIDOS_API.Infrastructure.Data;
using D6_UNIFOR_ACHADOS_PERDIDOS_API.Infrastructure.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();

builder.Services.AddScoped<IItemRepository, ItemRepository>();

builder.Services.AddScoped<IItemService, ItemService>();

var app = builder.Build();

app.MapOpenApi();

app.UseSwagger();

app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapControllers();

app.MapGet("/ping", () => "pong");

app.Run();