using Banking.Account.Consumer.Domain.Common;

namespace Banking.Account.Consumer.Domain;

public class BankAccount: BaseDomainModel
{
    public string Identifier { get; set; } = string.Empty;
    public string AccountHolder { get; set; } = String.Empty;
    public DateTime CreationDate { get; set; }
    public string AccountType { get; set; } = string.Empty;
    public double Balance { get; set; }
}