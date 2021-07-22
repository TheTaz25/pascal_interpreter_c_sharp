using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace compilerTest
{
    class Text
    {
        char[] chars;
        int currentPosition = 0;
        bool lastCharSent = false;

        private List<char> digits = new List<char>() { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };
        private List<char> mathChars = new List<char>() { '-', '+', '*', '/' };
        public Text(string text)
        {
            chars = text.ToCharArray();
        }

        public bool isEol() => currentPosition == (chars.Length - 1) && lastCharSent;

        public char getChar(bool advance = false) {
            char c = chars[currentPosition];
            if (currentPosition == (chars.Length - 1))
            {
                lastCharSent = true;
            }
            if (advance)
            {
                advanceChar();
            }
            return c;
        }

        public void advanceChar()
        {
            if (currentPosition != (chars.Length - 1))
            {
                currentPosition++;
            }
        }

        public bool isDigit() => digits.Contains(chars[currentPosition]);
        public bool isMathChar() => mathChars.Contains(chars[currentPosition]);

        public void skipWhiteSpace()
        {
            while (chars[currentPosition] == ' ')
            {
                advanceChar();
                if (isEol()) break;
            }
        }

        public bool isPlus() => chars[currentPosition] == '+';
        public bool isMinus() => chars[currentPosition] == '-';
        public bool isMult() => chars[currentPosition] == '*';
        public bool isDiv() => chars[currentPosition] == '/';

        public int getNextIntegerValue()
        {
            string allIntegers = getChar().ToString();
            advanceChar();
            while(!isEol() && isDigit())
            {
                allIntegers += getChar(true);
            }
            return int.Parse(allIntegers);
        }
    }
}
