using System.Net;
using Banking.Account.Command.Application.Features.BankAccount.Commands.CloseAccount;
using Banking.Account.Command.Application.Features.BankAccount.Commands.DepositFunds;
using Banking.Account.Command.Application.Features.BankAccount.Commands.OpenAccount;
using Banking.Account.Command.Application.Features.BankAccount.Commands.ReplayAccount;
using Banking.Account.Command.Application.Features.BankAccount.Commands.WithdrawFunds;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Banking.Account.Command.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BankAccountController : ControllerBase
{
    private readonly IMediator _mediator;

    public BankAccountController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("OpenAccount", Name = "OpenAccount")]
    [ProducesResponseType((int) HttpStatusCode.OK)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> OpenAccount([FromBody] OpenAccountCommand command)
    {
        var id = Guid.NewGuid().ToString();
        command.Id = id;
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("CloseAccount/{id}", Name = "CloseAccount")]
    [ProducesResponseType((int) HttpStatusCode.OK)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> CloseAccount(string id)
    {
        var command = new CloseAccountCommand {Id = id};
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    
    [HttpPut("DepositFund/{id}", Name = "DepositFund")]
    [ProducesResponseType((int) HttpStatusCode.OK)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> DepositFund(string id,[FromBody] DepositFundsCommand command)
    {
        command.Id = id;
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    
    [HttpPut("WithDrawnFund/{id}", Name = "WithDrawnFund")]
    [ProducesResponseType((int) HttpStatusCode.OK)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> WithDrawnFund(string id,[FromBody] WithdrawFundsCommand command)
    {
        command.Id = id;
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    
    [HttpPost("ReplayAccount/{id}", Name = "ReplayAccount")]
    [ProducesResponseType((int) HttpStatusCode.OK)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> ReplayAccount(string id)
    {
        var command = new ReplayByAggregateIdentifierCommand() {AggregateId = id};
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}