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
        // GET: Home
        [Route("Home/Index")]
        public IActionResult Index(string recherche = "", int page = 1)
        {
            ImdbEntity obj = new ImdbEntity();
            List<ImdbEntity> lstObj = new List<ImdbEntity>();
            if (recherche == "")
            {
                lstObj = Recherche();
            }
            else
            {
                string url = "http://www.omdbapi.com/?apikey=c8f45984&s=" + recherche + "&page=" + page;
                using (WebClient wc = new WebClient())
                {
                    var json = wc.DownloadString(url);

                    foreach (var item in json)
                    {
                        obj = Newtonsoft.Json.JsonConvert.DeserializeObject<ImdbEntity>(item.ToString());
                        lstObj.Add(obj);
                    }
                }

            }

            return View(lstObj);


        }
        public IActionResult About()
        {
            return View();
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
                "http://www.omdbapi.com/?apikey=c8f45984&t=Star%20Wars%207&page=1",
                "http://www.omdbapi.com/?apikey=c8f45984&t=The%20Fugitive&page=1",
                "http://www.omdbapi.com/?apikey=c8f45984&t=Fast%20&%20Furious%206&page=1",
                "http://www.omdbapi.com/?apikey=c8f45984&t=BlackList&page=1",
                "http://www.omdbapi.com/?apikey=c8f45984&t=Blindspot&page=1",
                "http://www.omdbapi.com/?apikey=c8f45984&t=Home%20Alone&page=1",
                "http://www.omdbapi.com/?apikey=c8f45984&t=Bourne&page=1"
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