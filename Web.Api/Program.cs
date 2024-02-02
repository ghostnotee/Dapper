using Dapper;
using Microsoft.Data.SqlClient;
using Web.Api.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("customers", async (IConfiguration configuration) =>
{
    var connectionString = configuration.GetConnectionString("DefaultConnection")!;
    using var connection = new SqlConnection(connectionString);
    const string sql = "SELECT Id, FirstName, LastName, Email, DateOfBirth FROM Customers";
    var customers = await connection.QueryAsync<Customer>(sql);
    return Results.Ok(customers);
});

app.Run();