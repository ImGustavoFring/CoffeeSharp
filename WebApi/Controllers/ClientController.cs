using System.Linq;
using System.Threading.Tasks;
using CoffeeSharp.Domain.Entities;
using Domain.DTOs;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using WebApi.Logic.Services.Interfaces;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/client")]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllClients()
        {
            var clients = await _clientService.GetAllClientsAsync();
            var clientDtos = clients.Select(c => new ClientDto
            {
                Id = c.Id,
                TelegramId = c.TelegramId,
                Name = c.Name,
                Balance = c.Balance
            });
            return Ok(clientDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClientById(long id)
        {
            var client = await _clientService.GetClientByIdAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            var clientDto = new ClientDto
            {
                Id = client.Id,
                TelegramId = client.TelegramId,
                Name = client.Name,
                Balance = client.Balance
            };
            return Ok(clientDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateClient([FromBody] CreateClientRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var client = new Client
            {
                TelegramId = request.TelegramId,
                Name = request.Name
            };

            var created = await _clientService.CreateClientAsync(client);
            var clientDto = new ClientDto
            {
                Id = created.Id,
                TelegramId = created.TelegramId,
                Name = created.Name,
                Balance = created.Balance
            };
            return CreatedAtAction(nameof(GetClientById), new { id = clientDto.Id }, clientDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClient(long id, [FromBody] UpdateClientRequest request)
        {
            if (id != request.Id)
            {
                ModelState.AddModelError("Id", "URL id does not match request body id.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var client = new Client
            {
                Id = request.Id,
                TelegramId = request.TelegramId,
                Name = request.Name
            };

            var updated = await _clientService.UpdateClientAsync(client);
            var clientDto = new ClientDto
            {
                Id = updated.Id,
                TelegramId = updated.TelegramId,
                Name = updated.Name,
                Balance = updated.Balance
            };
            return Ok(clientDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(long id)
        {
            await _clientService.DeleteClientAsync(id);
            return NoContent();
        }

        [HttpPost("{clientId}/balance/add")]
        public async Task<IActionResult> AddBalance(long clientId, [FromBody] AddBalanceRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updated = await _clientService.AddBalanceAsync(clientId, request.Amount);
            var clientDto = new ClientDto
            {
                Id = updated.Id,
                TelegramId = updated.TelegramId,
                Name = updated.Name,
                Balance = updated.Balance
            };
            return Ok(clientDto);
        }

        [HttpPost("{clientId}/balance/deduct")]
        public async Task<IActionResult> DeductBalance(long clientId, [FromBody] DeductBalanceRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updated = await _clientService.DeductBalanceAsync(clientId, request.Amount);
                var clientDto = new ClientDto
                {
                    Id = updated.Id,
                    TelegramId = updated.TelegramId,
                    Name = updated.Name,
                    Balance = updated.Balance
                };
                return Ok(clientDto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{clientId}/transactions")]
        public async Task<IActionResult> GetClientTransactions(
            long clientId,
            [FromQuery] TransactionType transactionType = TransactionType.All,
            [FromQuery] bool orderByNewestFirst = true)
        {
            var transactions = await _clientService.GetClientTransactionsAsync(clientId, orderByNewestFirst, transactionType);
            var transactionDtos = transactions.Select(bh => new BalanceHistoryDto
            {
                Id = bh.Id,
                ClientId = bh.ClientId,
                Sum = bh.Sum,
                CreatedAt = bh.CreatedAt,
                FinishedAt = bh.FinishedAt,
                BalanceHistoryStatusId = bh.BalanceHistoryStatusId
            });
            return Ok(transactionDtos);
        }

        [HttpPost("transaction/cancel")]
        public async Task<IActionResult> CancelTransaction([FromBody] CancelTransactionRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var previousBalance = await _clientService.CancelTransactionAsync(request.TransactionId);
                var response = new CancelTransactionResponse
                {
                    PreviousBalance = previousBalance
                };
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}


