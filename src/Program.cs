using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using web_backend.DbContexts;
using web_backend.Entities;
using web_backend.Services;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy
            .WithOrigins("http://localhost:3000", "https://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// Choose Cosmos connection based on environment
string cosmosConnectionString;
string cosmosDatabase;

if (builder.Environment.IsDevelopment())
{
    // Development: prefer explicit DEV_* keys (emulator)
    cosmosConnectionString = builder.Configuration["DEV_COSMOS_CONNECTION_STRING"];
    cosmosDatabase = builder.Configuration["DEV_COSMOS_DATABASE"];
}
else
{
    // Production: prefer environment variables, then PROD_* keys
    cosmosConnectionString = builder.Configuration["PROD_COSMOS_CONNECTION_STRING"];
    cosmosDatabase = builder.Configuration["PROD_COSMOS_DATABASE"];
}

if (string.IsNullOrEmpty(cosmosConnectionString))
{
    throw new InvalidOperationException("Cosmos DB connection string not configured. Set DEV_COSMOS_CONNECTION_STRING (development) or COSMOS_CONNECTION_STRING/PROD_COSMOS_CONNECTION_STRING (production).");
}

// Register DbContext
builder.Services.AddDbContext<CosmosContext>(options =>
{
    options.UseCosmos(
        connectionString: cosmosConnectionString,
        databaseName: cosmosDatabase
    );
});

// Register current user service and IHttpContextAccessor
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

// Register password hasher for User
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

// Register repositories
builder.Services.AddScoped<ITaskEntityRepository, TaskEntityRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();

// JWT authentication configuration
var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

if (!string.IsNullOrEmpty(jwtKey))
{
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = true;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ValidateIssuer = !string.IsNullOrEmpty(jwtIssuer),
            ValidIssuer = jwtIssuer,
            ValidateAudience = !string.IsNullOrEmpty(jwtAudience),
            ValidAudience = jwtAudience,
            ValidateLifetime = true
        };

        options.Events = new JwtBearerEvents
        {
            OnChallenge = context =>
            {
                context.HandleResponse();
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";
                return context.Response.WriteAsync("{\"error\":\"Unauthorized\"}");
            },
            OnForbidden = context =>
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                context.Response.ContentType = "application/json";
                return context.Response.WriteAsync("{\"error\":\"Forbidden\"}");
            }
        };
    });
}

// Require authentication for all endpoints by default
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

var app = builder.Build();

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

// CORS must be placed before auth and before MapControllers
app.UseCors("FrontendPolicy");

app.UseAuthentication();
app.UseAuthorization();

// JSON status code pages for 401/403
app.UseStatusCodePages(async ctx =>
{
    var resp = ctx.HttpContext.Response;
    if (resp.StatusCode == StatusCodes.Status401Unauthorized)
    {
        resp.ContentType = "application/json";
        await resp.WriteAsync("{\"error\":\"Unauthorized\"}");
    }
    else if (resp.StatusCode == StatusCodes.Status403Forbidden)
    {
        resp.ContentType = "application/json";
        await resp.WriteAsync("{\"error\":\"Forbidden\"}");
    }
});

app.MapControllers();

app.Run();
