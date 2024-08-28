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


    class MathOpers
    {
        public static int Sum(int a, int b) { return a + b; }
        public static double Sum(double a, double b) { return a + b; }
        public static int Mult(int a, int b) { return a * b; }
        public static double Mult(double a, double b) { return a * b; }
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
                else return null;
            }
        }
    }
}
