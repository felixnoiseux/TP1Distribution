using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using movieGEN.Models;
using Newtonsoft.Json.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel;
using javax.jws;

namespace movieGEN.Controllers
{
    public class HomeController : Controller
    {
        private List<ImdbEntity> lstObj = new List<ImdbEntity>();
        private string OMDBKey = Environment.GetEnvironmentVariable("OMDBKEY");

        // GET: Home
        [Route("Home/Index")]
        public IActionResult Index(string search = "", int page = 1)
        {
            if (search != "")
            {
                while (search[search.Length - 1] == ' ') //Si il y a des espaces en trop dans la recherche, les enlèves
                {
                    search = search.Substring(0, search.Length - 1);
                }
            }

            if (search == "")
            {
                lstObj = Recherche();
            }
            else
            {
                string url = "http://www.omdbapi.com/?apikey="+OMDBKey+"&s=" + search + "&page=" + page; 
                using (WebClient wc = new WebClient())
                {
                    var json = wc.DownloadString(url); //Télécharge le Json retourné par la recherche
                    if (json != "{\"Response\":\"False\",\"Error\":\"Movie not found!\"}" && json != "{\"Response\":\"False\",\"Error\":\"Too many results.\"}")
                    {
                        //Recherche le Nombre total de résultat
                        int NbResultat = 0;
                        if (Int32.TryParse(json.Substring(json.Length - 24, 4), out int x))
                            NbResultat = x;
                        else if (Int32.TryParse(json.Substring(json.Length - 23, 3), out int y))
                            NbResultat = y;
                        else if (Int32.TryParse(json.Substring(json.Length - 22, 2), out int z))
                            NbResultat = z;
                        else if (Int32.TryParse(json.Substring(json.Length - 21, 1), out int z1))
                            NbResultat = z1;
                        
                        string sFinjson = json.Substring(json.Length - 50, 50);
                        float nb = NbResultat / 10.0f;
                        int NPage = NbResultat / 10;
                        if (nb % 1 != 0) //Ajoute une page si le résultat n'est pas 0 exacte, en int il arrondi
                            NPage++;

                        int posArrayFin = json.Length - 50 + sFinjson.IndexOf("]"); //Position de fin de l'array
                        if (json[posArrayFin + 1] == ',')
                        {
                            string s = json.Substring(10, posArrayFin - 9);
                            var jsonArray = JArray.Parse(s);
                            lstObj = jsonArray.ToObject<List<ImdbEntity>>();
                            if (page < NPage)
                                ViewData["NextPage"] = page + 1; //Envoie les données de la prochaine page
                            if (page != 1)
                                ViewData["PreviousPage"] = page - 1; //Envoie les données de la prochaine page
                        }
                        else //Il arrive qu'il n'y est pas de virgule si le résultat est trop petit
                        {
                            string s = json.Substring(10, json.Length - 50);
                            var jsonArray = JArray.Parse(s);
                            lstObj = jsonArray.ToObject<List<ImdbEntity>>();
                            if (page != 1)
                                ViewData["PreviousPage"] = page - 1;
                        }
                        ViewData["ActualPage"] = page;
                        ViewData["search"] = search; //Envoie le mot clé de recherche (constance)
                    }
                }
            }
            return View(lstObj);


        }
        [HttpPost]
        public void ChangeLangage(string langage) 
        {
            Environment.SetEnvironmentVariable("TARGET_LANGUAGE", langage);
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult ListEbay(string Title = "avenger")
        {
            bool pay = false;
            Title = Title.Replace("&", "%26");
            List<Ebay> ebays = new List<Ebay>();
            string url = "https://svcs.ebay.com/services/search/FindingService/v1";
            url += "?OPERATION-NAME=findItemsByKeywords";
            url += "&SERVICE-VERSION=1.0.0";
            url += "&SECURITY-APPNAME=SamuelDe-movieGEN-PRD-9dfe51816-c0152236";
            url += "&GLOBAL-ID=EBAY-US";
            url += "&RESPONSE-DATA-FORMAT=JSON";
            url += "&REST-PAYLOAD";
            url += "&keywords=" + Title;
            url += "&paginationInput.entriesPerPage=10";
            using (WebClient wc = new WebClient())
            {
                var json = wc.DownloadString(url);
                if (!json.Contains("totalEntries\":[\"0\"]"))//Si il y a des résultat
                {
                    //occurence,pos des Braquette
                    Dictionary<int, int> occurenceDebut = new Dictionary<int, int>();
                    Dictionary<int, int> occurenceFin = new Dictionary<int, int>();
                    int occurencedebutCount = 0;
                    int occurenceFinCount = 0;

                    for (int i = 0; i < json.Length - 1; i++)
                    {
                        if (json[i] == '[')//6
                        {
                            occurencedebutCount++;
                            occurenceDebut.Add(occurencedebutCount, i);
                        }
                        else if (json[i] == ']')//max - 9
                        {
                            occurenceFinCount++;
                            occurenceFin.Add(occurenceFinCount, i);
                        }
                    }
                    //Vrai position de départ de l'array de donnée 6 et max-8
                    string s = json.Substring(occurenceDebut.Where(p => p.Key == 6).Select(u => u.Value).FirstOrDefault(), (occurenceFin.Where(p => p.Key == occurenceFin.Count - 8).Select(u => u.Value).FirstOrDefault()) - (occurenceDebut.Where(p => p.Key == 6).Select(u => u.Value).FirstOrDefault() - 1));
                    s = s.Replace("[", "");
                    s = s.Replace("]", "");
                    string final = "[" + s + "]"; //Remet les braquettes pour la mise en array
                    for (int i = 0; i < final.Length - 13; i++)
                    {
                        if (final.Substring(i, 13) == "paymentMethod")// paymentMethod Ajout des braquettes d'array 
                        {
                            final = final.Insert(i + 15, "[");
                            i += 3;
                            pay = true;
                        }
                        if (final.Substring(i, 7) == "autoPay")//autoPay
                        {
                            if (pay == true)
                            {
                                final = final.Insert(i - 2, "]");
                                i += 3;
                                pay = false;
                            }
                        }
                    }
                    var jsonArray = JArray.Parse(final); //convertion
                    ebays = jsonArray.ToObject<List<Ebay>>();
                    return View(ebays);
                }
            }
            return View();
        }
        public IActionResult Details(string imdbID)
        {
            ImdbEntity imdb = new ImdbEntity();
            string url = "http://www.omdbapi.com/?apikey="+OMDBKey+"&i=" + imdbID;
            using (WebClient wc = new WebClient())
            {
                var json = wc.DownloadString(url);
                imdb = Newtonsoft.Json.JsonConvert.DeserializeObject<ImdbEntity>(json);
            }
            try
            {
                Run(imdb).Wait();
            }
            catch (AggregateException ex)
            {
                foreach (var e in ex.InnerExceptions)
                    Console.WriteLine("Error: " + e.Message);
            }
            return View(imdb);
        }
        private async Task Run(ImdbEntity imdb) //API de youtube en async
        {
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = "AIzaSyDbb9kUcOZUNxY-h5mgZjnOinkNbH5YYxo",
                ApplicationName = this.GetType().ToString()
            });
            var searchListRequest = youtubeService.Search.List("snippet");
            searchListRequest.Q = imdb.Title + " Official trailer"; // Recherche par titre pour bande annonce
            searchListRequest.MaxResults = 1; //S'assure d'avoir le résultat le plus pertinent
            var searchListResponse = await searchListRequest.ExecuteAsync();
            List<string> videos = new List<string>();

            // Récupère les vidéos correspondantes
            foreach (var searchResult in searchListResponse.Items)
            {
                switch (searchResult.Id.Kind)
                {
                    case "youtube#video":
                        videos.Add(String.Format(searchResult.Id.VideoId)); 
                        break;
                }
            }
            ViewData["URLYoutube"] = "https://www.youtube.com/watch?v=" + videos.FirstOrDefault(); //Renvoie l'url de la vidéo

        }
        public List<ImdbEntity> Recherche()
        {
            List<ImdbEntity> listInit = new List<ImdbEntity>();
            ImdbEntity obj = new ImdbEntity();
            List<string> URL = new List<string>() // Liste de film sur la page de départ par défaut
            {
                "http://www.omdbapi.com/?apikey="+OMDBKey+"&t=captain&page=1",
                "http://www.omdbapi.com/?apikey="+OMDBKey+"&t=Marvel&page=1",
                "http://www.omdbapi.com/?apikey="+OMDBKey+"&t=iron&page=1",
                "http://www.omdbapi.com/?apikey="+OMDBKey+"&t=Sword%20Art%20Online&page=1",
                "http://www.omdbapi.com/?apikey="+OMDBKey+"&t=The%20Fugitive&page=1",
                "http://www.omdbapi.com/?apikey="+OMDBKey+"&t=Fast%20&%20Furious%206&page=1",
                "http://www.omdbapi.com/?apikey="+OMDBKey+"&t=Blindspot&page=1",
                "http://www.omdbapi.com/?apikey="+OMDBKey+"&t=Home%20Alone&page=1",
                "http://www.omdbapi.com/?apikey="+OMDBKey+"&t=Bourne&page=1",
                "http://www.omdbapi.com/?apikey="+OMDBKey+"&t=avengers&page=1"
            };

            foreach (var url in URL)
            {
                using (WebClient wc = new WebClient())
                {
                    var json = wc.DownloadString(url);
                    obj = Newtonsoft.Json.JsonConvert.DeserializeObject<ImdbEntity>(json);
                    listInit.Add(obj);
                }
            }
            return listInit;
        }
    }
}