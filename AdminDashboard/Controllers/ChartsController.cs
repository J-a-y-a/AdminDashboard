using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static AdminDashboard.Chart;

namespace AdminDashboard.Controllers
{
    public class ChartsController : Controller
    {
        // GET: Charts
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Charts()
        {
            List<DataPoint> dataPoints = new List<DataPoint>();

            dataPoints.Add(new DataPoint("Samsung", 25));
            dataPoints.Add(new DataPoint("Micromax", 13));
            dataPoints.Add(new DataPoint("Lenovo", 8));
            dataPoints.Add(new DataPoint("Intex", 7));
            dataPoints.Add(new DataPoint("Reliance", 6.8));
            dataPoints.Add(new DataPoint("Others", 40.2));

            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);

            return View();
        }
    }
}