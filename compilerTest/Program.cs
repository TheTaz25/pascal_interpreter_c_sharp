using System;

namespace compilerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string input;
            while (true)
            {
                try
                {
                    input = Console.ReadLine();
                }
                catch
                {
                    break;
                }
                if (input.Equals(""))
                {
                    continue;
                }
                Interpreter interpreter = new Interpreter(input);
                int result = interpreter.Expression();
                Console.WriteLine(result);
            }
        }
    }
}
