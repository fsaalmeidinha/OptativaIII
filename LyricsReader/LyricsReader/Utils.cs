using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace LyricsReader
{
    public class Utils
    {
        public static string NormalizeString(string texto)
        {
            string textoNormalizado = texto.Normalize(NormalizationForm.FormD);
            ////Elimina acentuação
            string textoSemAcentuacao = String.Join("", textoNormalizado.ToCharArray().Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark));

            List<UnicodeCategory> unicodes = new List<UnicodeCategory>(){
                UnicodeCategory.SpaceSeparator,
                UnicodeCategory.LetterNumber,
                UnicodeCategory.LowercaseLetter,
                UnicodeCategory.ModifierLetter,
                UnicodeCategory.OtherLetter,
                UnicodeCategory.TitlecaseLetter,
                UnicodeCategory.UppercaseLetter,
                UnicodeCategory.DecimalDigitNumber,
                UnicodeCategory.OtherNumber
            };

            //Elimina pontuacao
            textoSemAcentuacao = String.Join("", textoSemAcentuacao.ToCharArray().Select(c => unicodes.Contains(CharUnicodeInfo.GetUnicodeCategory(c)) ? c : ' '));

            return textoSemAcentuacao;
        }
    }
}
