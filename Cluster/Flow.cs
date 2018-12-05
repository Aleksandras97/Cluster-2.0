using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cluster
{
    class Flow
    {
        public string Load { get; set; }
        public string Unload { get; set; }
        public string Type { get; set; }
        public double FlowTons { get; set; }
        public double FlowTonKMs { get; set; }

        public Flow(string load, string unload, string type, double flowTons, double flowTonKMs)
        {
            Load = Load;
            Unload = unload;
            Type = type;
            FlowTons = flowTons;
            FlowTonKMs = flowTonKMs;
        }

        public Flow(string line)
        {
            string[] values = line.Split(',');
            Load = values[0];
            Unload = values[1];
            Type = values[2];
            FlowTons = Double.Parse(values[3]);
            FlowTonKMs = Double.Parse(values[4]);
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3} {4}", Load, Unload, Type, FlowTons, FlowTonKMs);
        }

        public string getType()
        {
            return Type;
        }
    }
}