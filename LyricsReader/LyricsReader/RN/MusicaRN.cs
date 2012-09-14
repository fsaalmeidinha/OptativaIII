using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LyricsReader.RN
{
    public class MusicaRN
    {
        ProjetoIndexacaoDBEntities ent;
        //private List<Palavra> _palavras;
        //private List<Palavra> Palavras
        //{
        //    get
        //    {
        //        if (_palavras == null)
        //            _palavras = ent.Palavras.ToList();
        //        return _palavras;
        //    }
        //}

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

                //musicas.ForEach(msk => ent.Musicas.AddObject(msk));
                //ent.SaveChanges();
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

        private List<Musica> PesquisarPalavrasFiltroComposto(List<string> filtros)
        {
            List<Musica> musicasRetorno = new List<Musica>();
            List<Palavra> palavrasFiltros = new List<Palavra>();

            //Lista todas as listas de palavras de cada filtro
            foreach (string filtro in filtros)
            {
                string filtroCompare = filtro.ToLower();
                Palavra palavra = ent.Palavras.FirstOrDefault(plv => plv.Descricao == filtroCompare);
                //Se um dos filtros não for encontrado, retornar vazio
                if (palavra == null)
                    return new List<Musica>();
                palavrasFiltros.Add(palavra);
            }

            List<Musica> musicas = palavrasFiltros.First().MusicaPalavras.Select(mskPlv => mskPlv.Musica).ToList();
            //Recupera a interseccao de todas as musicasPalavras
            foreach (Palavra palavra in palavrasFiltros.Skip(1))
            {
                musicas = musicas.Intersect(palavra.MusicaPalavras.Select(mskPlv => mskPlv.Musica)).ToList();
            }

            Dictionary<Musica, List<string>> indicesPosMusica = new Dictionary<Musica, List<string>>();
            foreach (Musica msk in musicas)
            {
                indicesPosMusica.Add(msk, new List<string>());
                //indices da primeira palavra na musica corrente
                List<int> indicesGuia = palavrasFiltros.First().MusicaPalavras.First(mskPlv => mskPlv.Musica == msk).Indices.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(ind => Convert.ToInt32(ind)).ToList();
                foreach (Palavra palavra in palavrasFiltros.Skip(1))
                {
                    //Deve ser encontrada na posição seguinte
                    indicesGuia = indicesGuia.Select(indGuia => indGuia = indGuia + 1).ToList();
                    List<int> indicesMusicaPalavra = palavra.MusicaPalavras.First(mskPlv => mskPlv.Musica == msk).Indices.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(ind => Convert.ToInt32(ind)).ToList();
                    //Recupera a interseccao
                    indicesGuia = indicesGuia.Intersect(indicesMusicaPalavra).ToList();
                }

                if (indicesGuia.Count > 0)
                {
                    musicasRetorno.Add(msk);
                }
            }
            return musicasRetorno;
        }
    }
}
