namespace Banking.Account.Command.Application.Features.BankAccount.Commands.OpenAccount;

public class OpenAccountRequest
{
    public string AccountHolder { get; set; } = string.Empty;
    public string AccountType { get; set; } = string.Empty;
    public double OpeningBalance { get; set; }
}