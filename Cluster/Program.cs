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
        const double Sunkvežimio_emisijos_lygis = 3;
        const double Traukinio_emisijos_lygis = 5;
        const string DistancesPath = @"Distances NUTS2-NUTS2.csv";
        const string FlowsPath = @"All flows.csv";
        public bool IsWarehouse = false;
        

        
        static void Main(string[] args)
        {
            string[] Warehouses = new string[]{"ES51",
                                           "ITC4",
                                           "FR71",
                                           "PL22",
                                           "PL41",
                                           "PL51"};

            
            List<Distance> distances = ReadExcelDistances();
            List<Flow> flows = ReadExcelFlows();
 

            double pasKaina = pastatuKaina(distances, Warehouses);
            double kainaBeSandėlių = CountPriceWithoutWarehouses(distances, flows);
            double transportavimoKaina = CountPrice(distances, flows, Warehouses);
            Console.WriteLine("Pirma uzduotis: " + kainaBeSandėlių);
            Console.WriteLine("Transporavimo kaina: " + transportavimoKaina);
            Console.WriteLine("Pastatu kostrukcijos kaina: " + pasKaina);
            Console.WriteLine("(2 uzd) Pilna kaina su kostrukcijos kaina: " + (pasKaina + transportavimoKaina));
            Console.WriteLine("Kiek pinigu issaugota: " + (kainaBeSandėlių - ((pasKaina + transportavimoKaina))));
 

            //Console.WriteLine(CountPriceWithoutWarehouses(distances, flows));

            //for (int i = 0; i < flows.Count; i++)
            //{
            //    Console.Write(i);
            //    Console.WriteLine(flows.ElementAt(i).ToString());
            //}
            //   Cheapest(distances, flows, Warehouses);
            //    WarehouseOptimization(distances, flows);
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
                    values = line.Split(';');
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
                        flows.Add(flow);
                    }
                }
            }
            return flows;
        }

        public static double CountPriceWithoutWarehouses(List<Distance> distances, List<Flow> flows)
        {
            double price = flows.Sum(x=> x.FlowTons) * distances.Sum(x => x.Dis) * Sunkvežimio_pristatymo_kaštai;
            double pollution = flows.Sum(x => x.FlowTons) * distances.Sum(x => x.Dis) * Sunkvežimio_emisijos_lygis;
            double total = price + pollution;

            return total;
        }



        //private static double CountPriceWithWarehouses(String pointA, Distance h, double tons, string[] warehouses)
        //{
        //    string pointB = h.getDestination();
        //    double distance = h.getDistance();
        //    double expenses = 0;
        //    bool o = false; //origin
        //    bool d = false; //destination
        //    for (int i = 0; i < warehouses.Length; i++)
        //    {
        //        if (!o && pointA.Equals(warehouses[i]))
        //        {
        //            o = true;
        //        }
        //        if (!d && pointB.Equals(warehouses[i]))
        //        {
        //            d = true;
        //        }
        //        if (o && d)
        //        {
        //            break;
        //        }
        //    }
        //    if (o && d)
        //    {
        //        expenses = (Kintantys_sandėlio_valdymo_kaštai * tons
        //            + tons * distance * Geležinkelio_pristatymo_kaštai
        //            + distance * tons * Traukinio_emisijos_lygis
        //            + Kintantys_sandėlio_statybos_kaštai * tons);
        //        return expenses;
        //    }
        //    else
        //    {
        //        expenses = (Sunkvežimio_pristatymo_kaštai * tons * distance
        //            + distance * Sunkvežimio_emisijos_lygis * tons);
        //        return expenses;
        //    }

        //}

        //private static double Cheapest(List<Distance> distances, List<Flow> flows, string[] warehouses)
        //{
        //    double totalPrice = 0;
        //    double sum = 0;
        //    for (int i = 0; i < flows.Count; i++)
        //    {
        //        if (flows[i].getType() == "Rail" || flows[i].getType() == "Road")
        //        {
        //            string start = flows[i].getLoad();
        //            string end = flows[i].getUnload();
        //            double tons = flows[i].getTons();
        //            double curentPrice = 0;
        //            double Price = CountPriceWithoutWarehouses(distances, flows[i]);
        //            totalPrice += Price;
        //            double Cheap = Double.MaxValue;
        //            for (int j = 0; j < distances.Count; j++)
        //            {
        //                if (distances[j].getOrigin().Equals(start))
        //                {
        //                    curentPrice += CountPriceWithWarehouses(start, distances[j], tons, warehouses);
        //                    if (Price < curentPrice)
        //                    {
        //                        curentPrice = 0;
        //                    }
        //                    else
        //                    {
        //                        start = distances[j].getDestination();
        //                        j = 0;
        //                        if (start.Equals(end))
        //                        {
        //                            if (Cheap >= curentPrice && curentPrice != 0)
        //                            {
        //                                Cheap = curentPrice;
        //                            }
        //                        }
        //                    }
        //                }

        //            }
        //            if (Cheap != Double.MaxValue)
        //            {
        //                sum += Cheap;
        //            }
        //            else
        //            {
        //                sum += Price;
        //            }
        //        }
        //    }
  
        //}

        //public static double FindCheapestPath(Distance start, Distance end, List<Distance> distances, List<Flow> flows, string[] warehouses)
        //{
        //    List<String> cities = getCitiesLIst(distances);

        //    string[] prec = new string[cities.Count];
        //    double[] d = new double[cities.Count];
        //    d = d.Select(x => 99999.9).ToArray();
        //    bool[] t = new bool[cities.Count];

        //    int oIndex = cities.FindIndex(city => city.Equals(start.Origin));
        //    int eIndex = cities.FindIndex(city => city.Equals(end.Origin));
        //    prec[oIndex] = start.Origin;
        //    d[oIndex] = 0;
        //    int destinationPrecChanged = 0;
        //    int shortestIndex = -1;
        //    while (destinationPrecChanged <= 2)
        //    {
        //        int nextShortestIndex = 9999;
        //        for (int j = 0; j < cities.Count; j++)
        //        {
        //            if(d[j] > shortestIndex && d[j] < nextShortestIndex)
        //            {
        //                nextShortestIndex = j;
        //            }
        //        }
        //        shortestIndex = nextShortestIndex;
        //        string city = cities[shortestIndex];

        //        for (int i = 0; i < cities.Count; i++)
        //        {
        //            Distance dis = null;
        //            if (!t[i])
        //            {
        //                for(int j = 0; j < cities.Count; j++)
        //                {
        //                    if (city.CompareTo(cities[i]) == 0 && distances[i].Destination.CompareTo(cities[j]) == 0)
        //                        dis = distances[i];
        //                }
                        
        //                double price = CountPrice(dis, flows, warehouses);
        //                if(price < d[i] && dis != null)
        //                {
        //                    d[i] = price;
        //                    prec[i] = city;
        //                }
        //            }
                    
        //        }
        //    }
        //    for (int k = 0; k < prec.Length; k++)
        //    {
        //        Console.Write(prec[k] + " ");
        //    }
        //    Console.WriteLine();
        //    return 1;

        //}

        static double CountPrice(List<Distance> distances, List<Flow> flows, string[] warehouses)
        {

            double expenses = 0;
            for(int i = 0; i < distances.Count; i++)
            {
                double tons = flows.Where(x => x.Load.Equals(distances[i].Origin)).Sum(y => y.FlowTons);

                if (warehouses.Contains(distances[i].Origin) && warehouses.Contains(distances[i].Destination))
                {
                    expenses += (Kintantys_sandėlio_valdymo_kaštai * tons
                        + tons * distances[i].Dis * Geležinkelio_pristatymo_kaštai
                        + distances[i].Dis * tons * Traukinio_emisijos_lygis
                        + Kintantys_sandėlio_statybos_kaštai * tons);
                }
                else
                {
                    expenses += (Sunkvežimio_pristatymo_kaštai * tons * distances[i].Dis
                        + distances[i].Dis * Sunkvežimio_emisijos_lygis * tons);

                }
            }
            
            return expenses;
        }


        //int FindCostIndex(string start, List<Distance> distances)
        //{
        //    string[] cities = distances.GroupBy(x => x.Origin).Select(y => y.First().Origin).ToArray();
        //    for (int i = 0; i < cities.Length ; i++)
        //    {
        //        if (cities[i] == start)
        //            return i;
        //    }
        //    return 99999;
        //}

        private static double pastatuKaina(List<Distance> distances, string[] warehouses)
        {
            double price = warehouses.Length * Pastovūs_sandėlio_statybos_kaštai
                + warehouses.Length * Pastovūs_sandėlio_valdymo_kaštai;
            return price;
        }

      /*  private static void WarehouseOptimization(List<Distance> distances, List<Flow> flows)
        {
            List<String> cities = distances.GroupBy(x => x.Destination)
                                        .Select(y => y.First().Destination).ToList();
            List<String> Best = new List<string>();
            Random rnd = new Random();
            double Price = Double.MaxValue;
            string[] newWarehouses = new string[5];
            for (int i = 0; i < 200; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    int randomNumber = rnd.Next(0, cities.Count);
                    newWarehouses[j] = cities[randomNumber];
                }
                double temp = Cheapest(distances, flows, newWarehouses);
                if (temp < Price)
                {
                    Price = temp;
                    Best = new List<string>();
                    for (int j = 0; j < 5; j++)
                    {
                        Best.Add(newWarehouses[j]);
                    }
                    Array.Clear(newWarehouses, 0, newWarehouses.Length);
                    
                }
                Console.WriteLine(Price);
                for (int k = 0; k < Best.Count; k++)
                {
                    Console.WriteLine(Best[k]);
                }
                Best.Clear();
            }
        }*/

    }
}
