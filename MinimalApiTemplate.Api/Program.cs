using Carter;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MinimalApiTemplate.Api.Database;
using MinimalApiTemplate.Middleware;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddDbContext<DataContext>(opt =>
        opt.UseNpgsql(builder.Configuration.GetConnectionString("MinimalApiTemplate")).EnableDetailedErrors());

Assembly assembly = typeof(Program).Assembly;

builder.Services.AddMediatR(config => config.RegisterServicesFromAssemblies(assembly));

builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddCarter();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DataContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapCarter();

//CreatePost.MapEndpoint(app);
app.UseMiddleware<ErrorHandlingMiddleware>();

// app.UseHttpsRedirection();

app.Run();

