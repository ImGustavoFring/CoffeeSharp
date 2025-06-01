using System.Linq;
using System.Threading.Tasks;
using CoffeeSharp.Domain.Entities;
using Domain.DTOs;
using Domain.DTOs.Client.Requests;
using Domain.DTOs.Shared;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Logic.BusinessServices.Interfaces;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/clients")]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet]
        public async Task<IActionResult> GetClients(
            [FromQuery] string? telegramId,
            [FromQuery] string? name,
            [FromQuery] int pageIndex = 0,
            [FromQuery] int pageSize = 50)
        {
            var (clients, total) = await _clientService.GetClientsAsync(
                telegramId, name,
                pageIndex, pageSize);

            Response.Headers.Add("X-Total-Count", total.ToString());

            var dtos = clients.Select(client => new ClientDto
            {
                Id = client.Id,
                TelegramId = client.TelegramId,
                Name = client.Name,
                Balance = client.Balance
            });

            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClientById(long id)
        {
            var client = await _clientService.GetClientByIdAsync(id);

            if (client == null)
                return NotFound();

            var dto = new ClientDto
            {
                Id = client.Id,
                TelegramId = client.TelegramId,
                Name = client.Name,
                Balance = client.Balance
            };

            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateClient([FromBody] CreateClientRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var client = new Client
            {
                TelegramId = request.TelegramId,
                Name = request.Name
            };

            var created = await _clientService.CreateClientAsync(client);

            var dto = new ClientDto
            {
                Id = created.Id,
                TelegramId = created.TelegramId,
                Name = created.Name,
                Balance = created.Balance
            };

            return CreatedAtAction(nameof(GetClientById), new { id = dto.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClient(long id,
            [FromBody] UpdateClientRequest request)
        {
            if (id != request.Id)
                ModelState.AddModelError("Id", "URL id does not match request body id.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var client = new Client
            {
                Id = request.Id,
                TelegramId = request.TelegramId,
                Name = request.Name
            };

            var updated = await _clientService.UpdateClientAsync(client);

            var dto = new ClientDto
            {
                Id = updated.Id,
                TelegramId = updated.TelegramId,
                Name = updated.Name,
                Balance = updated.Balance
            };

            return Ok(dto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(long id)
        {
            await _clientService.DeleteClientAsync(id);

            return NoContent();
        }

        [HttpPost("{clientId}/balance/add")]
        [Authorize]
        public async Task<IActionResult> AddBalance(long clientId, 
            [FromBody] AddBalanceRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdHistory = await _clientService.AddBalanceAsync(clientId, request.Amount);

            var dto = new BalanceHistoryDto
            {
                Id = createdHistory.Id,
                ClientId = createdHistory.ClientId,
                Sum = createdHistory.Sum,
                CreatedAt = createdHistory.CreatedAt,
                FinishedAt = createdHistory.FinishedAt,
                BalanceHistoryStatusId = createdHistory.BalanceHistoryStatusId
            };

            return Ok(dto);
        }

        [HttpPost("{clientId}/balance/deduct")]
        [Authorize]
        public async Task<IActionResult> DeductBalance(long clientId, 
            [FromBody] DeductBalanceRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdHistory = await _clientService.DeductBalanceAsync(clientId, request.Amount);

            var dto = new BalanceHistoryDto
            {
                Id = createdHistory.Id,
                ClientId = createdHistory.ClientId,
                Sum = createdHistory.Sum,
                CreatedAt = createdHistory.CreatedAt,
                FinishedAt = createdHistory.FinishedAt,
                BalanceHistoryStatusId = createdHistory.BalanceHistoryStatusId
            };

            return Ok(dto);
        }

        [HttpGet("transactions")]
        [Authorize]
        public async Task<IActionResult> GetClientTransactions(
            [FromQuery] long? clientId,
            [FromQuery] TransactionType transactionType = TransactionType.All,
            [FromQuery] TransactionStatus? transactionStatus = null,
            [FromQuery] DateTime? createdFrom = null,
            [FromQuery] DateTime? createdTo = null,
            [FromQuery] DateTime? finishedFrom = null,
            [FromQuery] DateTime? finishedTo = null,
            [FromQuery] bool orderByNewestFirst = true,
            [FromQuery] int pageIndex = 0,
            [FromQuery] int pageSize = 50)
        {
            var (transactions, total) = await _clientService.GetClientTransactionsAsync(
                clientId,
                transactionType,
                transactionStatus,
                createdFrom,
                createdTo,
                finishedFrom,
                finishedTo,
                orderByNewestFirst,
                pageIndex,
                pageSize);

            Response.Headers.Add("X-Total-Count", total.ToString());

            var dtos = transactions.Select(history => new BalanceHistoryDto
            {
                Id = history.Id,
                ClientId = history.ClientId,
                Sum = history.Sum,
                CreatedAt = history.CreatedAt,
                FinishedAt = history.FinishedAt,
                BalanceHistoryStatusId = history.BalanceHistoryStatusId
            });

            return Ok(dtos);
        }

        [HttpPatch("transactions/{transactionId}/complete")]
        public async Task<IActionResult> CompleteBalanceTransaction(long transactionId)
        {
            var updated = await _clientService.CompletePendingBalanceTransactionAsync(transactionId);

            var dto = new ClientDto
            {
                Id = updated.Id,
                TelegramId = updated.TelegramId,
                Name = updated.Name,
                Balance = updated.Balance
            };

            return Ok(dto);
        }

        [HttpPatch("transactions/{transactionId}/cancel")]
        public async Task<IActionResult> CancelTransaction(long transactionId)
        {
            var updated = await _clientService.CancelTransactionAsync(transactionId);

            var dto = new ClientDto
            {
                Id = updated.Id,
                TelegramId = updated.TelegramId,
                Name = updated.Name,
                Balance = updated.Balance
            };

            return Ok(dto);
        }
    }
}
