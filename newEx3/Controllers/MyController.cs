using newEx3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace newEx3.Controllers
{
    public class MyController : Controller
    {
        private Instructions instructions;
        // GET: My
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult display(string ip, int port, int time)
        {
            InfoModel.Instance.ip = ip;
            InfoModel.Instance.port = port.ToString();
            InfoModel.Instance.time = time;
            // open the server with the ip and port parameters after assign them.
            InfoModel.Instance.OpenServer();
            double lon = InfoModel.Instance.GetLon();
            double lat = InfoModel.Instance.GetLat();
            Console.WriteLine("lon - " + lon, "lat - " + lat);
            // save the time between requests.
            Session["time"] = time;
            ViewBag.lon = lon;
            ViewBag.lat = lat;
            return View();
        }
    }
}