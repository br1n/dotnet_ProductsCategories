using System;
using System.ComponentModel.DataAnnotations;

namespace Products_Categories.Models
{
    public class Association
    {
        [Key]
        public int AssociationId {get; set;}

        //Foreign Key to Product.UserId
        public int ProductId {get; set;}

        //Foreign Key to Category.UserId
        public int CategoryId {get; set;}
        public Category Category {get;set;}
        public Product Product {get;set;}

    }
}