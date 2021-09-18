using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;
using System.Reflection;
using WebApplication1.Views.Tools;
using System.Drawing;

namespace WebApplication1.Controllers
{
    public class ProductController : Controller
    {




        public static Dictionary<int, Product> productDictionary = new Dictionary<int, Product>();
        static readonly string path = @"C:\Users\steve\OneDrive\Desktop\GithubRepos\WebApplication08\WebApplication1\WebApplication1\json\text.json";
        //static int filtered = 0;




        public static void Deserilize()
        {
            if (System.IO.File.Exists(path))
            {
                string jsonstring = System.IO.File.ReadAllText(path);
                productDictionary = JsonConvert.DeserializeObject<Dictionary<int, Product>>(jsonstring);
            }
        }

        public Dictionary<int, Product> Filter(string filter)
        {
            Dictionary<int, Product> sortThisDic = productDictionary;

            if (filter == "availabilityTrue" /*|| filtered == 1*/)
            {
                sortThisDic = productDictionary.Values.Where(x => x.Availability == true).ToDictionary(x => x.Id, x => x);

                //return View(sortThisDic);
                //filtered = 1;
            }
            else if (filter == "availabilityFalse" /*|| filtered == 2*/)
            {
                sortThisDic = productDictionary.Values.Where(x => x.Availability == false).ToDictionary(x => x.Id, x => x);

                // return View(filtered);
                //filtered = 2;
            }
            ViewBag.filter = filter;


            return sortThisDic;

        }


        public Dictionary<int, Product> Sortieren(Dictionary<int, Product> sortThisDic, string field, string orderby)
        {

            // Weil der Rückgabewert von OrderBy ein IOrderedEnumerable ist.
            // Man könnte wenn man nach OrderBy ( .ToDictionary ) es auch in einem weiterne Dictionary zwischenspeichern.
            IOrderedEnumerable<KeyValuePair<int, Product>> sortetDictIoe = null;
            ViewBag.field = field;
            ViewBag.orderby = orderby;
            if (orderby == null)
            {
                //sortetDic = productDictionary;
                return sortThisDic;
            }

            else if (orderby == "asc")
            {
                switch (field)
                {
                    case "id":
                        sortetDictIoe = sortThisDic.OrderBy(x => x.Value.Id);
                        break;
                    case "des":
                        sortetDictIoe = sortThisDic.OrderBy(x => x.Value.Description);
                        break;
                    case "price":
                        sortetDictIoe = sortThisDic.OrderBy(x => x.Value.Price);
                        break;
                    case "avar":
                        sortetDictIoe = sortThisDic.OrderBy(x => x.Value.Availability);
                        break;
                    default: throw new Exception("Wrong Parameter");

                }


            }
            else if (orderby == "desc")
            {
                switch (field)
                {
                    case "id":
                        sortetDictIoe = sortThisDic.OrderByDescending(x => x.Value.Id);
                        break;
                    case "des":
                        sortetDictIoe = sortThisDic.OrderByDescending(x => x.Value.Description);
                        break;
                    case "price":
                        sortetDictIoe = sortThisDic.OrderByDescending(x => x.Value.Price);
                        break;
                    case "avar":
                        sortetDictIoe = sortThisDic.OrderByDescending(x => x.Value.Availability);
                        break;
                    default:
                        throw new Exception("Wrong Parameter");

                }

            }


            if (sortetDictIoe != null)
            {
                sortThisDic = sortetDictIoe.ToDictionary(x => x.Key, x => x.Value);
            }

            return sortThisDic;
        }


        /// <summary>
        ///  Index Code-Behind
        /// </summary>
        /// <param name="field"></param>
        /// <param name="filter"></param>
        /// <param name="orderby"></param>
        /// <returns></returns>
        public ActionResult Index(string field, string orderby, string filter)
        {
            Dictionary<int, Product> sortThisDic = Filter(filter);

            sortThisDic = Sortieren(sortThisDic, field, orderby);


            return View(sortThisDic.Values);
        }



        /// <summary>
        /// Erzeugt eine View in der wir anhand eines Formulares die Daten dann an CreatePost weiterleiten
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            // Replaced by DropdownList
            //ViewBag.VendorList = VendorList();
            return View();
        }

        public string VendorList()
        {
            string done = "";

            foreach (Vendor item in VendorController.vendorDictonary.Values)
            {
                done += $@"<option value = ""{item.ID}""> {item.Name} </option> ";

            }

            return done;
        }


        // 
        /// <summary>
        /// In der CreatePost Methode erstellen wir zunächst ein neues Product()- Objekt
        /// Dieses Objekt wird dem Dictionary hinzugefügt.
        /// Danach wird gespeichert mit der Helper.Save Methode
        /// </summary>
        /// <param name="description"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        public ActionResult CreatePost(string description, decimal price, int vendorId)
        {
            Product p = new Product();
            p.Id = productDictionary.Count + 1; // Um eines erhöht nachdem er zugewiesen wurde
            p.Description = description;
            p.Price = price;
            p.Availability = true;
            productDictionary.Add(p.Id, p);
            p.Vendor = VendorController.vendorDictonary[vendorId];
            VendorController.vendorDictonary[vendorId].ProductList.Add(p);
            Helper.Save(path, productDictionary.ToDictionary(x => x.Key, x => (object)x.Value));
            Helper.Save(VendorController.path, VendorController.vendorDictonary.ToDictionary(x => x.Key, x => (object)x.Value));
            return RedirectToAction("index");
        }

        /// <summary>
        /// In Details überprüfen wir als erstes ob die liste leer ist.
        /// Falls dies der Fall ist Redirecten wir zu unserer FehlerView.
        /// ansonnsten suchen wir für jeden nach dem Produkt in jedem VendorObject.Productlist
        /// falls wir es finden übergeben wir es an den ViewBag.Vendor, welchen wir in der View dann aufrufen
        /// Weiters übergeben wir der View dann das Ditionary für die Ausgabe.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(int? id)
        {


            if (id == null)
            {
                return RedirectToAction("Fehler");
            }
            else
            {
                var product = productDictionary[(int)id];

                ViewBag.Vendor = product.Vendor;



                return View(product);
            }
        }

        public ActionResult Fehler()
        {
            return View();
        }

        /// <summary>
        /// In diesem ActionResult "löschen" (setzen die Acailability auf false)
        /// den per ID übergebenen Datensatz aus dem Dictonary
        /// /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(int id)
        {
            productDictionary[id].Availability = false;

            Helper.Save(path, productDictionary.ToDictionary(x => x.Key, x => (object)x.Value));

            return RedirectToAction("index");
        }


        /// <summary>
        /// Übergibt dem EditView unsere Productliste
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {

            return View(productDictionary[id]);
        }



        /// <summary>
        /// An diese Methode wird das Formular aus unserem Edit übergeben,
        /// Danach wird gespeichert mithilfer der "Helper.Save()"
        /// </summary>
        /// <param name="id"></param>
        /// <param name="description"></param>
        /// <param name="price"></param>
        /// <param name="availability"></param>
        /// <returns></returns>
        public ActionResult EditPost(int id, string description, string price, bool? availability, HttpPostedFileBase file)
        {
            FileUpload(file, id);
            if (availability == null)
            {
                productDictionary[id].Availability = false;
            }
            else
            {
                productDictionary[id].Availability = true;
            }

            productDictionary[id].Description = description;
            price = price.Replace('.', ',');
            Decimal.TryParse(price, out decimal price2);
            productDictionary[id].Price = price2;
           
         
           

            Helper.Save(path, productDictionary.ToDictionary(x => x.Key, x => (object)x.Value));


            return RedirectToAction("Index");
        }

        /// <summary>
        /// Übergibt dem View die Values unseres Dictionaryes
        /// </summary>
        /// <returns></returns>
        public ActionResult PictureGallery()
        {
            return View(productDictionary.Values);
        }


        /// <summary>
        /// Gennerriert eien Table, übernimmt unsere ImagePaths und springt alle 3 Bilder in eine neue Zeile
        /// </summary>
        /// <returns>Html Table String</returns>
        public string GenerateImageGaleryHTML()
        {
            string ausgabe = "<table>";
            ausgabe += "<tr>";



            int zaehler = 0;
            foreach (var img in productDictionary)
            {
                if (zaehler == 3)
                {
                    ausgabe += "</tr> \r\n <tr> \r\n";
                    zaehler = 0;
                }



                ausgabe += "<td>" + img.Value.GetImagePath(100) + "</td>";
                zaehler++;
            }

            ausgabe += "</tr></table>";

            return ausgabe;
        }

        /// <summary>
        /// Hier rufen wir die Methode GenerateImageGaleryHTML() auf und geben sie in einen ViewBag
        /// </summary>
        /// <returns></returns>
        public ActionResult PictureGalleryAlt()
        {
            ViewBag.ausgabe = GenerateImageGaleryHTML();
            return View();
        }


        // Ausgeben der TopNProdukte
        /// <summary>
        /// Gibt die ersten 5 Produkte aus. (Hardcodet)
        /// </summary>
        /// <returns></returns>
        public ActionResult TopNProducts()
        {
            int count = 5;


            // Sortieren per Lambda, mit Take bekomme ich die ersten (count) Datensätze, falls der count OOI liegt werden die vornhandenen Elemente
            // genommen die verfügbar sind.
            var topProducts = productDictionary.OrderByDescending(x => x.Value.Price).Take(count);
            var topProductsDict = topProducts.ToDictionary(x => x.Key, x => x.Value);


            return View(topProductsDict.Values);
        }



        /// <summary>
        /// Der View zu unserer File-Upload-Page
        /// Wir müssen ihm eine ID mitgeben damit wir das (in unserem Fall)
        /// Bild später einem Produkt zuordnen können.
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public ActionResult UploadPicture(int productId)
        {
            ViewBag.productId = productId;
            return View();
        }


        /// <summary>
        /// Hier Verarbeiten wir die Hochgeladene Datei.
        /// Wir überprüfen folgende Fälle:
        /// Ist die Datei Leer?
        /// Ist das Dateivormat ein jpeg/jpg oder png?
        /// Wird die von uns festgelegte Maximalgröße nicht überschritten?
        /// Bei Erfolg Speichern wir das Bild.
        /// Bei Misserfolg Redirecten wir zur ErrorPage
        /// </summary>
        /// <param name="file"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public ActionResult FileUpload(HttpPostedFileBase file, int productId)
        {
            string check = "";
            
            if (file != null)
            {
                check = file.ContentType;

            }

            if (file != null && file.ContentLength <= 20 * 1024 * 1024 && check == "image/jpeg" || check == "image/png")
            {
                file.SaveAs($@"C:\Users\steve\OneDrive\Desktop\GithubRepos\WebApplication08\WebApplication1\WebApplication1\images\{productId}.jpg");
                return RedirectToAction("index");
            }
            //else if(System.IO.File.Exists($@"C:\Users\steve\OneDrive\Desktop\GithubRepos\WebApplication08\WebApplication1\WebApplication1\images\{productId}.jpg"))
            //{
            //    return RedirectToAction("index");
            //}
            else
            {
                return RedirectToAction("Fehler");
            }

        }


        
    }
}