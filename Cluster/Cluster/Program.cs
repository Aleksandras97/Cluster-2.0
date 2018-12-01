using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cluster
{
    class Program
    {
        const double Pastovūs_sandėlio_statybos_kaštai = 308525.05;
        const double Kintantys_sandėlio_statybos_kaštai = 539.91;
        const double Pastovūs_sandėlio_valdymo_kaštai = 8513.26;
        const double Kintantys_sandėlio_valdymo_kaštai = 6.31;
        const double Sunkvežimio_pristatymo_kaštai = 125.41;
        const double Geležinkelio_pristatymo_kaštai = 3.95;
        const double Sunkvežimio_emisijos_lygis = 0.062;
        const double Traukinio_emisijos_lygis = 0.022;
        const string DistancesPath = @"Distances NUTS2-NUTS2.csv";
        const string FlowsPath = @"All flows";

        static void Main(string[] args)
        {
            List<Distance> distances = ReadExcelDistances();
            List<Flow> flows = ReadExcelFlows();
            Console.WriteLine(CountPriceWithoutWarehouses(distances, flows));
        }

        private static List<Distance> ReadExcelDistances()
        {
            List<Distance> distances = new List<Distance>();
            using (var reader = new StreamReader(DistancesPath))
            {
                var line = reader.ReadLine();
                var values = line.Split(';');

                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine();
                    Distance distance = new Distance(line);
                    if (distance.Origin.CompareTo(distance.Destination) != 0)
                    {
                        distances.Add(distance);
                    }
                }
            }
            return distances;
        }
        private static List<Flow> ReadExcelFlows()
        {
            List<Flow> flows = new List<Flow>();
            using (var reader = new StreamReader(DistancesPath))
            {
                var line = reader.ReadLine();
                var values = line.Split(';');

                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine();
                    Flow flow = new Flow(line);
                    if(flow.Load.CompareTo(flow.Unload) != 0)
                    {
                        flows.Add(flow);
                    }
                }
            }
            return flows;
        }
        public static double CountPriceWithoutWarehouses(List<Distance> distances, List<Flow> flows)
        {
            double price = flows.Sum(x => x.FlowTons) * distances.Sum(x=> x.Dis) * Sunkvežimio_pristatymo_kaštai;
            double polution = flows.Sum(x => x.FlowTons) * distances.Sum(x => x.Dis) * Sunkvežimio_emisijos_lygis;

            double total = price + polution;

            return total;
        }

    }
}