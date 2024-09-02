using NetDelegatesApp;


Account account = new(1000);
account.Handler += AccountOperationHandler;


//account.Handler += PrintRedMessage;
//account.Put(500);
//account.Handler -= PrintRedMessage;
//account.Take(700);

//account.Handler += (message) =>
//{
//    Console.ForegroundColor = ConsoleColor.Green;
//    Console.WriteLine(message);
//};
//account.Put(500);

//account.Handler -= (message) =>
//{
//    Console.ForegroundColor = ConsoleColor.Green;
//    Console.WriteLine(message);
//};
//account.Put(700);

//void PrintRedMessage(string message)
//{
//    Console.ForegroundColor = ConsoleColor.Red;
//    Console.WriteLine(message);
//}

void AccountOperationHandler(object sender, AccountEventArgs e)
{
    string message = e.Operation switch
    {
        AccountOperation.Put => $"Put {e.Amount}. Total: {e.AccountAmount}",
        AccountOperation.Take => $"Take {e.Amount}. Total: {e.AccountAmount}",
        AccountOperation.None => $"None. {e.Amount}. Total: {e.AccountAmount}",
    };
    Console.WriteLine(message);
}


delegate void AccountGoodHandler(object sender, AccountEventArgs e);

enum AccountOperation
{
    Put,
    Take,
    None
}

class AccountEventArgs : EventArgs
{
    public int Amount { get; set; }
    
    public int AccountAmount { get; set; }

    public AccountOperation Operation { get; set; }

    public AccountEventArgs(int amount, int accountAmount, AccountOperation operation)
    {
        Amount = amount;
        AccountAmount = accountAmount;
        Operation = operation;
    }
}

class Account
{
    AccountGoodHandler? handler;

    public event AccountGoodHandler? Handler
    {
        add
        {
            handler += value;
        }
        remove
        {
            handler -= value;
        }
    }

    public int Amount { get; set; }

    public Account(int amount = 0) => Amount = amount;

    public void Put(int amount)
    {
        Amount += amount;
        //handler?.Invoke($"To account put {amount} rub. Total amount: {Amount}");
        handler?.Invoke(this, new AccountEventArgs(amount, Amount, AccountOperation.Put));
    }

    public void Take(int amount)
    {
        if(Amount > amount)
        {
            Amount -= amount;
            handler?.Invoke(this, new AccountEventArgs(amount, Amount, AccountOperation.Take));
        }
        else
            handler?.Invoke(this, new AccountEventArgs(amount, Amount, AccountOperation.None));

    }
    
}
