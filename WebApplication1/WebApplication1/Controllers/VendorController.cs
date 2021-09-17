using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.Views.Tools;

namespace WebApplication1.Controllers
{
    public class VendorController :  Controller 
    {
       public static Dictionary<int, Vendor> vendorDictonary = new Dictionary<int, Vendor>();

        //TODO: Relativen Pfad verwenden? 
        //Der Pfad wo das Json gespeichert ist
        public static string path = @"D:\VisualStudio2019\WebProgramieren\WebApplication1\WebApplication1\json\vendors.json";
 

        /// <summary>
        /// Zum auslesen der Json-Datei und dem Updaten
        /// unseres Vendor-Dic. zur Laufzeit
        /// </summary>
        public static void Deserilize()
        {
            if (System.IO.File.Exists(path))
            {
                string jsonstring = System.IO.File.ReadAllText(path);
                vendorDictonary = JsonConvert.DeserializeObject<Dictionary<int, Vendor>>(jsonstring);
            }

        }
  
        // Index der Vendors
        public ActionResult Index()
        {
            return View(vendorDictonary.Values);
        }


        // GET: Vendor/Details/5
        public ActionResult Details(int id)
        {
            var vendor = vendorDictonary[id];

            return View(vendor);
        }

        // GET: Vendor/Create
        public ActionResult Create()
        {
            return View();
        }

       /// <summary>
       /// In dieser Methode nehmen wir das Vendor Objekt entgegen,
       /// danach speichern wir es in unserem Vendor-Dic. und rufen danach die Save() auf
       /// </summary>
       /// <param name="vendor"></param>
       /// <returns></returns>
        public ActionResult CreatePost(Vendor vendor)
        {
            vendor.ID = vendorDictonary.Count + 1;
            vendor.IsActive = true;
            vendorDictonary.Add(vendor.ID, vendor);

            Helper.Save(path, vendorDictonary.ToDictionary(x => x.Key, x => (object)x.Value));

            return RedirectToAction("index");
        }

        // GET: Vendor/Edit/5
        public ActionResult Edit(int id)
        {
            return View(vendorDictonary[id]);
        }


        // TODO: Implement Vendor Immages
        public ActionResult EditPost(int id, string name, bool? isactive)
        {
            if (isactive == null)
            {
                vendorDictonary[id].IsActive = false;
            }
            else
            {
                vendorDictonary[id].IsActive = true;
            }

            vendorDictonary[id].Name = name;



            Helper.Save(path, vendorDictonary.ToDictionary(x => x.Key, x => (object)x.Value));

            return RedirectToAction("Index");
        }
        // GET: Vendor/Delete/5
        public ActionResult Delete(int id)
        {
            vendorDictonary[id].IsActive = false;
            Helper.Save(path, vendorDictonary.ToDictionary(x => x.Key, x => (object)x.Value));
            return RedirectToAction("index");
        }

        // TODO: Vendor Image import 
        
    }
}
