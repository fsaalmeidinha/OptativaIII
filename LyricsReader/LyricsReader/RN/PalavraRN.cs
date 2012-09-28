using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace LyricsReader.RN
{
    public class PalavraRN
    {
        ProjetoIndexacaoDBEntities ent;
        MusicaRN musicaRN;

        #region Construtores

        public PalavraRN()
            : this(new ProjetoIndexacaoDBEntities())
        {
        }

        public PalavraRN(ProjetoIndexacaoDBEntities ent)
        {
            this.ent = ent;
            musicaRN = new MusicaRN(ent);
        }

        #endregion Construtores

        public string SalvarPalavras()
        {
            try
            {
                //Atualizar antes de rodar
                int qtdLidas = 1000;
                int qtdLer = 200;
                List<Musica> musicas = ent.Musicas.OrderBy(msk => msk.Id).Skip(qtdLidas).Take(qtdLer).ToList();

                RecuperaPalavrasMusicas(musicas);

                ent.SaveChanges();
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        private void RecuperaPalavrasMusicas(List<Musica> musicas)
        {
            //List<Palavra> palavras = ent.Palavras.ToList();//new List<Palavra>();
            //List<MusicaPalavra> musicasPalavras = ent.MusicaPalavras.ToList();// new List<MusicaPalavra>();

            foreach (Musica msk in musicas)
            {
                //Elimina os caracteres especiais e pontuação
                string[] palavrasMusica = Utils.NormalizeString(msk.Letra).Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                for (int indPalavraMusica = 0; indPalavraMusica < palavrasMusica.Length; indPalavraMusica++)
                {
                    string p = palavrasMusica[indPalavraMusica].ToLower();
                    Palavra palavra = ent.Palavras.FirstOrDefault(plv => String.Compare(plv.Descricao, p, true) == 0);
                    if (palavra == null)
                    {
                        palavra = new Palavra() { Descricao = p };
                    }

                    MusicaPalavra musicaPalavras = ent.MusicaPalavras.FirstOrDefault(mskPlv => mskPlv.Palavra.Descricao == palavra.Descricao && mskPlv.Musica.Id == msk.Id);
                    if (musicaPalavras == null)
                    {
                        musicaPalavras = new MusicaPalavra()
                        {
                            Musica = msk,
                            Palavra = palavra
                        };
                    }

                    musicaPalavras.Indices += "," + indPalavraMusica;
                }
            }
        }
    }
}
