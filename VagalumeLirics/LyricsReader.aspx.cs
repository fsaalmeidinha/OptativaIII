using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;

public partial class LyricsReader : System.Web.UI.Page
{
    //http://www.vagalume.com.br/browse/style/mpb.html
    //Procurar nessa URL

    string urls = "http://www.vagalume.com.br/gusttavo-lima/60-segundos.html|||http://www.vagalume.com.br/gusttavo-lima/a-promessa.html|||http://www.vagalume.com.br/gusttavo-lima/aceita-ou-nao-aceita.html|||http://www.vagalume.com.br/gusttavo-lima/agua-de-bar.html|||http://www.vagalume.com.br/gusttavo-lima/alo-cuida-de-mim-sem-ar.html|||http://www.vagalume.com.br/gusttavo-lima/amor-de-poeta.html|||http://www.vagalume.com.br/gusttavo-lima/amor-de-primavera.html|||http://www.vagalume.com.br/gusttavo-lima/amor-ou-ilusao.html|||http://www.vagalume.com.br/gusttavo-lima/amor-perigoso.html|||http://www.vagalume.com.br/gusttavo-lima/arrasta.html|||http://www.vagalume.com.br/gusttavo-lima/as-mina-pira.html|||http://www.vagalume.com.br/gusttavo-lima/as-vezes-sim-as-vezes-nao.html|||http://www.vagalume.com.br/gusttavo-lima/ate-amanhecer.html|||http://www.vagalume.com.br/gusttavo-lima/balada-boa-tche-tche-rere.html|||http://www.vagalume.com.br/gusttavo-lima/bandida-sem-vergonha.html|||http://www.vagalume.com.br/gusttavo-lima/besteirinha.html|||http://www.vagalume.com.br/gusttavo-lima/bilauzinho.html|||http://www.vagalume.com.br/gusttavo-lima/cachaca-com-limao.html|||http://www.vagalume.com.br/gusttavo-lima/calafrio.html|||http://www.vagalume.com.br/gusttavo-lima/camila.html|||http://www.vagalume.com.br/gusttavo-lima/caso-consumado.html|||http://www.vagalume.com.br/gusttavo-lima/catireiro-safado.html|||http://www.vagalume.com.br/gusttavo-lima/chega-mais-pra-ca.html|||http://www.vagalume.com.br/gusttavo-lima/chega-pra-ca.html|||http://www.vagalume.com.br/gusttavo-lima/chora-chora.html|||http://www.vagalume.com.br/gusttavo-lima/chovendo-paixao.html|||http://www.vagalume.com.br/gusttavo-lima/chuva-de-amor.html|||http://www.vagalume.com.br/gusttavo-lima/coca-cola.html|||http://www.vagalume.com.br/gusttavo-lima/cor-de-ouro.html|||http://www.vagalume.com.br/gusttavo-lima/coracao-revelacao.html|||http://www.vagalume.com.br/gusttavo-lima/curticao.html|||http://www.vagalume.com.br/gusttavo-lima/demais-da-conta.html|||http://www.vagalume.com.br/gusttavo-lima/doidaca.html|||http://www.vagalume.com.br/gusttavo-lima/e-ai.html|||http://www.vagalume.com.br/gusttavo-lima/e-fato.html|||http://www.vagalume.com.br/gusttavo-lima/e-sempre-assim.html|||http://www.vagalume.com.br/gusttavo-lima/entao-ta.html|||http://www.vagalume.com.br/gusttavo-lima/entrada-franca.html|||http://www.vagalume.com.br/gusttavo-lima/espumas-ao-vento-ao-vivo-com-elba-ramalho.html|||http://www.vagalume.com.br/gusttavo-lima/esse-amor.html|||http://www.vagalume.com.br/gusttavo-lima/eu-menti.html|||http://www.vagalume.com.br/gusttavo-lima/eu-te-achei.html|||http://www.vagalume.com.br/gusttavo-lima/eu-te-quero-sim.html|||http://www.vagalume.com.br/gusttavo-lima/eu-volto-pra-voce.html|||http://www.vagalume.com.br/gusttavo-lima/eu-vou.html|||http://www.vagalume.com.br/gusttavo-lima/eu-vou-contar-pro-ceis.html|||http://www.vagalume.com.br/gusttavo-lima/eu-vou-tentando-te-agarrar.html|||http://www.vagalume.com.br/gusttavo-lima/fazer-beber.html|||http://www.vagalume.com.br/gusttavo-lima/fora-do-comum.html|||http://www.vagalume.com.br/gusttavo-lima/fui-palhaco.html|||http://www.vagalume.com.br/gusttavo-lima/furacao.html|||http://www.vagalume.com.br/gusttavo-lima/gatinha-assanhada.html|||http://www.vagalume.com.br/gusttavo-lima/inventor-dos-amores.html|||http://www.vagalume.com.br/gusttavo-lima/joga-as-maos.html|||http://www.vagalume.com.br/gusttavo-lima/labios-divididos.html|||http://www.vagalume.com.br/gusttavo-lima/larguei-de-ser-besta.html|||http://www.vagalume.com.br/gusttavo-lima/linda-demais.html|||http://www.vagalume.com.br/gusttavo-lima/linda-flor.html|||http://www.vagalume.com.br/gusttavo-lima/lua.html|||http://www.vagalume.com.br/gusttavo-lima/mal-de-amor.html|||http://www.vagalume.com.br/gusttavo-lima/mamae-falou.html|||http://www.vagalume.com.br/gusttavo-lima/me-deixe-em-paz.html|||http://www.vagalume.com.br/gusttavo-lima/menina.html|||http://www.vagalume.com.br/gusttavo-lima/meu-destino.html|||http://www.vagalume.com.br/gusttavo-lima/mineirinha.html|||http://www.vagalume.com.br/gusttavo-lima/na-hora-h-passou-da-conta-me-apaixonei-da-boca-pra-fora.html|||http://www.vagalume.com.br/gusttavo-lima/nao-va-chamar-de-traicao.html|||http://www.vagalume.com.br/gusttavo-lima/nao-vou-mais-chorar.html|||http://www.vagalume.com.br/gusttavo-lima/no-mundo-da-lua.html|||http://www.vagalume.com.br/gusttavo-lima/o-amor-nao-foi-feito-pra-dividir.html|||http://www.vagalume.com.br/gusttavo-lima/o-carona.html|||http://www.vagalume.com.br/gusttavo-lima/o-mundo-e-uma-bola.html|||http://www.vagalume.com.br/gusttavo-lima/o-nosso-amor-venceu.html|||http://www.vagalume.com.br/gusttavo-lima/paraquedas.html|||http://www.vagalume.com.br/gusttavo-lima/paredao.html|||http://www.vagalume.com.br/gusttavo-lima/paulinha.html|||http://www.vagalume.com.br/gusttavo-lima/pecado-de-amor.html|||http://www.vagalume.com.br/gusttavo-lima/planeta-solidao.html|||http://www.vagalume.com.br/gusttavo-lima/pode-ir-embora.html|||http://www.vagalume.com.br/gusttavo-lima/prazo-de-validade.html|||http://www.vagalume.com.br/gusttavo-lima/prefiro-a-solidao.html|||http://www.vagalume.com.br/gusttavo-lima/principe-encantado.html|||http://www.vagalume.com.br/gusttavo-lima/que-nao-da-que-nao-deu.html|||http://www.vagalume.com.br/gusttavo-lima/quebrei-a-cara.html|||http://www.vagalume.com.br/gusttavo-lima/quem-tem-sorte-e-sortero.html|||http://www.vagalume.com.br/gusttavo-lima/quero-te-pegar.html|||http://www.vagalume.com.br/gusttavo-lima/quimica-do-nosso-amor.html|||http://www.vagalume.com.br/gusttavo-lima/realidade.html|||http://www.vagalume.com.br/gusttavo-lima/refem.html|||http://www.vagalume.com.br/gusttavo-lima/revelacao.html|||http://www.vagalume.com.br/gusttavo-lima/rosas-versos-e-vinhos.html|||http://www.vagalume.com.br/gusttavo-lima/saudade.html|||http://www.vagalume.com.br/gusttavo-lima/sem-voce-eu-morro.html|||http://www.vagalume.com.br/gusttavo-lima/sete-mares.html|||http://www.vagalume.com.br/gusttavo-lima/so-voce-nao-ve.html|||http://www.vagalume.com.br/gusttavo-lima/sou-foda.html|||http://www.vagalume.com.br/gusttavo-lima/sou-seu-fa-com-israel-e-rodolffo.html|||http://www.vagalume.com.br/gusttavo-lima/super-homem-com-edson.html|||http://www.vagalume.com.br/gusttavo-lima/tem-alguem-em-seu-lugar.html|||http://www.vagalume.com.br/gusttavo-lima/toc-da-ploc.html|||http://www.vagalume.com.br/gusttavo-lima/tornado.html|||http://www.vagalume.com.br/gusttavo-lima/tsunami.html|||http://www.vagalume.com.br/gusttavo-lima/ultima-lagrima.html|||http://www.vagalume.com.br/gusttavo-lima/um-grande-amor-nao-pode-acabar-assim.html|||http://www.vagalume.com.br/gusttavo-lima/unidos-para-sempre.html|||http://www.vagalume.com.br/gusttavo-lima/universo-alegria.html|||http://www.vagalume.com.br/gusttavo-lima/vai-nao-vai.html|||http://www.vagalume.com.br/gusttavo-lima/vem-ni-mim-dodge-ram.html|||http://www.vagalume.com.br/gusttavo-lima/viva-intensamente.html|||http://www.vagalume.com.br/gusttavo-lima/voce-jogou-fora.html|||http://www.vagalume.com.br/gusttavo-lima/volta-pra-mim.html";
    protected void Page_Load(object sender, EventArgs e)
    {
        List<Musica> musicas = new List<Musica>();
        foreach (string url in urls.Split(new string[] { "|||" }, StringSplitOptions.RemoveEmptyEntries))
        {
            Musica musica = new Musica(); musica.Cantor = "gusttavo_lima";
            musica.NomeMusica = RecuperarNomeMusicaDaUrl(url);
            string htmlResult = "";
            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                htmlResult = wc.DownloadString(url);

            } string letraComTags = htmlResult.Split(new string[] { "<div class=\"editable_area\"" }, StringSplitOptions.RemoveEmptyEntries)[1];
            letraComTags = letraComTags.Substring(letraComTags.IndexOf('>') + 1);
            letraComTags = letraComTags.Split(new string[] { "</div>" }, StringSplitOptions.RemoveEmptyEntries)[0];
            musica.Letra = EliminarTagsHtml(letraComTags);

            musicas.Add(musica);
        }
    }

    private string RecuperarNomeMusicaDaUrl(string url)
    {
        return url.Split('/').Last().Split('.').First();
    }

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

    private class Musica
    {
        public string Cantor { get; set; }
        public string NomeMusica { get; set; }
        public string Letra { get; set; }
    }
}