using System;

namespace compilerTest
{
    class Program
    {
        static void Main()
        {
            string input = System.IO.File.ReadAllText(@"D:\Code\compilerTest\compilerTest\compilerTest\theProgram.txt");
                
            Interpreter interpreter = new Interpreter(input);
            Ast rootNode = interpreter.Program();
            rootNode.Visit();
            Console.WriteLine();
        }
    }
}
