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
            while (text.SkipComment() || text.SkipWhiteSpace()) { }
            
            if (text.IsEol())
            {
                return new Token(Token.Type.EOF, null);
            }

            if (text.IsDigit())
            {
                dynamic numberResult = text.GetNextNumberValue();
                if (numberResult is int)
                {
                    return new Token(Token.Type.INTEGER_CONST, numberResult);
                }
                else if (numberResult is float)
                {
                    return new Token(Token.Type.REAL_CONST, numberResult);
                }
            }

            if (text.IsAlphaNumeric())
            {
                string word = text.GetNextAlphaNumericValue();
                return text.GetTokenForKeyWord(word);
            }

            if (text.IsColon() && text.PeekChar() == '=')
            {
                text.AdvanceChar();
                text.AdvanceChar();
                return new Token(Token.Type.ASSIGN, ":=");
            } else if (text.IsColon())
            {
                text.AdvanceChar();
                return new Token(Token.Type.COLON, ":");
            }

            if (text.IsComma())
            {
                text.AdvanceChar();
                return new Token(Token.Type.COMMA, ",");
            }

            if (text.IsSemicolon())
            {
                text.AdvanceChar();
                return new Token(Token.Type.SEMI, ';');
            }

            if (text.IsDot())
            {
                text.AdvanceChar();
                return new Token(Token.Type.DOT, '.');
            }

            if (text.IsMathChar())
            {
                if (text.IsPlus())
                    return new Token(Token.Type.PLUS, text.GetChar(true));
                if (text.IsMinus())
                    return new Token(Token.Type.MINUS, text.GetChar(true));
                if (text.IsMult())
                    return new Token(Token.Type.MULT, text.GetChar(true));
                if (text.IsFloatDiv())
                    return new Token(Token.Type.FLOAT_DIV, text.GetChar(true));
            }

            if (text.IsParen())
            {
                if (text.IsOpenParen())
                    return new Token(Token.Type.PAREN_OPEN, text.GetChar(true));
                if (text.IsClosingParen())
                    return new Token(Token.Type.PAREN_CLOSE, text.GetChar(true));
            }

            throw new Exception("Token not recognized: " + (int)text.GetChar());
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

        public Ast Factor()
        {
            dynamic tokenValue = currentToken.GetValue();
            Ast node;
            switch (currentToken.GetTokenType())
            {
                case Token.Type.INTEGER_CONST:
                    Eat(Token.Type.INTEGER_CONST);
                    node = new Numeric(tokenValue);
                    break;
                case Token.Type.REAL_CONST:
                    Eat(Token.Type.REAL_CONST);
                    node = new Numeric(tokenValue);
                    break;
                case Token.Type.ID:
                    Eat(Token.Type.ID);
                    node = new Var(tokenValue);
                    break;
                default:
                    throw new DataMisalignedException();
            }

            return node;
        }

        public Ast Wrapped()
        {
            List<Token.Type> applicableTypes = new List<Token.Type> { Token.Type.PAREN_OPEN };
            while (applicableTypes.Contains(currentToken.GetTokenType()))
            {
                if (currentToken.GetTokenType() == Token.Type.PAREN_OPEN)
                {
                    Eat(Token.Type.PAREN_OPEN);
                    Ast node = Expression();
                    Eat(Token.Type.PAREN_CLOSE);
                    return node;
                }
            }
            return Factor();
        }

        public Ast UnaryParse()
        {
            Token token = currentToken;
            if (token.GetTokenType() == Token.Type.MINUS)
            {
                Eat(Token.Type.MINUS);
                return new UnaryOp(token.GetTokenType(), Wrapped());
            }
            else if (token.GetTokenType() == Token.Type.PLUS)
            {
                Eat(Token.Type.PLUS);
                return new UnaryOp(token.GetTokenType(), Wrapped());
            }
            return Wrapped();
        }

        public Ast Term()
        {
            Ast node = UnaryParse();
            List<Token.Type> applicableTypes = new List<Token.Type> { Token.Type.INTEGER_DIV, Token.Type.MULT, Token.Type.FLOAT_DIV };
            while (applicableTypes.Contains(currentToken.GetTokenType()))
            {
                Token token = currentToken;
                if (token.GetTokenType() == Token.Type.MULT)
                {
                    Eat(Token.Type.MULT);
                }
                else if (token.GetTokenType() == Token.Type.INTEGER_DIV)
                {
                    Eat(Token.Type.INTEGER_DIV);
                } else if(token.GetTokenType() == Token.Type.FLOAT_DIV)
                {
                    Eat(Token.Type.FLOAT_DIV);
                }
                node = new BinOp(node, token.GetTokenType(), UnaryParse());
            }
            return node;
        }

        public Ast Expression()
        {
            Ast node = Term();
            List<Token.Type> applicableTypes = new List<Token.Type> { Token.Type.MINUS, Token.Type.PLUS };
            while (applicableTypes.Contains(currentToken.GetTokenType()))
            {
                Token token = currentToken;
                if (token.GetTokenType() == Token.Type.PLUS)
                {
                    Eat(Token.Type.PLUS);
                }
                else if (token.GetTokenType() == Token.Type.MINUS)
                {
                    Eat(Token.Type.MINUS);
                }
                node = new BinOp(node, token.GetTokenType(), Term());
            }
            return node;
        }

        public Ast Empty()
        {
            return new NoOp();
        }

        public Var Variable()
        {
            Var node = new Var(currentToken.GetValue());
            Eat(Token.Type.ID);
            return node;
        }

        public Ast Identifier()
        {
            Ident node = new Ident(currentToken.GetValue());
            Eat(Token.Type.ID);
            return node;
        }

        public Ast AssignmentStatement()
        {
            Assign node = new Assign();
            node.AddLeft(Identifier());
            node.AddOperator(currentToken);
            Eat(Token.Type.ASSIGN);
            node.AddRight(Expression());
            return node;
        }

        public Ast Statement()
        {
            switch(currentToken.GetTokenType())
            {
                case Token.Type.BEGIN:
                    return CompoundStatement();
                case Token.Type.ID:
                    return AssignmentStatement();
                default:
                    return Empty();
            }
        }

        public List<Ast> StatementList()
        {
            Ast node = Statement();

            List<Ast> nodes = new List<Ast> { node };

            while (currentToken.GetTokenType() == Token.Type.SEMI)
            {
                Eat(Token.Type.SEMI);
                nodes.Add(Statement());
            }

            if (currentToken.GetTokenType() == Token.Type.ID)
            {
                throw new Exception("ID not supported");
            }

            return nodes;
        }

        public Ast CompoundStatement()
        {
            // GetNextToken();
            Eat(Token.Type.BEGIN);
            List<Ast> nodes = StatementList();
            Eat(Token.Type.END);

            Compound root = new Compound();
            foreach (Ast ast in nodes)
            {
                root.Add(ast);
            }

            return root;
        }

        public Ast Program()
        {
            Eat(Token.Type.PROGRAM);
            Var varNode = Variable();
            string programName = varNode.getName();
            Eat(Token.Type.SEMI);
            Ast blockNode = Block();
            Ast programNode = new ProgramSpec(programName, blockNode);
            Eat(Token.Type.DOT);
            return programNode;
        }

        public Ast Block()
        {
            List<Ast> declarationNodes = Declarations();
            Ast compoundStatementNode = CompoundStatement();
            return new Block(declarationNodes, compoundStatementNode);
        }

        public List<Ast> Declarations()
        {
            List<Ast> declarations = new List<Ast>();
            if (currentToken.GetTokenType() == Token.Type.VAR)
            {
                Eat(Token.Type.VAR);
                while (currentToken.GetTokenType() == Token.Type.ID)
                {
                    List<Ast> declaration = VariableDeclaration();
                    declarations.AddRange(declaration);
                    Eat(Token.Type.SEMI);
                }
            }
            return declarations;
        }

        public List<Ast> VariableDeclaration()
        {
            List<Ast> varNodes = new List<Ast> { new Var(currentToken.GetValue()) };
            Eat(Token.Type.ID);

            while (currentToken.GetTokenType() == Token.Type.COMMA)
            {
                Eat(Token.Type.COMMA);
                varNodes.Add(new Var(currentToken.GetValue()));
                Eat(Token.Type.ID);
            }

            Eat(Token.Type.COLON);
            Ast typeNode = TypeSpec();
            List<Ast> varDeclarations = new List<Ast>();
            foreach (Ast node in varNodes)
            {
                varDeclarations.Add(new VarDecleration(node, typeNode));
            }
            return varDeclarations;
        }

        public Ast TypeSpec()
        {
            Token token = currentToken;
            if (currentToken.GetTokenType() == Token.Type.INTEGER)
            {
                Eat(Token.Type.INTEGER);
            } else
            {
                Eat(Token.Type.REAL);
            }
            return new VarType(token);
        }
    }
}
