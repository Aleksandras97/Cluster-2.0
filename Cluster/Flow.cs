using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cluster
{
    class Flow
    {
        private string Load { get; set; }        //Pradinis miestas
        private string Unload { get; set; }      //Galutinis miestas
        private string Type { get; set; }        //Tipas
        private double FlowTons { get; set; }    //Tonos
        private double FlowTonKMs { get; set; }  //Tonos/KMs

        /// <summary>
        /// Konstruktorius
        /// </summary>
        /// <param name="load">Pradinis miestas</param>
        /// <param name="unload">Galutinis miestas</param>
        /// <param name="type">Tipas</param>
        /// <param name="flowTons">Tonos</param>
        /// <param name="flowTonKMs">Tonos/KMs</param>
        public Flow(string load, string unload, string type, double flowTons, double flowTonKMs)
        {
            Load = Load;
            Unload = unload;
            Type = type;
            FlowTons = flowTons;
            FlowTonKMs = flowTonKMs;
        }

        /// <summary>
        /// Nuskaitymas
        /// </summary>
        /// <param name="line">Nuskaitoma eilute</param>
        public Flow(string line)
        {
            string[] values = line.Split(',');
            Load = values[0];
            Unload = values[1];
            Type = values[2];
            FlowTons = Double.Parse(values[3]);
            FlowTonKMs = Double.Parse(values[4]);
        }

        /// <summary>
        /// ToString metodas
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3} {4}", Load, Unload, Type, FlowTons, FlowTonKMs);
        }

        public String getLoad()
        {
            return Load;
        }
        public String getUnload()
        {
            return Unload;
        }

        public double getTons()
        {
            return FlowTons;
        }

        public string getType()
        {
            return Type;
        }
    }
}