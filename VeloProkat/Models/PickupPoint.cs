using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeloProkat.Models
{
    internal class PickupPoint
    {
        public int Id { get; set; }

        public string NumberPoint { get; set; } = null!;

        public string City { get; set; } = null!;

        public string Street { get; set; } = null!;

        public string NumberStreet { get; set; } = null!;
    }
}
