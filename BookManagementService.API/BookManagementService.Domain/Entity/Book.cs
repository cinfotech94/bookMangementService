using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagementService.Domain.Entity;
    public class Book
    {
        [Key]
        public Guid id { get; set; }
        public string title { get; set; }
        public string ISBN { get; set; }
        public string author { get; set; }
        public string publicationYear { get; set;}
        public DateTime timeAdded { get; set; }=DateTime.Now;
        public string genre { get; set; }
        public int quantity { get; set; } = 1;
        public double price { get; set; }
        public int pages { get; set; }
        public string description { get; set; }
        public string category { get; set; }
        public int noClick { get; set; }
        public int noOfPPurchase { get; set; }
        public int noOfCart { get; set; }
    }
