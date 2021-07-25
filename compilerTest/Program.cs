using System;

namespace compilerTest
{
    class Program
    {
        static void Main()
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
                Ast rootNode = interpreter.Expression();
                int result = rootNode.Visit();
                Console.WriteLine(result);
            }
        }
    }
}
