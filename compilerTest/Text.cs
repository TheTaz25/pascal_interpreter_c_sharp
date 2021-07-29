using System.Collections.Generic;

namespace compilerTest
{
    class Text
    {
        readonly char[] chars;
        int currentPosition = 0;
        bool lastCharSent = false;

        static readonly private List<char> alpha = new List<char>("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray());
        static readonly private List<char> digits = new List<char>() { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };
        static readonly private List<char> mathChars = new List<char>() { '-', '+', '*', '/' };
        static readonly private List<char> parens = new List<char>() { '(', ')' };
        static readonly private Dictionary<string, Token.Type> keywords = new Dictionary<string, Token.Type>();
        public Text(string text)
        {
            chars = text.ToCharArray();
            keywords.Add("BEGIN", Token.Type.BEGIN);
            keywords.Add("END", Token.Type.END);
        }

        public bool IsEol() => currentPosition == (chars.Length - 1) && lastCharSent;
        public bool IsEolReal() => currentPosition == (chars.Length - 1);

        public char PeekChar() => !IsEolReal() ? chars[currentPosition + 1] : ' ';
        public char GetChar(bool advance = false)
        {
            char c = chars[currentPosition];
            if (IsEolReal())
            {
                lastCharSent = true;
            }
            if (advance)
            {
                AdvanceChar();
            }
            return c;
        }

        public void AdvanceChar()
        {
            if (!IsEolReal())
            {
                currentPosition++;
            }
        }

        public bool IsDigit() => digits.Contains(chars[currentPosition]);
        public bool IsMathChar() => mathChars.Contains(chars[currentPosition]);
        public bool IsParen() => parens.Contains(chars[currentPosition]);
        public bool IsAlpha() => alpha.Contains(chars[currentPosition]);


        public void SkipWhiteSpace()
        {
            while (char.IsWhiteSpace(chars[currentPosition]))
            {
                AdvanceChar();
                if (IsEolReal())
                {
                    lastCharSent = true;
                    break;
                }
            }
        }

        public bool IsPlus() => chars[currentPosition] == '+';
        public bool IsMinus() => chars[currentPosition] == '-';
        public bool IsMult() => chars[currentPosition] == '*';
        public bool IsDiv() => chars[currentPosition] == '/';
        public bool IsOpenParen() => chars[currentPosition] == '(';
        public bool IsClosingParen() => chars[currentPosition] == ')';
        public bool IsColon() => chars[currentPosition] == ':';
        public bool IsSemicolon() => chars[currentPosition] == ';';
        public bool IsDot() => chars[currentPosition] == '.';

        public int GetNextIntegerValue()
        {
            string allIntegers = GetChar().ToString();
            AdvanceChar();
            while (!IsEol() && IsDigit())
            {
                allIntegers += GetChar(true);
            }
            return int.Parse(allIntegers);
        }

        public string GetNextAlphaNumericValue()
        {
            string result = "";
            while (!IsEol() && IsAlphaNumeric())
            {
                result += GetChar(true);
            }
            return result;
        }

        public bool IsKeyWord(string word) => keywords.ContainsKey(word);

        public Token GetTokenForKeyWord(string word)
        {
            if (keywords.TryGetValue(word, out Token.Type result))
            {
                return new Token(result, null);
            }
            else
            {
                return new Token(Token.Type.ID, word);
            }
        }

        public bool IsAlphaNumeric() => IsAlpha() || IsDigit();
    }
}
