using Microsoft.AspNetCore.OData;
using Microsoft.OData.ModelBuilder;
using TodoApi.Models;
using TodoApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Build the OData EDM model with camelCase naming
var modelBuilder = new ODataConventionModelBuilder();
modelBuilder.EnableLowerCamelCase();
modelBuilder.EntitySet<TodoItem>("TodoItems");
var edmModel = modelBuilder.GetEdmModel();

// Add services
builder.Services.AddSingleton<JsonTodoRepository>();
builder.Services.AddControllers().AddOData(options =>
    options
        .Select()
        .Filter()
        .OrderBy()
        .SetMaxTop(100)
        .Count()
        .Expand()
        .AddRouteComponents("odata", edmModel));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDev", policy =>
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

app.UseCors("AllowAngularDev");
app.MapControllers();

app.Run();

