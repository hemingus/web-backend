using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Cosmos;
using web_backend.DbContexts;
using web_backend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IRepository, CosmosRepository>();
builder.Services.AddDbContextFactory<CosmosContext>(optionsBuilder => { 
    optionsBuilder.UseCosmos(
        connectionString: builder.Configuration["PROD_COSMOS_CONNECTION_STRING"],
        databaseName: builder.Configuration["PROD_COSMOS_DATABASE"]
        );
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

//app.MapControllers();

app.UseEndpoints(endpoints => 
    endpoints.MapControllers());

app.Run();
