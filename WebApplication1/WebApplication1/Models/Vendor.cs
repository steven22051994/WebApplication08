using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using Newtonsoft.Json;

namespace WebApplication1.Models
{
    // Diese Anotation erlaubt es uns ohne Fehler die Referrenzierten Objekte mit abzuspeichern
    // First Reffernece
    // https://stackoverflow.com/questions/7397207/json-net-error-self-referencing-loop-detected-for-type
    // Second Refference
    //https://www.newtonsoft.com/json/help/html/preserveobjectreferences.htm
    [JsonObject(IsReference = true)] 
    public class Vendor
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
     
        public List<Product> ProductList = new List<Product>();


    }
}