using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LyricsReader.RN
{
    public class SoundexRN
    {
        public static String RetornaCodigoSoundex(string palavra)
        {
            String retornoSoundex = String.Empty;
            char[] palavraQuebrada = palavra.ToUpper().ToCharArray();

            retornoSoundex = palavraQuebrada[0].ToString();

            for (int i = 1; i < palavraQuebrada.Length; i++)
            {
                retornoSoundex += ValorSoundex(palavraQuebrada[i]).ToString();
            }

            retornoSoundex = retornoSoundex.Replace("0", "");
            for (int i = retornoSoundex.Length; i < 4; i++)
            {
                retornoSoundex += "0";
            }

            return retornoSoundex.PadLeft(4);
        }

        private static int ValorSoundex(char p)
        {
            if (p == 'B' || p == 'F' || p == 'P' || p == 'V')
                return 1;
            else if (p == 'C' || p == 'G' || p == 'J' || p == 'K' || p == 'Q' || p == 'S' || p == 'X' || p == 'Z')
                return 2;
            else if (p == 'D' || p == 'T')
                return 3;
            else if (p == 'L')
                return 4;
            else if (p == 'M' || p == 'N')
                return 5;
            else if (p == 'R')
                return 6;
            else
                return 0;
        }
    }
}
