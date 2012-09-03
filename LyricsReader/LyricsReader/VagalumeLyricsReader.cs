using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using LyricsReader.Model;
using System.Threading;

namespace LyricsReader
{
    public class VagalumeLyricsReader
    {
        //string urlEstilo = "http://www.vagalume.com.br/browse/style/mpb.html";
        List<char> letrasAlfabeto = new List<char>() { 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
        List<string> urlsEstilos = new List<string>() { 
            "http://www.vagalume.com.br/browse/style/mpb.html",
            "http://www.vagalume.com.br/browse/style/pagode.html",
            "http://www.vagalume.com.br/browse/style/samba.html",
            "http://www.vagalume.com.br/browse/style/samba-enredo.html",
            "http://www.vagalume.com.br/browse/style/sertanejo.html",
            "http://www.vagalume.com.br/browse/style/forro.html",
            "http://www.vagalume.com.br/browse/style/axe.html"
        };
        List<Musica> musicas = new List<Musica>();
        Mutex mut = new Mutex();

        public List<Musica> LerMusicas(ref string erros)
        {
            try
            {
                List<Thread> threads = new List<Thread>();

                foreach (string urlEstilo in urlsEstilos)
                {
                    string urlEstiloFixThread = urlEstilo;
                    Thread thread = new Thread(unused => RecuperarMusicasPorEstilo(urlEstiloFixThread));
                    thread.Start();
                    threads.Add(thread);
                }

                threads.ForEach(thread => thread.Join());

                return musicas;
            }
            catch (Exception ex)
            {
                erros = ex.Message.ToString();
                return null;
            }
        }

        #region Filtros para o estilo

        //private string RecuperarHtmlCantoresPorEstilo(string urlEstilo)
        //{
        //    string htmlResult;
        //    using (WebClient wc = new WebClient())
        //    {
        //        wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
        //        htmlResult = wc.DownloadString(urlEstilo);
        //    }
        //    return htmlResult;
        //}

        private void RecuperarMusicasPorEstilo(string urlEstilo)
        {
            List<string> urlsCantoresEstilo = FiltrarUrlsCantores(urlEstilo);
            urlsCantoresEstilo = urlsCantoresEstilo.Where(url => letrasAlfabeto.Contains(url.ToLower()[27])).ToList();

            foreach (string urlCantor in urlsCantoresEstilo)
            {
                List<string> urlsMusicasCantor = RecuperarUrlsMusicasCantor(urlCantor);

                string nomeCantor = urlCantor.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries).Last();
                List<Musica> musicasCantor = LerMusicasCantor(nomeCantor, urlsMusicasCantor);

                mut.WaitOne();
                musicas.AddRange(musicasCantor);
                mut.ReleaseMutex();
            }
        }

        private List<string> FiltrarUrlsCantores(string urlEstilo)
        {
            string html = RecuperarHtml(urlEstilo);

            html = html.Split(new string[] { "<div id=\"browseTops\">" }, StringSplitOptions.RemoveEmptyEntries).Last();
            html = html.Split(new string[] { "<a name=\"inicio\"></a>" }, StringSplitOptions.RemoveEmptyEntries).Last();
            html = html.Split(new string[] { "<span class=\"ending\">", "<span class='ending'>" }, StringSplitOptions.RemoveEmptyEntries).First();

            List<string> urls = html.Split(new string[] { "<a href=" }, StringSplitOptions.RemoveEmptyEntries).Skip(1).ToList();
            urls = urls.Select(url => "http://www.vagalume.com.br" + url.Split(' ', '>').First()).ToList();
            return urls;
        }

        #endregion Filtros para o estilo

        #region Filtros para o Artista

        private List<string> RecuperarUrlsMusicasCantor(string urlCantor)
        {
            string html = RecuperarHtml(urlCantor);
            if (!html.Contains("<div id=\"artist-songlist\">"))
                return new List<string>();

            html = html.Split(new string[] { "<div id=\"artist-songlist\">" }, StringSplitOptions.RemoveEmptyEntries).Last();
            html = html.Split(new string[] { "</div>" }, StringSplitOptions.RemoveEmptyEntries).First();

            List<string> urls = html.Split(new string[] { "href=" }, StringSplitOptions.RemoveEmptyEntries).Skip(1).ToList();
            urls = urls.Select(url => "http://www.vagalume.com.br" + url.Split(' ', '>').First()).ToList();
            return urls;
        }

        #endregion Filtros para o Artista

        #region Filtros para a musica

        private List<Musica> LerMusicasCantor(string nomeCantor, List<string> urlsMusicasCantor)
        {
            List<Musica> musicas = new List<Musica>();
            foreach (string urlMusica in urlsMusicasCantor)
            {
                string nomeMusica = urlMusica.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries).Last().Split(new string[] { ".html" }, StringSplitOptions.RemoveEmptyEntries).First();
                Musica musica = new Musica() { Cantor = nomeCantor, NomeMusica = nomeMusica };
                string html = RecuperarHtml(urlMusica);

                if (!html.Contains("<div class=\"editable_area\""))
                    continue;

                string letraComTags = html.Split(new string[] { "<div class=\"editable_area\"" }, StringSplitOptions.RemoveEmptyEntries)[1];
                letraComTags = letraComTags.Substring(letraComTags.IndexOf('>') + 1);
                letraComTags = letraComTags.Split(new string[] { "</div>" }, StringSplitOptions.RemoveEmptyEntries)[0];
                string letra = EliminarTagsHtml(letraComTags);

                musica.Letra = letra;

                musicas.Add(musica);
                ProjetoIndexacaoDBEntities ent = new ProjetoIndexacaoDBEntities();
                ent.Musicas.AddObject(musica);
                ent.SaveChanges();
            }

            return musicas;
        }

        #endregion Filtros para a musica

        private string EliminarTagsHtml(string letraComTags)
        {
            letraComTags = letraComTags.Replace("<br/>", "\n");
            EliminarTagsHtmlRecursiva(letraComTags);
            return letraComTags;
        }

        private string EliminarTagsHtmlRecursiva(string letraComTags)
        {
            if (letraComTags.Contains('<'))
            {
                letraComTags = letraComTags.Substring(0, letraComTags.IndexOf('<')) + letraComTags.Substring(letraComTags.IndexOf('>') + 1);
                EliminarTagsHtml(letraComTags);
            }

            return letraComTags;
        }

        private string RecuperarHtml(string urlCantor)
        {
            string htmlResult;
            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                htmlResult = wc.DownloadString(urlCantor);
            }
            return htmlResult;
        }
    }
}


