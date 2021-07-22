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
            EOF
        }

        private Type type;
        private dynamic value;

        public Token(Type type, dynamic value)
        {
            this.type = type;
            this.value = value;
        }

        public string print() => "Token({type}, {value})".Replace("{type}", type.ToString()).Replace("{value}", value);

        public Type getType() => type;
        public dynamic getValue() => value;
    }
}
