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
        public bool IsWarehouse = false;
        

        string[] Warehouses = new string[]{"ES51",
                                           "ITC4",
                                           "FR71",
                                           "PL22",
                                           "PL41",
                                           "PL51"};
        static void Main(string[] args)
        {
            int flowsSize = 0;
            Dictionary<String, Distance> distances = ReadExcelDistances();
            Dictionary<String, Flow> flows = ReadExcelFlows(ref flowsSize);

            Console.WriteLine(CountPriceWithoutWarehouses(distances, flows));

            for (int i = 0; i < flows.Count; i++)
            {
                Console.Write(i);
                Console.WriteLine(flows.ElementAt(i).ToString());
            }
            Console.ReadKey();
        }

        private static Dictionary<String, Distance> ReadExcelDistances()
        {
            Dictionary<String, Distance> distances = new Dictionary<String, Distance>();
            using (var reader = new StreamReader(DistancesPath))
            {
                var line = reader.ReadLine();
                var values = line.Split(';');

                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine();
                    values = line.Split(';');
                    Distance distance = new Distance(line);
                    if (distance.Origin.CompareTo(distance.Destination) != 0)
                    {
                        distances.Add(values[0], distance);
                    }
                }
            }
            return distances;
        }
        private static Dictionary<String, Flow> ReadExcelFlows(ref int i)
        {
            Dictionary<String, Flow> flows = new Dictionary<String, Flow>();
            using (var reader = new StreamReader(FlowsPath))
            {
                var line = reader.ReadLine();
                var values = line.Split(';');

                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine();
                    values = line.Split(';');
                    Flow flow = new Flow(line);
                    if (flow.Load.CompareTo(flow.Unload) != 0 && flow.Type.CompareTo("Waterway") != 0)
                    {
                        flows.Add(values[0] + "-" + values[1] + values[2], flow);
                        i++;
                    }
                }
            }
            return flows;
        }
        public static double CountPriceWithoutWarehouses(Dictionary<String, Distance> distances, Dictionary<String, Flow> flows)
        {
            double price = flows.Sum(x => x.Value.FlowTons) * distances.Sum(x => x.Value.Dis) * Sunkvežimio_pristatymo_kaštai;
            double pollution = flows.Sum(x => x.Value.FlowTons) * distances.Sum(x => x.Value.Dis) * Sunkvežimio_emisijos_lygis;
            Console.WriteLine(flows.Sum(x => x.Value.FlowTons));
            double total = price + pollution;

            return total;
        }

        public static double CountPriceWithWarehouses(List<Distance> distances, List<Flow> flows)
        {
            return 1;
        }
        public double FindPath(Distance start, Distance end, Dictionary<String, Distance> distances, Dictionary<String, Distance> flows, string[] warehouses)
        {
            List<String> cities = distances.GroupBy(x => x.Value.Origin)
                                        .Select(y => y.First().Value.Origin).ToList();
            string[] prec = new string[cities.Count];
            double[] d = new double[cities.Count];
            bool[] a = new bool[cities.Count];

            int index = cities.FindIndex(p => p.Equals(start.Origin));
            prec[index] = start.Origin;
            d[index] = 0;
            a[index] = true;

            int z = a.Where(y => y.Equals(true)).Count();
            for (int j = 0; j < 3; j++)
            {
                for (int i = 0; i < cities.Count; i++)
                {
                    string city = cities.ElementAt(i);
                    if (warehouses.Contains(city))
                    {
                        CountPrice(distances[pirmas miestas + "-" + city], [pirmas miestas + "-" + city] + "Rail");
                    }
                    else
                    {
                        CountPrice(distances[pirmas miestas + "-" + city], [pirmas miestas + "-" + city] + "Road");
                    }
                }
            }

            return 1;

        }



        int FindCostIndex(string start, List<Distance> distances)
        {
            string[] cities = distances.GroupBy(x => x.Origin).Select(y => y.First().Origin).ToArray();
            for (int i = 0; i < cities.Length ; i++)
            {
                if (cities[i] == start)
                    return i;
            }
            return 99999;
        }

        public static double CountPrice(Distance distance, Flow flow)
        {
            if (flow.Type.Equals("Road"))
            {
                return flow.FlowTons * distance.Dis * (Sunkvežimio_pristatymo_kaštai + Sunkvežimio_emisijos_lygis);
            }
            else if (flow.Type.Equals("Rail"))
            {
                return flow.FlowTons * distance.Dis * (Geležinkelio_pristatymo_kaštai + Traukinio_emisijos_lygis);
            }
            return -1;
        }

    }
}
