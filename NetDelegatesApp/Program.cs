BankAccount accaunt = new(1000);
accaunt.RegisterHandler(PrintGreen);

accaunt.Take(500);
accaunt.Take(400);

accaunt.RegisterHandler(PrintRed);

accaunt.Take(200);


void PrintRed(string message)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(message);
}

void PrintGreen(string message)
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine(message);
}


public delegate void AccountHandler(string message);

public class BankAccount
{
    int amount;
    AccountHandler? accountHandler;

    public void RegisterHandler(AccountHandler handler)
    {
        accountHandler += handler;
    }

    public BankAccount(int amount)
    {
        this.amount = amount;
        this.accountHandler = null;
    }

    public void Add(int amount)
    {
        this.amount += amount;
        accountHandler?.Invoke($"На счет доблено {amount} руб. Общая сумма {this.amount}");
    }

    public void Take(int amount)
    {
        if(amount < this.amount)
        {
            this.amount -= amount;
            accountHandler?.Invoke($"Со счета снято {amount} руб. Общая сумма {this.amount}");
        }
        else
            accountHandler?.Invoke("На счете не зватает средств");

    }
}


