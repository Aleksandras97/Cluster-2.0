using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cluster
{
    class City
    {
        public string Key {get; set;}
        public string Pradzia { get; set; }
        public string Pabaiga { get; set; }
        public double Kelias { get; set; }
        public double Laikas { get; set; }

        public City(string pradzia, string pabaiga, double kelias, double laikas)
        {
            Pradzia = pradzia;
            Pabaiga = pabaiga;
            Kelias = kelias;
            Laikas = laikas;
            Key = string.Concat(pradzia, '-', pabaiga);
        }

        public City(string key, string pradzia, string pabaiga, double kelias, double laikas)
        {
            Key = key;
            Pradzia = pradzia;
            Pabaiga = pabaiga;
            Kelias = kelias;
            Laikas = laikas;
        }

        public City(string line)
        {
            string[] vals = line.Split(',');
            Key = vals[0];
            Pradzia = vals[1];
            Pabaiga = vals[2];
            Kelias = Double.Parse(vals[3]);
            Laikas = Double.Parse(vals[4]);
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3} {4}", Key, Pradzia, Pabaiga, Kelias, Laikas);
        }
    }
}
