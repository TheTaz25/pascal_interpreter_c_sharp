using System;
using System.Collections.Generic;

namespace compilerTest
{
    class Interpreter
    {
        private string text;
        private int pos = 0;
        private Token currentToken = null;
        public Interpreter(string text)
        {
            this.text = text;
        }

        private Token getNextToken()
        {
            if (isEOL())
            {
                return new Token(Token.Type.EOF, null);
            }

            string currentChar = text.Substring(pos++, 1);

            while (currentChar == " ")
            {
                currentChar = text.Substring(pos++, 1);
            }

            switch (currentChar)
            {
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                case "9":
                case "0":
                    return getIntegerToken(currentChar);
                case "+":
                    return new Token(Token.Type.PLUS, currentChar);
                case "-":
                    return new Token(Token.Type.MINUS, currentChar);
                default:
                    throw new Exception("Error parsing input");
            }
        }

        private bool isEOL() => pos > text.Length - 1;

        private Token getIntegerToken(string current)
        {
            string allIntegers = current;
            List<string> allDigits = new List<string>() { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
            if (!isEOL())
            {
                while (!isEOL())
                {
                    string currentChar = text.Substring(pos++, 1);
                    if (allDigits.Contains(currentChar))
                    {
                        allIntegers += currentChar;
                    }
                    else
                    {
                        pos--;
                        break;
                    }
                    if (isEOL()) { break; }
                }
            }
            return new Token(Token.Type.INTEGER, int.Parse(allIntegers));
        }

        private void eat(Token.Type type)
        {
            if (currentToken.getType() == type)
            {
                currentToken = getNextToken();
            }
            else
            {
                throw new Exception("Error parsing input");
            }
        }

        public dynamic expression()
        {
            currentToken = getNextToken();
            Token left = currentToken;
            eat(Token.Type.INTEGER);

            Token op = currentToken;
            if (op.getType().Equals(Token.Type.PLUS))
            {
                eat(Token.Type.PLUS);
            }
            else
            {
                eat(Token.Type.MINUS);
            }

            Token right = currentToken;
            eat(Token.Type.INTEGER);

            switch (op.getType())
            {
                case Token.Type.PLUS:
                    return left.getValue() + right.getValue();
                case Token.Type.MINUS:
                    return left.getValue() - right.getValue();
                default:
                    throw new Exception("Error parsing input");
            }
        }
    }
}
