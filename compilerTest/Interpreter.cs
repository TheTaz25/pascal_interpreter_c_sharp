using System;

namespace compilerTest
{
    class Interpreter
    {
        private Text text;
        private Token currentToken = null;
        public Interpreter(string text)
        {
            this.text = new Text(text);
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
