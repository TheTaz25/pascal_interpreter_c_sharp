using System;
using System.Collections.Generic;
using System.Linq;

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
            string[] allDigits = new string[10]{"0", "1", "2", "3", "4", "5", "6", "7", "8", "9"};
            string text = this.text;
            if (pos > text.Length - 1)
            {
                return new Token(Token.Type.EOF, null);
            }

            string currentChar = text.Substring(pos, 1);
            pos++;

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
                    return new Token(Token.Type.INTEGER, int.Parse(currentChar));
                case "+":
                    return new Token(Token.Type.PLUS, currentChar);
                default:
                    throw new Exception("Error parsing input");
            }
        }

        private void eat(Token.Type type)
        {
            if (currentToken.getType() == type)
            {
                currentToken = getNextToken();
            } else
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
            eat(Token.Type.PLUS);

            Token right = currentToken;
            eat(Token.Type.INTEGER);

            return left.getValue() + right.getValue();
        }
    }
}
