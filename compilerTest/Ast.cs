using System;
using System.Collections.Generic;

namespace compilerTest
{
    abstract class Ast
    {
        abstract public dynamic Visit();

        public static Dictionary<string, dynamic> globalScope = new Dictionary<string, dynamic>();
    }

    class BinOp : Ast
    {
        Ast left;
        Token.Type op;
        Ast right;

        public BinOp()
        {

        }

        public BinOp(Ast left, Token.Type op, Ast right)
        {
            this.left = left;
            this.op = op;
            this.right = right;
        }

        public void SetLeft(Ast ast)
        {
            left = ast;
        }
        public void SetOp(Token.Type ast)
        {
            op = ast;
        }
        public void SetRight(Ast ast)
        {
            right = ast;
        }

        public void SwitchSides()
        {
            Ast swap = left;
            left = right;
            right = swap;
        }

        public override dynamic Visit()
        {
            dynamic leftValue = left.Visit();
            dynamic rightValue = right.Visit();
            switch (op)
            {
                case Token.Type.INTEGER_DIV:
                    return leftValue / rightValue;
                case Token.Type.MULT:
                    return leftValue * rightValue;
                case Token.Type.PLUS:
                    return leftValue + rightValue;
                case Token.Type.MINUS:
                    return leftValue - rightValue;
                case Token.Type.FLOAT_DIV:
                    return (float)leftValue / (float)rightValue;
                default:
                    throw new NotSupportedException();
            }
        }
    }

    class UnaryOp : Ast
    {
        readonly Token.Type op;
        readonly Ast value;

        public UnaryOp(Token.Type op, Ast value)
        {
            this.op = op;
            this.value = value;
        }

        public override dynamic Visit()
        {
            if (op == Token.Type.PLUS)
                return +value.Visit();
            if (op == Token.Type.MINUS)
                return -value.Visit();
            throw new NotSupportedException();
        }
    }

    class Compound : Ast
    {
        readonly List<Ast> children = new List<Ast>();
        public override dynamic Visit()
        {
            foreach (Ast node in children)
            {
                node.Visit();
            }
            foreach (dynamic item in globalScope.Values)
            {
                Console.WriteLine(item);
            }
            return null;
        }

        public void Add(Ast node)
        {
            children.Add(node);
        }
    }

    class Assign : Ast
    {
        Ast left;
        Token op;
        Ast right;

        public void AddLeft(Ast variableName)
        {
            left = variableName;
        }

        public void AddOperator(Token token)
        {
            op = token;
        }
        public void AddRight(Ast ast)
        {
            right = ast;
        }
        public override dynamic Visit()
        {
            dynamic key = left.Visit();
            if (globalScope.ContainsKey(key))
            {
                globalScope[key] = right.Visit();
            } else
            {
                globalScope.Add(left.Visit(), right.Visit());
            }
            return null;
        }
    }

    class Var : Ast
    {
        readonly string name;

        public Var(string name)
        {
            this.name = name;
        }

        public string getName() => name;
        public override dynamic Visit()
        {
            bool success = globalScope.TryGetValue(name, out dynamic result);
            if (success)
            {
                return result;
            }
            throw new KeyNotFoundException("Key {key} has not been delcared".Replace("{key}", name));
        }
    }

    class Ident : Ast
    {
        readonly string identifier;
        public Ident(string identifier)
        {
            this.identifier = identifier;
        }

        public override dynamic Visit()
        {
            return identifier;
        }
    }

    class NoOp : Ast
    {
        public override dynamic Visit()
        {
            return null;
        }
    }

    class Numeric : Ast
    {
        readonly int value;
        public Numeric(int value)
        {
            this.value = value;
        }

        public BinOp ToBinOp()
        {
            BinOp binOp = new BinOp();
            binOp.SetLeft(this);
            return binOp;
        }

        public override dynamic Visit()
        {
            return value;
        }
    }

    class ProgramSpec : Ast
    {
        string programName;
        Ast block;
        public ProgramSpec(string name, Ast block)
        {
            programName = name;
            this.block = block;
        }


        public override dynamic Visit()
        {
            throw new NotImplementedException();
        }
    }

    class Block : Ast
    {
        List<Ast> declarations;
        Ast compoundStatement;
        public Block(List<Ast> declarations, Ast compoundStatement)
        {
            this.declarations = declarations;
            this.compoundStatement = compoundStatement;
        }

        public override dynamic Visit()
        {
            throw new NotImplementedException();
        }
    }

    class VarDecleration : Ast
    {
        Ast var;
        Ast type;

        public VarDecleration(Ast var, Ast type)
        {
            this.var = var;
            this.type = type;
        }

        public override dynamic Visit()
        {
            throw new NotImplementedException();
        }
    }

    class VarType : Ast
    {
        Token token;
        dynamic value;
        public VarType(Token token)
        {
            this.token = token;
            value = token.GetValue();
        }

        public override dynamic Visit()
        {
            throw new NotImplementedException();
        }
    }
}
