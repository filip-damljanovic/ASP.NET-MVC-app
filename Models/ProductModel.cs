using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC_app.Models
{
    public class ProductModel
    {
        public int ID { get; set; }
        public string Naziv { get; set; }

        [DataType(DataType.MultilineText)]
        public string Opis { get; set; }
        public string Kategorija { get; set; }
        public string Proizvodjac { get; set; }
        public string Dobavljac { get; set; }
        public decimal Cena { get; set; }

    }
}