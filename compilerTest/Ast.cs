using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace compilerTest
{
    abstract class Ast
    {
        abstract public dynamic Visit();
    }

    class BinOp : Ast
    {
        Ast left;
        Token op;
        Ast right;

        public void SetLeft(Ast ast)
        {
            left = ast;
        }
        public void SetOp(Token ast)
        {
            op = ast;
        }
        public void SetRight(Ast ast)
        {
            right = ast;
        }

        public override dynamic Visit()
        {
            dynamic leftValue = left.Visit();
            dynamic rightValue = right.Visit();
            switch (op.GetTokenType()) {
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

    class Numeric : Ast
    {
        readonly int value;
        public Numeric(int value)
        {
            this.value = value;
        }

        public override dynamic Visit()
        {
            return value;
        }
    }
}
