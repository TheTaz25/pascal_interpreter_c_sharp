namespace compilerTest
{
    class Token
    {
        public enum Type
        {
            INTEGER,
            PLUS,
            MINUS,
            MULT,
            DIV,
            PAREN_OPEN,
            PAREN_CLOSE,
            EOF
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
