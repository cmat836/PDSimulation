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

    }
}
