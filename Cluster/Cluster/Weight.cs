using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cluster
{
    class Weight
    {
        public string Origin { get; set; }
        public string Destination { get; set; }
        public bool Road { get; set; }
        public double Price { get; set; }

        public Weight(string origin, string destination, bool road, double price)
        {
            Origin = origin;
            Destination = destination;
            Road = road;
            Price = price;
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3} ",  Origin, Destination, Road, Price);
        }
    }
}