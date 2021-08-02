using System;
using System.Collections.Generic;

namespace compilerTest
{
    class Text
    {
        readonly char[] chars;
        int currentPosition = 0;
        bool lastCharSent = false;

        static readonly private List<char> alpha = new List<char>("_abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray());
        static readonly private List<char> digits = new List<char>() { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };
        static readonly private List<char> mathChars = new List<char>() { '-', '+', '*', '/' };
        static readonly private List<char> parens = new List<char>() { '(', ')' };
        static readonly private Dictionary<string, Token.Type> keywords = new Dictionary<string, Token.Type>();
        public Text(string text)
        {
            chars = text.ToCharArray();
            if (keywords.Count == 0)
            {
                keywords.Add("BEGIN", Token.Type.BEGIN);
                keywords.Add("END", Token.Type.END);
                keywords.Add("DIV", Token.Type.INTEGER_DIV);
                keywords.Add("PROGRAM", Token.Type.PROGRAM);
                keywords.Add("VAR", Token.Type.VAR);
                keywords.Add("INTEGER", Token.Type.INTEGER);
                keywords.Add("REAL", Token.Type.REAL);
            }
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


        public bool SkipWhiteSpace()
        {
            bool result = false;
            List<char> whitelist = new List<char> { '\n', '\r', '\t', ' ' };
            while(whitelist.Contains(chars[currentPosition]))
            {
                result = true;
                AdvanceChar();
                if (IsEolReal())
                {
                    lastCharSent = true;
                    break;
                }
            }
            return result;
        }

        public bool SkipComment()
        {
            bool result = false;
            if (chars[currentPosition] == '{')
            {
                result = true;
                while(chars[currentPosition] != '}')
                {
                    AdvanceChar();
                }
                AdvanceChar();
            }
            return result;
        }

        public bool IsPlus() => chars[currentPosition] == '+';
        public bool IsMinus() => chars[currentPosition] == '-';
        public bool IsMult() => chars[currentPosition] == '*';
        public bool IsFloatDiv() => chars[currentPosition] == '/';
        public bool IsOpenParen() => chars[currentPosition] == '(';
        public bool IsClosingParen() => chars[currentPosition] == ')';
        public bool IsColon() => chars[currentPosition] == ':';
        public bool IsSemicolon() => chars[currentPosition] == ';';
        public bool IsDot() => chars[currentPosition] == '.';
        public bool IsStartOfComment() => chars[currentPosition] == '{';
        public bool IsComma() => chars[currentPosition] == ',';

        public dynamic GetNextNumberValue()
        {
            string allNumbers = GetChar().ToString();
            AdvanceChar();
            while (!IsEol() && IsDigit())
            {
                allNumbers += GetChar(true);
            }

            if(!IsEol() && IsDot())
            {
                allNumbers += ',';
                GetChar(true);
                while(!IsEol() && IsDigit())
                {
                    allNumbers += GetChar(true);
                }
                return float.Parse(allNumbers);
            }
            return int.Parse(allNumbers);
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
            if (keywords.TryGetValue(word.ToUpper(), out Token.Type result))
            {
                if (result == Token.Type.INTEGER_DIV)
                {
                    return new Token(result, "div");
                }
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
