namespace compilerTest
{
    class Token
    {
        public enum Type
        {
            INTEGER_CONST,
            PLUS,
            MINUS,
            MULT,
            INTEGER_DIV,
            FLOAT_DIV,
            PAREN_OPEN,
            PAREN_CLOSE,
            EOF,
            BEGIN,
            END,
            DOT,
            ASSIGN,
            SEMI,
            ID,
            PROGRAM,
            VAR,
            COLON,
            COMMA,
            INTEGER,
            REAL,
            REAL_CONST,
        }

        readonly private Type type;
        readonly private dynamic value;

        public Token(Type type, dynamic value)
        {
            this.type = type;
            this.value = value;
        }

        public string Print() => "Token({type}, {value})".Replace("{type}", type.ToString()).Replace("{value}", value);

        public Type GetTokenType() => type;
        public dynamic GetValue() => value;
    }
}
