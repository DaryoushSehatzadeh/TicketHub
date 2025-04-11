using System;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace FunctionApp1
{
    public class Function1
    {
        private readonly ILogger<Function1> _logger;

        public Function1(ILogger<Function1> logger)
        {
            _logger = logger;
        }

        [Function(nameof(Function1))]
        async public Task Run([QueueTrigger("tickethub", Connection = "AzureWebJobsStorage")] QueueMessage response)
        {
            _logger.LogInformation($"C# Queue trigger function processed: {response.MessageText}");

            var order = System.Text.Json.JsonSerializer.Deserialize<Order>(response.MessageText);

            // get connection string from app settings
            string? connectionString = Environment.GetEnvironmentVariable("SqlConnectionString");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("SQL connection string is not set in the environment variables.");
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync(); // Note the ASYNC

                var query = @"
                    INSERT INTO dbo.ConcertOrders 
                    (
                        ConcertId, Email, Name, Phone, Quantity, CreditCard,
                        Expiration, SecurityCode, Address, City, Province,
                        PostalCode, Country
                    )
                    VALUES 
                    (
                        @concertId, @email, @name, @phone, @quantity, @creditCard,
                        @expiration, @securityCode, @address, @city, @province,
                        @postalCode, @country
                    )";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@concertId", order.ConcertId);
                    cmd.Parameters.AddWithValue("@email", order.Email);
                    cmd.Parameters.AddWithValue("@name", order.Name);
                    cmd.Parameters.AddWithValue("@phone", order.Phone);
                    cmd.Parameters.AddWithValue("@quantity", order.Quantity);
                    cmd.Parameters.AddWithValue("@creditCard", order.CreditCard);
                    cmd.Parameters.AddWithValue("@expiration", order.Expiration);
                    cmd.Parameters.AddWithValue("@securityCode", order.SecurityCode);
                    cmd.Parameters.AddWithValue("@address", order.Address);
                    cmd.Parameters.AddWithValue("@city", order.City);
                    cmd.Parameters.AddWithValue("@province", order.Province);
                    cmd.Parameters.AddWithValue("@postalCode", order.PostalCode);
                    cmd.Parameters.AddWithValue("@country", order.Country);


                    await cmd.ExecuteNonQueryAsync();
                }
            }

        }
    }
}
