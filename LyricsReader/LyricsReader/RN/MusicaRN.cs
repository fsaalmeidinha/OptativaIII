using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LyricsReader.RN
{
    public class MusicaRN
    {
        ProjetoIndexacaoDBEntities ent;

        #region Construtores

        public MusicaRN()
            : this(new ProjetoIndexacaoDBEntities())
        {
        }

        public MusicaRN(ProjetoIndexacaoDBEntities ent)
        {
            this.ent = ent;
        }

        #endregion Construtores

        public List<Musica> PesquisarMusicas()
        {
            return ent.Musicas.ToList();
        }

        public string SalvarMusicas()
        {
            try
            {
                string erros = "";
                List<Musica> musicas = new VagalumeLyricsReader().LerMusicas(ref erros);

                if (erros == null)
                    return erros;

                int maxCantor = musicas.Max(msk => msk.Cantor.Length);
                int maxNomeMusica = musicas.Max(msk => msk.NomeMusica.Length);
                int maxLetra = musicas.Max(msk => msk.Letra.Length);

                musicas.ForEach(msk => ent.Musicas.AddObject(msk));
                ent.SaveChanges();
                return erros;
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        public List<Musica> PesquisarMusicasPorFiltro(params string[] filtros)
        {
            List<string> filtrosSimples = filtros.Where(filt => !filt.Contains(" ")).ToList();
            List<string> filtrosCompostos = filtros.Where(filt => filt.Contains(" ")).ToList();
            List<Musica> musicas = ent.Musicas.ToList();

            foreach (string filtro in filtrosSimples)
            {
                List<Palavra> palavras = ent.Palavras.ToList();
                palavras = palavras.Where(plv => plv.Descricao.ToLower() == filtro.ToLower()).ToList();
                HashSet<Musica> musicasFiltro = new HashSet<Musica>();
                palavras.ForEach(plv => plv.MusicaPalavras.ToList().ForEach(mskPlv => musicasFiltro.Add(mskPlv.Musica)));
                musicas = musicas.Where(msk => musicasFiltro.Any(mskFiltro => mskFiltro.Id == msk.Id)).ToList();
            }

            //foreach (string filtro in filtrosCompostos)
            //{
            //    List<Palavra> palavras = ent.Palavras.ToList();
            //    palavras = palavras.Where(plv => filtro.ToLower().Split(' ').Contains(plv.Descricao.ToLower())).ToList();
            //}

            return musicas.ToList();
        }
    }
}
