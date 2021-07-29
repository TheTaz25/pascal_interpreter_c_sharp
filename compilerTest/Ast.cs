using System;
using System.Collections.Generic;

namespace compilerTest
{
    abstract class Ast
    {
        abstract public dynamic Visit();

        public static Dictionary<string, int> globalScope = new Dictionary<string, int>();
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
                case Token.Type.DIV:
                    return leftValue / rightValue;
                case Token.Type.MULT:
                    return leftValue * rightValue;
                case Token.Type.PLUS:
                    return leftValue + rightValue;
                case Token.Type.MINUS:
                    return leftValue - rightValue;
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
            Console.WriteLine(globalScope);
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
        public override dynamic Visit()
        {
            bool success = globalScope.TryGetValue(name, out int result);
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
}
