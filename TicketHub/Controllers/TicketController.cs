using Azure.Storage.Queues;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TicketHub.Models;

namespace TicketHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ILogger<TicketController> _logger;
        private readonly IConfiguration _configuration;

        // Constructor to inject logger and configuration
        public TicketController(ILogger<TicketController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        // Simple GET method returning a message
        [HttpGet]
        public ActionResult<string> Get()
        {
            return "Welcome to the TicketHub API!";
        }

        // POST method to accept a Ticket and push it to Azure Queue
        [HttpPost("newticket")]
        public async Task<IActionResult> CreateTicket(Ticket ticket)
        {
            // Validate the ticket model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Fetch Azure storage connection string from the configuration
            string queueName = "tickethub";
            string? storageConnectionString = _configuration["AzureStorageConnectionString"];

            // Check if the connection string is missing or empty
            if (string.IsNullOrEmpty(storageConnectionString))
            {
                return BadRequest("Azure Storage connection string is not configured.");
            }

            try
            {
                // Create a QueueClient using the connection string and queue name
                var queueClient = new QueueClient(storageConnectionString, queueName);

                // Serialize the Ticket object to JSON
                string ticketMessage = JsonSerializer.Serialize(ticket);

                // Enqueue the message to Azure Queue
                var encodedMessage = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(ticketMessage));
                await queueClient.SendMessageAsync(encodedMessage);

                // Return a success response with the ticket
                return CreatedAtAction(nameof(CreateTicket), new { id = ticket.concertId }, ticket);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error sending message to Azure Queue: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
            }
        }
    }
}
