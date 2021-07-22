﻿using System;
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
                case "*":
                    return new Token(Token.Type.MULT, currentChar);
                case "/":
                    return new Token(Token.Type.DIV, currentChar);
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

            

            int result = left.getValue();
            while(currentToken.getType() != Token.Type.EOF)
            {
                Token op = currentToken;
                switch (op.getType())
                {
                    case Token.Type.PLUS:
                    case Token.Type.MINUS:
                    case Token.Type.DIV:
                    case Token.Type.MULT:
                        eat(op.getType());
                        break;
                }
                Token right = currentToken;
                eat(Token.Type.INTEGER);
                switch (op.getType())
                {
                    case Token.Type.PLUS:
                        result += right.getValue();
                        break;
                    case Token.Type.MINUS:
                        result -= right.getValue();
                        break;
                    case Token.Type.MULT:
                        result *= right.getValue();
                        break;
                    case Token.Type.DIV:
                        result /= right.getValue();
                        break;
                    default:
                        throw new Exception("Error parsing input");
                }
            }
            return result;
        }
    }
}
