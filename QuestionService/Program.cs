using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.AddServiceDefaults();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();  // exposes /openapi/v1.json

    // // NOTE:  This section describes how to add Swagger UI
    // // Step 1:  Install NuGet Swashbuckle.AspNetCore.SwaggerUI
    // // Step 2:  Add the follwoing statement here...
    // app.UserSwaggerUI(options =>
    // {
    //      options.SwaggerEndpoint("/openapi/v1.json","v1");
    // });
    // // Step 3:  Browse to https://localhost:<port>/swagger

    // NOTE:  This section describes how to add Scalar (Microsoft recommended OpenAPI UI)
    // Step 1:  Install NuGet Scalar.AspNetCore
    // Step 2:  Add the follwoing statement here...
    app.MapScalarApiReference();
    // Step 3:  Browse to https://localhost:<port>/scalar
}

app.UseAuthorization();

app.MapControllers();

app.MapDefaultEndpoints();

app.Run();
