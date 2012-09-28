using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LyricsReader.RN
{
    public class BigramaRN
    {
        ProjetoIndexacaoDBEntities ent;
        MusicaRN musicaRN;
        PalavraRN palavraRN;

        #region Construtores

        public BigramaRN()
            : this(new ProjetoIndexacaoDBEntities())
        {
        }

        public BigramaRN(ProjetoIndexacaoDBEntities ent)
        {
            this.ent = ent;
            musicaRN = new MusicaRN(ent);
            palavraRN = new PalavraRN(ent);
        }

        #endregion Construtores

        public string SalvarBigramas()
        {
            try
            {
                int qtdLidas = 0;
                int qtdLer = 10975;
                List<Palavra> palavras = ent.Palavras.OrderBy(plv => plv.Id).Skip(qtdLidas).Take(qtdLer).ToList();

                AdicionarPalavrasABigramas(palavras);

                ent.SaveChanges();
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        private void AdicionarPalavrasABigramas(List<Palavra> palavras)
        {
            List<Bigrama> bigramas = ent.Bigramas.ToList();
            foreach (Palavra palavra in palavras)
            {
                string palavraStr = palavra.Descricao.ToLower();
                Bigrama bigExtremoEsq = bigramas.FirstOrDefault(big => big.Valor == "$" + palavraStr.First().ToString());
                if (bigExtremoEsq == null)
                {
                    bigExtremoEsq = new Bigrama() { Valor = "$" + palavraStr.First().ToString(), ExtremoAEsquerda = true };
                    bigramas.Add(bigExtremoEsq);
                }
                bigExtremoEsq.Palavras.Add(palavra);

                Bigrama bigExtremoDir = bigramas.FirstOrDefault(big => big.Valor == palavraStr.Last().ToString() + "$");
                if (bigExtremoDir == null)
                {
                    bigExtremoDir = new Bigrama() { Valor = palavraStr.Last().ToString() + "$", ExtremoADireita = true };
                    bigramas.Add(bigExtremoDir);
                }
                bigExtremoDir.Palavras.Add(palavra);

                for (int indInicioPalavra = 0; indInicioPalavra < palavra.Descricao.Length - 1; indInicioPalavra++)
                {
                    string bigramaStr = palavraStr.Substring(indInicioPalavra, 2);
                    Bigrama bigrama = bigramas.FirstOrDefault(big => big.Valor == bigramaStr);
                    if (bigrama == null)
                    {
                        bigrama = new Bigrama() { Valor = bigramaStr };
                        bigramas.Add(bigrama);
                    }

                    bigrama.Palavras.Add(palavra);
                }
            }
        }
    }
}
