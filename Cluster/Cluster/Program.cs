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
        const string FlowsPath = @"All flows.csv";
        

        string[] Warehouses = new string[]{"ES51",
                                           "ITC4",
                                           "FR71",
                                           "PL22",
                                           "PL41",
                                           "PL51"};
        static void Main(string[] args)
        {
            int flowsSize = 0;
            List<Distance> distances = ReadExcelDistances();
            List<Flow> flows = ReadExcelFlows(ref flowsSize);

             List<Weight> weights = CountWeight(distances, flows);
                 Console.WriteLine(weights.Capacity);
                 for (int i = 0; i < flows.Count; i++)
                 {
                     Console.Write(i);
                     Console.WriteLine(flows.ElementAt(i).ToString());
                 }
            Console.ReadKey();
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
        private static List<Flow> ReadExcelFlows(ref int i)
        {
            List<Flow> flows = new List<Flow>();
            using (var reader = new StreamReader(FlowsPath))
            {
                var line = reader.ReadLine();
                var values = line.Split(';');

                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine();
                    Flow flow = new Flow(line);
                    if(flow.Load.CompareTo(flow.Unload) != 0 && flow.Type.CompareTo("Waterway") != 0)
                    {
                        flows.Add(flow);
                        i++;
                    }
                }
            }
            return flows;
        }
        public static double CountPriceWithoutWarehouses(List<Distance> distances, List<Flow> flows)
        {
            double price = flows.Sum(x => x.FlowTons) * distances.Sum(x=> x.Dis) * Sunkvežimio_pristatymo_kaštai;
            double pollution = flows.Sum(x => x.FlowTons) * distances.Sum(x => x.Dis) * Sunkvežimio_emisijos_lygis;
            Console.WriteLine(flows.Sum(x => x.FlowTons));
            double total = price + pollution;

            return total;
        }

        public static double CountPriceWithWarehouses(List<Distance> distances, List<Flow> flows)
        {
            return 1;
        }
        public double FindPath(Distance start, Distance end, List<Distance> distances)
        {
            string[] cities = distances.GroupBy(x => x.Origin)
                                        .Select(y => y.First().Origin).ToArray();
            string[] prec = new string[cities.Length];
            double[] d = new double[cities.Length];
            double shortestPathValue = 0;

            return 1;

        }
        public static List<Weight> CountWeight(List<Distance> distances, List<Flow> flows)
        {
            List<Weight> weights = new List<Weight>();
            for(int i = 0; i < flows.Count; i++)
            {
                Flow flow = flows.ElementAt(i);
                Distance dis = distances.Find(x => x.Origin.Equals(flow.Load));
                if(dis != null)
                {
                    if (flow.Type.CompareTo("Road") == 0)
                    {
                        double price = flow.FlowTons * dis.Dis * Sunkvežimio_pristatymo_kaštai;
                        double pollution = flow.FlowTons * dis.Dis * Sunkvežimio_emisijos_lygis;
                        double total = price + pollution;
                        Weight w = new Weight(dis.Origin, dis.Destination, true, total);
                        weights.Add(w);
                    }
                    else if (flow.Type.CompareTo("Rail") == 0)
                    {
                        double price = flow.FlowTons * dis.Dis * Geležinkelio_pristatymo_kaštai;
                        double pollution = flow.FlowTons * dis.Dis * Traukinio_emisijos_lygis;
                        double total = price + pollution;
                        Weight w = new Weight(dis.Origin, dis.Destination, false, total);
                        weights.Add(w);
                    }
                }
                
                
            }
            return weights;
        }
    }
}
