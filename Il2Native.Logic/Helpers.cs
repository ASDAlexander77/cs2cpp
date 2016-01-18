namespace Il2Native.Logic
{
    public static class Helpers
    {
        public static string CleanUpNameAllUnderscore(this string name)
        {
            if (name == null)
            {
                return null;
            }

            var s = new char[name.Length];

            var n = ' ';
            for (var i = 0; i < name.Length; i++)
            {
                var c = name[i];
                switch (c)
                {
                    case '.':
                    case ':':
                    case '<':
                    case '>':
                    case '-':
                    case ',':
                    case '*':
                    case '[':
                    case ']':
                    case '&':
                    case '(':
                    case ')':
                    case '{':
                    case '}':
                    case '$':
                    case '=':
                        n = '_';
                        break;
                    default:
                        n = c;
                        break;
                }

                s[i] = n;
            }

            return new string(s);
        }

        public static string CleanUpName(this string name)
        {
            if (name == null)
            {
                return null;
            }

            var s = new char[name.Length];

            var n = ' ';
            for (var i = 0; i < name.Length; i++)
            {
                var c = name[i];
                switch (c)
                {
                    case ' ':
                        n = '_';
                        break;
                    case '.':
                        n = '_';
                        break;
                    case ':':
                        n = '_';
                        break;
                    case '<':
                        n = 'G';
                        break;
                    case '>':
                        n = 'C';
                        break;
                    case '-':
                        n = '_';
                        break;
                    case ',':
                        n = '_';
                        break;
                    case '*':
                        n = 'P';
                        break;
                    case '[':
                        n = 'A';
                        break;
                    case ']':
                        n = 'Y';
                        break;
                    case '&':
                        n = 'R';
                        break;
                    case '(':
                        n = 'F';
                        break;
                    case ')':
                        n = 'N';
                        break;
                    case '{':
                        n = 'C';
                        break;
                    case '}':
                        n = 'Y';
                        break;
                    case '$':
                        n = 'D';
                        break;
                    case '=':
                        n = 'E';
                        break;
                    default:
                        n = c;
                        break;
                }

                s[i] = n;
            }

            return new string(s);
        }
    }
}
