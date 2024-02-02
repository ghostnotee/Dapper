using Dapper;
using Microsoft.Data.SqlClient;
using Web.Api.Models;
using Web.Api.Services;

namespace Web.Api.EndPoints;

public static class CustomerEndpoints
{
    public static void MapCustomerEndPoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("customers");
        
        group.MapGet("", async (SqlConnectionFactory sqlConnectionFactory) =>
        {
            await using var connection = sqlConnectionFactory.Create();
            const string sql = "SELECT Id, FirstName, LastName, Email, DateOfBirth FROM Customers";
            var customers = await connection.QueryAsync<Customer>(sql);
            return Results.Ok(customers);
        });

        group.MapGet("{id}", async (int id, SqlConnectionFactory sqlConnectionFactory) =>
        {
            await using var connection = sqlConnectionFactory.Create();
            const string sql = "select * from Customers where id = @CustomerId";
            var customer = await connection.QuerySingleOrDefaultAsync<Customer>(sql, new {CustomerId = id});
            return customer is not null ? Results.Ok(customer) : Results.NotFound();
        });

        group.MapPost("", async (Customer customer, SqlConnectionFactory sqlConnectionFactory) =>
        {
            await using var connection = sqlConnectionFactory.Create();
            const string sql = """
                                insert into Customers (FirstName, LastName, Email, DateOfBirth)
                                values (@FirstName, @LastName, @Email, @DateOfBirth)
                               """;
            await connection.ExecuteAsync(sql, customer);
            return Results.Ok();
        });

        group.MapPut("{id}", async (string id, Customer customer, SqlConnectionFactory sqlConnectionFactory) =>
        {
            await using var connection = sqlConnectionFactory.Create();
            const string sql = """
                               update Customers
                               set FirstName = @FirstName, LastName = @LastName, Email = @Email, DateOfBirth = @DateOfBirth
                               where id = @id
                               """;
            await connection.ExecuteAsync(sql, customer);
            return Results.NoContent();
        });

        group.MapDelete("{id}", async (int id, SqlConnectionFactory sqlConnectionFactory) =>
        {
            await using var connection = sqlConnectionFactory.Create();
            const string sql = "Delete from Customers Where Id = @id";
            await connection.ExecuteAsync(sql, new { Id = id });
            return Results.NoContent();
        });
    }
}