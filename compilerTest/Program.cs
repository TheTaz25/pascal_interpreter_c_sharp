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
                Ast rootNode = interpreter.Program();
                rootNode.Visit();
                Console.WriteLine();
            }
        }
    }
}
