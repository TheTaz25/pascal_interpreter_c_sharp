using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace compilerTest
{
    class Text
    {
        readonly char[] chars;
        int currentPosition = 0;
        bool lastCharSent = false;

        static readonly List<char> digits = new List<char>() { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };
        static readonly private List<char> mathChars = new List<char>() { '-', '+', '*', '/' };
        static readonly private List<char> parens = new List<char>() { '(', ')' };
        public Text(string text)
        {
            chars = text.ToCharArray();
        }

        public bool IsEol() => currentPosition == (chars.Length - 1) && lastCharSent;
        public bool IsEolReal() => currentPosition == (chars.Length - 1);

        public char GetChar(bool advance = false) {
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


        public void SkipWhiteSpace()
        {
            while (chars[currentPosition] == ' ')
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

        public int GetNextIntegerValue()
        {
            string allIntegers = GetChar().ToString();
            AdvanceChar();
            while(!IsEol() && IsDigit())
            {
                allIntegers += GetChar(true);
            }
            return int.Parse(allIntegers);
        }
    }
}
