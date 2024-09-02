using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDelegatesApp
{
    delegate int Oper(int x, int y);
    delegate void MessageOut();
    delegate int AccOperation(ref int begin, int x, int y);
    delegate void PrintMessage(string message);
    delegate int IntOpers(int a, int b);
    delegate double DoubleOpers(double a, double b);
    delegate T Opers<T>(T a, T b);
    delegate T1 DelegateMethod<out T1, in T2>(T2 varisble);

    public delegate void AccountHandler(string message);

    delegate Message MessageBuilder(string text); // delegate for covariant
    delegate void EmailReceiver(EmailMessage message); // delegate for contrvariant


    // generic delegates
    delegate T MessageBuilderT<out T>(string text); // generic delegate for covariant
    delegate void MessageReceiverT<in T>(T message); // generic delegate for contrvariant
    delegate TOut MessageConvert<in TIn, out TOut>(TIn message); // both



    class MathOpers
    {
        public static int Sum(int a, int b) { return a + b; }
        public static double Sum(double a, double b) { return a + b; }
        public static int Mult(int a, int b) { return a * b; }
        public static double Mult(double a, double b) { return a * b; }
    }

    delegate bool Filter(int item);

    class Message
    {
        public string Text { set; get; }

        public Message(string text) => Text = text;

        public virtual void Print() => Console.WriteLine(Text);
    }

    class EmailMessage : Message
    {
        public EmailMessage(string text) : base(text) { }

        public override void Print() => Console.WriteLine($"Email: {Text}");
    }

    class SmsMessage : Message
    {
        public SmsMessage(string text) : base(text) { }

        public override void Print() => Console.WriteLine($"Sms: {Text}");
    }


    class User
    {
        string name;

        public User(string name) => this.name = name;

        public string Name
        {
            get => this.name;
            set => this.name = value;
        }
    }

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
            if (amount < this.amount)
            {
                this.amount -= amount;
                accountHandler?.Invoke($"Со счета снято {amount} руб. Общая сумма {this.amount}");
            }
            else
                accountHandler?.Invoke("На счете не зватает средств");

        }
    }

    static class Examples
    {
        public static void DelegateWelcome()
        {
            Oper operation = Add;
            operation = new Oper(Mult);

            Console.WriteLine(operation(10, 20)); // Mult(10, 20)

            MessageOut mout = PrintHello;
            mout(); // PrintHello();

            Calc(Mult, PrintHello, 20, 30);

            int Calc(Oper operation, MessageOut printer, int a, int b)
            {
                printer();
                return operation(a, b);
            }

            int Add(int a, int b)
            {
                return a + b;
            }

            int Mult(int a, int b)
            {
                return a * b;
            }

            void PrintHello()
            {
                Console.WriteLine("Hello world");
            }

            
        }

        public static void DelegateInvoke()
        {
            string message = "Hello world";

            PrintMessage? printer;
            printer = PrintGreen;
            //printer += PrintRed;
            printer(message);
            Console.WriteLine();

            printer -= PrintGreen;
            //if(printer is not null)
            //    printer(message);
            printer?.Invoke(message);


            //PrintMessage printGreen = PrintGreen;
            //PrintMessage printRed = PrintRed;
            //PrintMessage printRedGreen = printGreen + printRed;


            AccOperation operation = Sum;
            operation += Mult;
            int begin = 0;
            Console.WriteLine(operation.Invoke(ref begin, 20, 30));
            Console.WriteLine(begin);


            void PrintGreen(string message)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(message);
            }

            void PrintRed(string message)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(message);
            }

            int Sum(ref int begin, int a, int b)
            {
                begin = begin + a + b;
                return a + b;
            }
            int Mult(ref int begin, int a, int b)
            {
                begin = begin * a * b;
                return a * b;
            }
        }

        public static void DelegateGeneric()
        {
            IntOpers intOpers = MathOpers.Sum;
            DoubleOpers doubleOpers = MathOpers.Sum;

            Opers<int> intOpersT = MathOpers.Sum;
            Opers<double> doubleOpersT = MathOpers.Sum;



            Calc(MathOpers.Sum, 10.0, 20);


            int IntCalc(IntOpers oper, int a, int b)
            {
                return oper(a, b);
            }

            int CalcT(Opers<int> oper, int a, int b)
            {
                return oper(a, b);
            }

            T Calc<T>(Opers<T> oper, T a, T b)
            {
                return oper(a, b);
            }

            IntOpers? GetOper(char sign)
            {
                if (sign == '+') return MathOpers.Sum;
                else if (sign == '*') return MathOpers.Mult;
                else if (sign == '-') return (x, y) => x - y;
                else return null;
            }
        }

        public static void DeleagteUse()
        {
            BankAccount accaunt = new(1000);
            accaunt.RegisterHandler(PrintGreen);

            accaunt.Take(500);
            accaunt.Take(400);

            accaunt.RegisterHandler(PrintRed);

            accaunt.Take(200);

            DelegateMethod<int, double> m = new DelegateMethod<int, double>(Ceil);


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

            int Ceil(double x)
            {
                return (int)x;
            }
        }

        public static void DelegateLambda()
        {
            //Oper operation;

            //operation = delegate (int a, int b)
            //{
            //    return a - b;
            //};

            //operation = (a, b) => { return a - b; };
            //operation = (a, b) => a - b;
            //operation = (a, b) =>
            //{
            //    int power = 1;
            //    for (int i = 0; i < b; i++)
            //        power *= a;
            //    return power;
            //};




            //int result = Calc(delegate (int a, int b) 
            //                  {
            //                      int power = 1;
            //                      for(int i = 0; i < b; i++)
            //                          power *= a;
            //                      return power;
            //                  }, 10, 20);

            //result = Calc((int a, int b) =>
            //{
            //    int power = 1;
            //    for (int i = 0; i < b; i++)
            //        power *= a;
            //    return power;
            //}, 
            //10, 20);

            //MessageOut mo = delegate
            //{
            //    Console.WriteLine("Empty anonim method");
            //};

            //mo = () => Console.WriteLine("Empty anonim method");

            PrintMessage pm = (name) => Console.WriteLine($"Hello {name}");
            pm += (name) => Console.WriteLine($"Good By {name}");

            pm("Bobby");
            Console.WriteLine();

            int[] array = new int[10];
            Random random = new();
            for (int i = 0; i < array.Length; i++)
                array[i] = random.Next(1, 99);
            foreach (var item in array)
                Console.Write($"{item} ");

            int result = SumFilter(array, i => i % 2 == 0);
            Console.WriteLine($"Sum evens items = {result}");

            Console.WriteLine($"Sum items divs 5 = {SumFilter(array, i => i % 5 == 0)}");


            int SumFilter(int[] array, Filter filter)
            {
                int result = 0;
                foreach (var item in array)
                    if (filter(item))
                        result += item;
                return result;
            }


            int Calc(Oper operation, int a, int b)
            {
                return operation(a, b);
            }

            int Sum(int a, int b)
            {
                return a + b;
            }

            //int Del(int a, int b)
            //{
            //    return a - b;
            //}

            int Mult(int a, int b)
            {
                return a * b;
            }
        }

        public static void ActionFuncPredicate()
        {
            Action action = () => Console.WriteLine("Hello");
            Action<string> helloUser = (name) => Console.WriteLine($"Hello {name}");
            Action<string, int> helloPerson =
                (name, age) => Console.WriteLine($"Name: {name}, Age: {age}");

            helloPerson("Bobby", 32);

            Func<int, int, int> oper = Sum;
            Func<float, int, double> moper = Power;

            int Sum(int a, int b)
            {
                return a + b;
            }

            double Power(float x, int n)
            {
                double result = 1;
                for (int i = 0; i < n; i++)
                    result *= x;
                return result;
            }
        }

        public static void DelegateCovarinatContrvariant()
        {
            // non generic delegates
            

            // covariant
            MessageBuilder messageBuilder = WriteMessage;
            Message message = messageBuilder("Hello world");
            message.Print();
            Console.WriteLine();

            messageBuilder = WriteEmailMessage; // Covariant
            message = messageBuilder("Hello people");
            message.Print();
            Console.WriteLine();

            // contrvariant
            EmailReceiver emailBox = ReceiveEmailMessage;
            emailBox(new EmailMessage("Good by world"));
            Console.WriteLine();

            emailBox = ReceiveMessage;
            emailBox(new EmailMessage("Good by people"));
            Console.WriteLine();


            // methods for covariant
            Message WriteMessage(string text) => new Message(text);
            EmailMessage WriteEmailMessage(string text) => new EmailMessage(text);

            // methods for contrvariant
            void ReceiveEmailMessage(EmailMessage message) => message.Print();
            void ReceiveSmsMessage(SmsMessage message) => message.Print();
            void ReceiveMessage(Message message) => message.Print();


            // method for generic covariant
            MessageBuilderT<SmsMessage> SmsMessageSender = (string text) => new SmsMessage(text);
            MessageBuilderT<Message> MessageSender = SmsMessageSender;
            Message messageSms = MessageSender("Hello Generic World");
            messageSms.Print();
            Console.WriteLine();

            // method for generic contrvariant
            MessageReceiverT<Message> messageReceiver = (Message message) => message.Print();
            MessageReceiverT<SmsMessage> messageSmsReceiver = messageReceiver;

            messageSmsReceiver(new SmsMessage("Good by Generic people"));
            // messageSmsReceiver(new Message("Good by Generic people")); // error

            messageReceiver(new Message("Good by Generic Base Message"));
            messageReceiver(new EmailMessage("Good by Generic Email Message"));
            messageReceiver(new SmsMessage("Good by Generic Sms Message"));


            // both
            MessageConvert<Message, EmailMessage> toEmailConverter = (Message message) => new EmailMessage(message.Text);
            MessageConvert<SmsMessage, Message> converter = toEmailConverter;

            Message messageBoth = converter(new SmsMessage("Both Sms Message"));
            messageBoth.Print();



            
        }
    }
}
