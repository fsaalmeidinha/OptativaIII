﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LyricsReader.Model;

namespace LyricsReader
{
    public class Program
    {
        static void Main(string[] args)
        {
            List<Musica> musicas = new VagalumeLyricsReader().LerMusicas();
            Musica msk = musicas.First();
            msk.Letra = Utils.NormalizeString(msk.Letra);

            Dictionary<string, int> dic = new Dictionary<string, int>();
            List<string> palavras = new List<string>();
            musicas.Select(m => m.Letra.ToLower()).ToList().ForEach(l => palavras.AddRange(l.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)));
            palavras = palavras.Distinct().ToList();
            palavras.ForEach(p => dic.Add(p, musicas.Count(m => m.Letra.Contains(" " + p + " "))));
        }
    }
}