using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Queries
{
    public class ProductInventory :IResult
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int CurrentStock { get; set; }
    }
}
