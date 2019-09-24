using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Microsoft.AspNetCore.Mvc;
using movieGEN.Models;
using Newtonsoft.Json.Linq;
namespace movieGEN.Controllers
{

    public class HomeController : Controller
    {
        List<ImdbEntity> lstObj = new List<ImdbEntity>();
        // GET: Home
        [Route("Home/Index")]
        public IActionResult Index(string search = "", int page = 1)
        {
            ImdbEntity obj = new ImdbEntity();
            if (search != "")
            {
                while (search[search.Length - 1] == ' ')
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
                string url = "http://www.omdbapi.com/?apikey=c8f45984&s=" + search + "&page=" + page;
                using (WebClient wc = new WebClient())
                {
                    var json = wc.DownloadString(url);
                    if (json != "{\"Response\":\"False\",\"Error\":\"Movie not found!\"}" && json != "{\"Response\":\"False\",\"Error\":\"Too many results.\"}")
                    {
                        int NbResultat = 0;
                        if (Int32.TryParse(json.Substring(json.Length - 24, 4), out int x))
                        {
                            NbResultat = x;

                        }
                        else if (Int32.TryParse(json.Substring(json.Length - 23, 3), out int y))
                        {
                            NbResultat = y;
                        }
                        else if (Int32.TryParse(json.Substring(json.Length - 22, 2), out int z))
                        {
                            NbResultat = z;
                        }
                        else if (Int32.TryParse(json.Substring(json.Length - 21, 1), out int z1))
                        {
                            NbResultat = z1;
                        }
                        string sFinjson = json.Substring(json.Length - 50, 50);
                        float nb = NbResultat / 10.0f;
                        int NPage = NbResultat / 10;
                        if (nb % 1 != 0)
                        {
                            NPage++;
                        }

                        int posArrayFin = json.Length - 50 + sFinjson.IndexOf("]");
                        if (json[posArrayFin + 1] == ',')
                        {
                            string s = json.Substring(10, posArrayFin - 9);
                            var jsonArray = JArray.Parse(s);
                            lstObj = jsonArray.ToObject<List<ImdbEntity>>();
                            if (page < NPage)
                            {
                                ViewData["NextPage"] = page + 1;
                            }
                            if (page != 1)
                            {
                                ViewData["PreviousPage"] = page - 1;
                            }
                        }
                        else
                        {
                            string s = json.Substring(10, json.Length - 50);
                            var jsonArray = JArray.Parse(s);
                            lstObj = jsonArray.ToObject<List<ImdbEntity>>();
                            if (page != 1)
                            {
                                ViewData["PreviousPage"] = page - 1;
                            }
                        }
                        ViewData["ActualPage"] = page;
                        ViewData["search"] = search;
                    }

                }

            }
            return View(lstObj);


        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult ListEbay(string Title = "avenger")
        {
            ViewData["Film"] = Title;
            return View();
        }
        public IActionResult Details(string imdbID)
        {
            ImdbEntity imdb = new ImdbEntity();
            string url = "http://www.omdbapi.com/?apikey=c8f45984&i=" + imdbID;
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
                {
                    Console.WriteLine("Error: " + e.Message);
                }
            }

            return View(imdb);
        }
        private async Task Run(ImdbEntity imdb)
        {
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = "AIzaSyDbb9kUcOZUNxY-h5mgZjnOinkNbH5YYxo",
                ApplicationName = this.GetType().ToString()
            });
            //https://www.youtube.com/watch?v=
            var searchListRequest = youtubeService.Search.List("snippet");
            searchListRequest.Q = imdb.Title + " Official trailer"; // Replace with your search term.
            searchListRequest.MaxResults = 1;

            // Call the search.list method to retrieve results matching the specified query term.
            var searchListResponse = await searchListRequest.ExecuteAsync();

            List<string> videos = new List<string>();

            // Add each result to the appropriate list, and then display the lists of
            // matching videos, channels, and playlists.
            foreach (var searchResult in searchListResponse.Items)
            {
                switch (searchResult.Id.Kind)
                {
                    case "youtube#video":
                        videos.Add(String.Format(searchResult.Id.VideoId));
                        break;
                }
            }
            ViewData["URLYoutube"] = "https://www.youtube.com/watch?v=" + videos.FirstOrDefault();

        }
        public List<ImdbEntity> Recherche()
        {
            List<ImdbEntity> listInit = new List<ImdbEntity>();
            ImdbEntity obj = new ImdbEntity();
            List<string> URL = new List<string>()
            {
                "http://www.omdbapi.com/?apikey=c8f45984&t=captain&page=1",
                "http://www.omdbapi.com/?apikey=c8f45984&t=Marvel&page=1",
                "http://www.omdbapi.com/?apikey=c8f45984&t=iron&page=1",
                "http://www.omdbapi.com/?apikey=c8f45984&t=Sword%20Art%20Online&page=1",
                "http://www.omdbapi.com/?apikey=c8f45984&t=The%20Fugitive&page=1",
                "http://www.omdbapi.com/?apikey=c8f45984&t=Fast%20&%20Furious%206&page=1",
                "http://www.omdbapi.com/?apikey=c8f45984&t=Blindspot&page=1",
                "http://www.omdbapi.com/?apikey=c8f45984&t=Home%20Alone&page=1",
                "http://www.omdbapi.com/?apikey=c8f45984&t=Bourne&page=1",
                "http://www.omdbapi.com/?apikey=c8f45984&t=avengers&page=1"
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