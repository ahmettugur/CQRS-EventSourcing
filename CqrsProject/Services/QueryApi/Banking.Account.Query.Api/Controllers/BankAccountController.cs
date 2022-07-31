using System.Net;
using Banking.Account.Query.Application.Features.BankAccounts.Queries.FindAccountById;
using Banking.Account.Query.Application.Features.BankAccounts.Queries.FindAllAccounts;
using Banking.Account.Query.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Banking.Account.Query.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BankAccountController : ControllerBase
{
    private readonly IMediator _mediator;

    public BankAccountController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("AllAccounts", Name = "GetAllAccounts")]
    [ProducesResponseType(typeof(IEnumerable<BankAccount>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetAllAccounts()
    {
        var result = await _mediator.Send(new FindAllAccountsQuery());
        return Ok(result);        
    }

    [HttpGet("AccountByIdentifier/{id}", Name = "GetAccountByIdentifier")]
    [ProducesResponseType(typeof(IEnumerable<BankAccount>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetAccountByIdentifier(string id)
    {
        var result = await _mediator.Send(new FindAccountByIdQuery {Identifier = id});
        return Ok(result);
    }
}