using System;
using System.Collections.Generic;

namespace Diplom.Models
{
    public class Food
    {
        public int Id { get; set; }
        public string NameFood { get; set; }
        public ICollection<Arrival> Arrivals { get; set; }
        public ICollection<PreviousBalance> PreviousBalances { get; set; }
        public ICollection<ProductConsumption> ProductConsumptions { get; set; }
    }
}   