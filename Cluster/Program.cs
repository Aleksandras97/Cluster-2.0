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
            string[] newWarehouses = new string[5];
            double Price = 0;

            double distance = distances.Sum(x => x.getDistance());
            double pasKaina = pastatuKaina(distances, Warehouses);
            double kainaBeSandėlių = CountPriceWithoutWarehouses(distances, flows, distance);
            double transportavimoKaina = CountPriceWithWarehouses(distances, flows, Warehouses);

            WarehouseOptimization(distances, flows, out newWarehouses, out Price);

            Console.WriteLine("Pirma uzduotis: " + kainaBeSandėlių);
            Console.WriteLine("Transporavimo kaina: " + transportavimoKaina);
            Console.WriteLine("Pastatu kostrukcijos kaina: " + pasKaina);
            Console.WriteLine("(2 uzd) Pilna kaina su kostrukcijos kaina: " + (pasKaina + transportavimoKaina));
            Console.WriteLine("Kiek pinigu issaugota: " + (kainaBeSandėlių - ((pasKaina + transportavimoKaina))));


            Console.WriteLine("Optimalesni sandėliai:");
            for(int i = 0; i < newWarehouses.Length; i++)
            {
                Console.WriteLine(newWarehouses[i]);
            }
            Console.WriteLine("Kaina: " + Price);
            Console.WriteLine("Kiek pinigu issaugota: " + (kainaBeSandėlių - Price));

            Console.ReadKey();
        }
        /// <summary>
        /// Nuskaitomi Distance failo elementai
        /// </summary>
        /// <returns></returns>
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
                    if (distance.getOrigin().CompareTo(distance.getDestination()) != 0)
                    {
                        distances.Add(distance);
                    }
                }
            }
            return distances;
        }

        /// <summary>
        /// Nuskaitomi Flow failo elementai
        /// </summary>
        /// <returns></returns>
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
                    if (flow.getLoad().CompareTo(flow.getUnload()) != 0 && flow.getType().CompareTo("Waterway") != 0)
                    {
                        flows.Add(flow);
                    }
                }
            }
            return flows;
        }

        /// <summary>
        /// Skaiciuojama kaina, kai sandeliu tarp miestu nera
        /// </summary>
        /// <param name="distances">distance listas</param>
        /// <param name="flows">flows listas</param>
        /// <param name="distance">visu atstumu suma</param>
        /// <returns>suma</returns>
        public static double CountPriceWithoutWarehouses(List<Distance> distances, List<Flow> flows, double distance)
        {
            double price = flows.Sum(x => x.getTons()) * Sunkvežimio_pristatymo_kaštai;
            double pollution = flows.Sum(x => x.getTons()) * Sunkvežimio_emisijos_lygis;
            double total = distance * (price + pollution);
            return total;
        }

        /// <summary>
        /// Skaiciuojama kaina, kai sandelius pastateme nurodytose miestose
        /// </summary>
        /// <param name="distances">distances listas</param>
        /// <param name="flows">flows listas</param>
        /// <param name="warehouses">nurodyti miestai kur bus statomas sandelys</param>
        /// <returns></returns>
        static double CountPriceWithWarehouses(List<Distance> distances, List<Flow> flows, string[] warehouses)
        {

            double expenses = 0;
            for(int i = 0; i < distances.Count; i++)
            {
                double tons = flows.Where(x => x.getLoad().Equals(distances[i].getOrigin())).Sum(y => y.getTons());

                if (warehouses.Contains(distances[i].getOrigin()) && warehouses.Contains(distances[i].getDestination()))
                {
                    //Sandelio kastai
                    expenses += (Kintantys_sandėlio_valdymo_kaštai * tons
                        + tons * distances[i].getDistance() * Geležinkelio_pristatymo_kaštai
                        + distances[i].getDistance() * tons * Traukinio_emisijos_lygis
                        + Kintantys_sandėlio_statybos_kaštai * tons);
                }
                else
                {
                    //Sunkvezimio susidarantys kastai
                    expenses += (Sunkvežimio_pristatymo_kaštai * tons * distances[i].getDistance()
                        + distances[i].getDistance() * Sunkvežimio_emisijos_lygis * tons);

                }
            }
            
            return expenses;
        }
        /// <summary>
        /// Suskaiciuojami pastatu kastai nurodytuose regionose
        /// </summary>
        /// <param name="distances"></param>
        /// <param name="warehouses"></param>
        /// <returns></returns>
        private static double pastatuKaina(List<Distance> distances, string[] warehouses)
        {
            double price = warehouses.Length * Pastovūs_sandėlio_statybos_kaštai
                + warehouses.Length * Pastovūs_sandėlio_valdymo_kaštai;
            return price;
        }

        /// <summary>
        /// Randamas potimalesnis budas pastatyti sandelius
        /// </summary>
        /// <param name="distances">distances listas</param>
        /// <param name="flows">flows listas</param>
        /// <param name="newWarehouses">nauji sandeliai</param>
        /// <param name="Price">sandeliu kastu</param>
        private static void WarehouseOptimization(List<Distance> distances, List<Flow> flows, out string[] newWarehouses, out double Price)
        {
            //Visu skirtinu miestu listas
            List<String> cities = distances.GroupBy(x => x.getDestination())
                                        .Select(y => y.First().getDestination()).ToList();
            //Geriausiu miestu masyvas
            List<String> Best = new List<string>();
            Random rnd = new Random();
            Price = Double.MaxValue;
            newWarehouses = new string[5];
            //Optimalesniu miestu ieskojimas
            for (int i = 0; i < 200; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    int randomNumber = rnd.Next(0, cities.Count);
                    newWarehouses[j] = cities[randomNumber];
                }
                //Skaiciuojami kastai su naujai atrinkaitais miestais
                double temp = CountPriceWithWarehouses(distances, flows, newWarehouses);
                //Tikrinama ar kaina yra mazesne 
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
        }

    }
}

