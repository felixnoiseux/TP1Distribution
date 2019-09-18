using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
                        //char[] s1 = new char[100];
                        //int i1 = 0;
                        //for (int i = json.Length-1; i > json.Length - 50; i--)
                        //{
                        //    s1[i1] = json[i];
                        //    i1++;
                        //}
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
        public IActionResult Details(string imdbID)
        {
            ImdbEntity imdb = new ImdbEntity();
            string url = "http://www.omdbapi.com/?apikey=c8f45984&i=" + imdbID;
            using (WebClient wc = new WebClient())
            {
                var json = wc.DownloadString(url);

                imdb = Newtonsoft.Json.JsonConvert.DeserializeObject<ImdbEntity>(json);
            }
            return View(imdb);
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