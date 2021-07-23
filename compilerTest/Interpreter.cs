using System;
using System.Collections.Generic;

namespace compilerTest
{
    class Interpreter
    {
        private Text text;
        private Token currentToken = null;
        public Interpreter(string text)
        {
            this.text = new Text(text);
            currentToken = getNextToken();
        }

        private Token getNextToken()
        {

            text.skipWhiteSpace();
            if (text.isEol())
            {
                return new Token(Token.Type.EOF, null);
            }

            if (text.isDigit())
                return new Token(Token.Type.INTEGER, text.getNextIntegerValue());

            if(text.isMathChar())
            {
                if (text.isPlus())
                    return new Token(Token.Type.PLUS, text.getChar(true));
                if (text.isMinus())
                    return new Token(Token.Type.MINUS, text.getChar(true));
                if (text.isMult())
                    return new Token(Token.Type.MULT, text.getChar(true));
                if (text.isDiv())
                    return new Token(Token.Type.DIV, text.getChar(true));
            }

            throw new Exception("Token not recognized: " + text.getChar());
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

        public dynamic factor()
        {
            int value = currentToken.getValue();
            eat(Token.Type.INTEGER);
            return value;
        }

        public dynamic term()
        {
            int result = factor();
            List<Token.Type> applicableTypes = new List<Token.Type> { Token.Type.DIV, Token.Type.MULT };
            while(applicableTypes.Contains(currentToken.getType()))
            {
                Token token = currentToken;
                if (token.getType() == Token.Type.MULT)
                {
                    eat(Token.Type.MULT);
                    result *= factor();
                }
                else if (token.getType() == Token.Type.DIV)
                {
                    eat(Token.Type.DIV);
                    result /= factor();
                }
            }
            return result;
        }

        public dynamic expression()
        {
            int result = term();
            List<Token.Type> applicableTypes = new List<Token.Type> { Token.Type.MINUS, Token.Type.PLUS };
            while (applicableTypes.Contains(currentToken.getType())) {
                Token token = currentToken;
                if (token.getType() == Token.Type.PLUS)
                {
                    eat(Token.Type.PLUS);
                    result += term();
                } else if (token.getType() == Token.Type.MINUS)
                {
                    eat(Token.Type.MINUS);
                    result -= term();
                }
            }
            return result;
        }
    }
}
