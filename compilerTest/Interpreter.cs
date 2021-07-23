using System;
using System.Collections.Generic;

namespace compilerTest
{
    class Interpreter
    {
        readonly private Text text;
        private Token currentToken = null;
        public Interpreter(string text)
        {
            this.text = new Text(text);
            currentToken = GetNextToken();
        }

        private Token GetNextToken()
        {

            text.SkipWhiteSpace();
            if (text.IsEol())
            {
                return new Token(Token.Type.EOF, null);
            }

            if (text.IsDigit())
                return new Token(Token.Type.INTEGER, text.GetNextIntegerValue());

            if(text.IsMathChar())
            {
                if (text.IsPlus())
                    return new Token(Token.Type.PLUS, text.GetChar(true));
                if (text.IsMinus())
                    return new Token(Token.Type.MINUS, text.GetChar(true));
                if (text.IsMult())
                    return new Token(Token.Type.MULT, text.GetChar(true));
                if (text.IsDiv())
                    return new Token(Token.Type.DIV, text.GetChar(true));
            }

            throw new Exception("Token not recognized: " + text.GetChar());
        }

        private void Eat(Token.Type type)
        {
            if (currentToken.GetTokenType() == type)
            {
                currentToken = GetNextToken();
            }
            else
            {
                throw new Exception("Error parsing input");
            }
        }

        public dynamic Factor()
        {
            int value = currentToken.GetValue();
            Eat(Token.Type.INTEGER);
            return value;
        }

        public dynamic Term()
        {
            int result = Factor();
            List<Token.Type> applicableTypes = new List<Token.Type> { Token.Type.DIV, Token.Type.MULT };
            while(applicableTypes.Contains(currentToken.GetTokenType()))
            {
                Token token = currentToken;
                if (token.GetTokenType() == Token.Type.MULT)
                {
                    Eat(Token.Type.MULT);
                    result *= Factor();
                }
                else if (token.GetTokenType() == Token.Type.DIV)
                {
                    Eat(Token.Type.DIV);
                    result /= Factor();
                }
            }
            return result;
        }

        public dynamic Expression()
        {
            int result = Term();
            List<Token.Type> applicableTypes = new List<Token.Type> { Token.Type.MINUS, Token.Type.PLUS };
            while (applicableTypes.Contains(currentToken.GetTokenType())) {
                Token token = currentToken;
                if (token.GetTokenType() == Token.Type.PLUS)
                {
                    Eat(Token.Type.PLUS);
                    result += Term();
                } else if (token.GetTokenType() == Token.Type.MINUS)
                {
                    Eat(Token.Type.MINUS);
                    result -= Term();
                }
            }
            return result;
        }
    }
}
