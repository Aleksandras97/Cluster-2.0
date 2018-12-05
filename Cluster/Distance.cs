using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cluster
{
    class Distance
    {
        public string Key { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public double Dis { get; set; }
        public double Time { get; set; }

        public Distance(string origin, string destination, double distance, double time)
        {
            Origin = origin;
            Destination = destination;
            Dis = distance;
            Time = time;
            Key = string.Concat(origin, '-', destination);
        }

        public Distance(string key, string pradzia, string pabaiga, double kelias, double laikas)
        {
            Key = key;
            Origin = pradzia;
            Destination = pabaiga;
            Dis = kelias;
            Time = laikas;
        }

        public Distance(string line)
        {
            string[] vals = line.Split(',');
            Key = vals[0];
            Origin = vals[1];
            Destination = vals[2];
            Dis = Double.Parse(vals[3]);
            Time = Double.Parse(vals[4]);
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3} {4}", Key, Origin, Destination, Dis, Time);
        }
    }
}