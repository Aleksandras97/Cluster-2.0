using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cluster
{
    class Program
    {
        const double Pastovūs_sandėlio_statybos_kaštai = 308525.05;
        const double Kintantys_sandėlio_statybos_kaštai = 539.91;
        const double Postovūs_sandėlio_valdymo_kaštai = 8513.26;
        const double Kintantys_sandėlio_valdymo_kaštai = 6.31;
        const double Sunkvežimio_pristatymo_kaštai = 125.41;
        const double Geležinkelio_pristatymo_kaštai = 3.95;
        const double Sunkvežimio_emisijos_lygis = 0.062;
        const double Traukinio_emisijos_lygis = 0.022;
        const string DistancesPath = @"";
        const string Flows = @"";
        const List<string> Warehouses;

        static void Main(string[] args)
        {
        }

        private static void ReadExcelDistances(List<City> cities)
        {
            List<string> cityNames = new List<string>();
            using (var reader = new StreamReader(DistancesPath))
            {
                var line = reader.ReadLine();
                var values = line.Split(';');

                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine();
                    values = line.Split(';');
                    City city = new City(values[0]...);
                    cities.Add(city);
                    cityNames.Add(values[0]);
                }
                cityNames = cityNames.Distinct();
                ()
                {
                    if (x.name)
                }
            }
        }
        private static void ReadExcelFlows()
        {
            using (var reader = new StreamReader(Flows))
            {
                List<string> listA = new List<string>();
                List<string> listB = new List<string>();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';');

                    listA.Add(values[0]);
                    listB.Add(values[1]);
                }
            }
        }

    }
}
