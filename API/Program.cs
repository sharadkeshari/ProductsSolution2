using API.Filters;
using API.Middleware;
using Application.Interfaces;
using Application.Services;
using Application.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IItemService, ItemService>();
// Add controllers
builder.Services.AddControllers();

// Configure FluentValidation for automatic validation
builder.Services.AddFluentValidationAutoValidation(); // Enables automatic validation of incoming requests.
builder.Services.AddFluentValidationClientsideAdapters();    // Enables client-side adapters if needed.

builder.Services.AddValidatorsFromAssemblyContaining<CreateProductDtoValidator>();
// Configure Swagger and have it integrate with the versioned API explorer.
builder.Services.AddSwaggerGen(options =>
{
    // options.SwaggerDoc("v1", new OpenApiInfo { Title = "Products API", Version = "v1" });

    // Add the Bearer security definition
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter 'Bearer' [space] and then your valid token. Example: \"Bearer abcdef12345\"",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    // Add a global security requirement so that all endpoints require the token unless specified otherwise.
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new List<string>() // No specific scopes required for JWT Bearer
        }
    });
    options.OperationFilter<AddHeaderParameterOperationFilter>(); // Add custom header parameter to all operations
});

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("x-api-version"),
        new MediaTypeApiVersionReader("x-api-version"),
        new QueryStringApiVersionReader("x-api-version")
    );
});
builder.Services.AddVersionedApiExplorer(options =>
{
    // Define the format of the API version in generated SwaggerDoc name.
    options.GroupNameFormat = "'v'VVV";
    // Substitute the version in the URL with the actual API version.
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
// ---------------------------

builder.Services.AddAuthorization();

var app = builder.Build();
app.UseMiddleware<ErrorHandlingMiddleware>();
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}
// Configure the HTTP request pipeline.
//if (app.Environment.IsProduction() || app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Products API V1");
});
//}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
