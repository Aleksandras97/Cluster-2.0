using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cluster
{
    class Distance
    {
        private string Key { get; set; }         //Raktas
        private string Origin { get; set; }      //Pradinis miestas
        private string Destination { get; set; } //Galutinis miestas
        private double Dis { get; set; }         //Atstumas tarp miestu
        private double Time { get; set; }        //Laikas

        /// <summary>
        /// Kostruktorius
        /// </summary>
        /// <param name="origin">Pradinis miestas</param>
        /// <param name="destination">Galutinis miestas</param>
        /// <param name="distance">Atstumas tarp miestu</param>
        /// <param name="time">Laikas</param>
        public Distance(string origin, string destination, double distance, double time)
        {
            Origin = origin;
            Destination = destination;
            Dis = distance;
            Time = time;
            Key = string.Concat(origin, '-', destination);
        }

        /// <summary>
        /// Konstruktorius
        /// </summary>
        /// <param name="key">Raktas</param>
        /// <param name="origin">Pradinis miestas</param>
        /// <param name="destination">Galutinis miestas</param>
        /// <param name="distance">Atstumas tarp miestu</param>
        /// <param name="time">Laikas</param>
        public Distance(string key, string pradzia, string pabaiga, double kelias, double laikas)
        {
            Key = key;
            Origin = pradzia;
            Destination = pabaiga;
            Dis = kelias;
            Time = laikas;
        }

        /// <summary>
        /// Nuskaitymas
        /// </summary>
        /// <param name="line">Nuskaityta eilute</param>
        public Distance(string line)
        {
            string[] vals = line.Split(',');
            Key = vals[0];
            Origin = vals[1];
            Destination = vals[2];
            Dis = Double.Parse(vals[3]);
            Time = Double.Parse(vals[4]);
        }

        /// <summary>
        /// ToString metodas
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3} {4}", Key, Origin, Destination, Dis, Time);
        }

        public String getOrigin()
        {
            return Origin;
        }

        public String getDestination()
        {
            return Destination;
        }

        public double getDistance()
        {
            return Dis;
        }

        public double getTime()
        {
            return Time;
        }
    }
}