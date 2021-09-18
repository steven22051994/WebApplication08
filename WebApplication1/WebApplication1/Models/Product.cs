using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    // Diese Anotation erlaubt es uns ohne Fehler die Referrenzierten Objekte mit abzuspeichern
    // First Reffernece
    // https://stackoverflow.com/questions/7397207/json-net-error-self-referencing-loop-detected-for-type
    // Second Refference
    //https://www.newtonsoft.com/json/help/html/preserveobjectreferences.htm
    [JsonObject(IsReference = true)]
    public class Product
    {

        public int Id { get; set; }
        // Mit hilfe von Anotationen und REGEX können wir steuern welche Eingabe valide ist.
        [Required(ErrorMessage = "Du musst da was angeben :smh:")]
        [MaxLength(100, ErrorMessage = "Ist zu lang :smh:")]
        [MinLength(2, ErrorMessage = "Ist zu kurz :smh:")]
        public string Description { get; set; }
        
      
        [RegularExpression(@"^[0-9]*.[0-9]{2}$", ErrorMessage = "Gültige Zahl angeben")]
        public decimal Price { get; set; }

        public bool Availability { get; set; }
        // TODO: Eigenschaft entfernen und alles über die Methode GetImagePath() laufen lassen
        //public string ImagePath
        //{
        //    get
        //    {

        //        string path = $@"D:\VisualStudio2019\WebProgramieren\WebApplication1\WebApplication1\images\{this.Id}.jpg";
        //        if (System.IO.File.Exists(path))
        //        {
        //            return $"<img alt={this.Description} height=100 src=/images/" + this.Id + ".jpg>";
        //        }

        //        return $"<img alt={this.Description} height=100 src=/images/default.jpg>";

        //    }
        //}


        public string GetImagePath(int height)
        {

            string path = $@"C:\Users\steve\OneDrive\Desktop\GithubRepos\WebApplication08\WebApplication1\WebApplication1\images\{this.Id}.jpg";
            if (System.IO.File.Exists(path))
            {
                
                return $"<img alt={this.Description} height=" + height + " src=/images/"+this.Id+".jpg>";
            }
            return $"<img alt={this.Description} height=" + height + " src=/images/default.jpg>";
        }

        public Vendor Vendor { get; set; }





    }
}