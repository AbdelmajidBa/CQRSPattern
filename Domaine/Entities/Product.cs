using System;
using System.Collections.Generic;
using System.Text;

namespace Domaine.Entities
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal UnitPrice { get; set; }
        public bool IsOutOfStock 
        {
            get => CurrentStock <= 0;
            
        }
        public int CurrentStock { get; set; }
    }
}
