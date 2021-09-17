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
    [JsonObject(IsReference = true)]
    public class Product
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "Du musst da was angeben :smh:")]
        [MaxLength(100, ErrorMessage = "Ist zu lang :smh:")]
        [MinLength(2, ErrorMessage = "Ist zu kurz :smh:")]
        public string Description { get; set; }
        [RegularExpression(@"^[0-9]+[,|\.]+([0-9]{1,2})?$", ErrorMessage = "Gültige Zahl angeben")]
        public decimal Price { get; set; }

        public bool Availability { get; set; }

        public string ImagePath
        {
            get
            {

                string path = $@"D:\VisualStudio2019\WebProgramieren\WebApplication1\WebApplication1\images\{this.Id}.jpg";
                if (System.IO.File.Exists(path))
                {
                    return $"<img alt={this.Description} height=100 src=/images/" + this.Id + ".jpg>";
                }

                return $"<img alt={this.Description} height=100 src=/images/default.jpg>";

            }
        }


        public string GetImagePath(int height)
        {

            string path = $@"D:\VisualStudio2019\WebProgramieren\WebApplication1\WebApplication1\images\{this.Id}.jpg";
            if (System.IO.File.Exists(path))
            {
                return $"<img alt={this.Description} height=" + height + " src=/images/"+this.Id+".jpg>";
            }

            return $"<img alt={this.Description} height=" + height + " src=/images/default.jpg>";


        }

        public Vendor Vendor { get; set; }





    }
}