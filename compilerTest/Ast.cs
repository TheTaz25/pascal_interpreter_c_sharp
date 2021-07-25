using System;

namespace compilerTest
{
    abstract class Ast
    {
        abstract public dynamic Visit();
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
