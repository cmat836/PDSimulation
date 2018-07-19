using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LumenWorks.Framework.IO.Csv;
using System.IO;

namespace PDSimulation.src
{
    class DataReader
    {
        public CsvReader data { get; private set; }
        public DataReader(String fileName)
        {
            data = new CsvReader(new StreamReader(fileName), true);
        }

        // Gets the probability (0->1) from a 1-5 scale
        public static double getProbabilityFromScale(String data, int scaleMin, int scaleMax)
        {
            double fieldData = Convert.ToDouble(data);
            return (fieldData - scaleMin) / (scaleMax - 1);
        }

    }
}
