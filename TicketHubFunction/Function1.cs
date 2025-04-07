using System;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Data.SqlClient;

namespace TicketHubFunction
{
    public class Function1
    {
        private readonly ILogger<Function1> _logger;

        public Function1(ILogger<Function1> logger)
        {
            _logger = logger;
        }

        [Function(nameof(Function1))]
        public async Task Run([QueueTrigger("tickethub", Connection = "AzureWebJobsStorage")] QueueMessage message)
        {
            _logger.LogInformation($"C# Queue trigger function processed: {message.MessageText}");

            string json = message.MessageText;

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
           

            //Deserizalize the message JSON into a tickethub object
            Tickethub? tickethub = JsonSerializer.Deserialize<Tickethub>(json, options);

            if(tickethub == null)
            {
                _logger.LogError("Failed to deserialize the message into a Tickethub object");
                return;
            }
           

            _logger.LogInformation($"Hello {tickethub.Name}");

            //
            //Add ticketub into database
            //

            // get connection string from app settings
            string? connectionString = Environment.GetEnvironmentVariable("SqlConnectionString");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("SQL connection string is not set in the environment variables.");
            }


            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync(); 

                var query = "INSERT INTO tickethub (ConcertId, Email, Name, Phone, Quantity, CreditCard, Expiration, SecurityCode, Address, Province, PostalCode,City, Country) VALUES (@ConcertId, @Email, @Name, @Phone, @Quantity, @CreditCard, @Expiration, @SecurityCode, @Address, @Province, @PostalCode,@City, @Country);";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ConcertId", tickethub.ConcertId);
                    cmd.Parameters.AddWithValue("@Email", tickethub.Email);
                    cmd.Parameters.AddWithValue("@Name", tickethub.Name);
                    cmd.Parameters.AddWithValue("@Phone", tickethub.Phone);
                    cmd.Parameters.AddWithValue("@Quantity", tickethub.Quantity);
                    cmd.Parameters.AddWithValue("@CreditCard", tickethub.CreditCard);
                    cmd.Parameters.AddWithValue("@Expiration", tickethub.Expiration);
                    cmd.Parameters.AddWithValue("@SecurityCode", tickethub.SecurityCode);
                    cmd.Parameters.AddWithValue("@Address", tickethub.Address);
                    cmd.Parameters.AddWithValue("@Province", tickethub.Province);
                    cmd.Parameters.AddWithValue("@PostalCode", tickethub.PostalCode);
                    cmd.Parameters.AddWithValue("@City", tickethub.City);
                    cmd.Parameters.AddWithValue("@Country", tickethub.Country);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
            _logger.LogInformation("Contact added to database");
        }
    }
}
