using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace PujcovnaKnih.Models
{
    public class Book
    {
        public int ID { get; set; }
        [Required, StringLength(50)]
        public string Title { get; set; }
        [Required, StringLength(50)]
        public string Author { get; set; }
        [Required, StringLength(30)]
        public string Genre { get; set; }
        [Required, DataType(DataType.Currency)]
        public decimal Price { get; set; }
        [Required]
        public string IsAvailable { get; set; }
    }
}