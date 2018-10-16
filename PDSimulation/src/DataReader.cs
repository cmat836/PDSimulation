using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LumenWorks.Framework.IO.Csv;
using System.IO;
using System.Windows.Forms;

namespace PDSimulation.src
{
    class DataReader
    {
        public bool fileloadedsuccessfully = true;

        public CsvReader data { get; private set; }
        public DataReader(String fileName)
        {
            try
            {
                data = new CsvReader(new StreamReader(fileName), true);
            }
            catch (Exception e)
            {
                fileloadedsuccessfully = false;
                MessageBox.Show("Error: Please enter a valid file name");
                                        
            }
        }

        // Gets the probability (0->1) from a 1-5 scale
        public static double getProbabilityFromScale(String data, int scaleMin, int scaleMax)
        {
            double fieldData = Convert.ToDouble(data);
            return (fieldData - scaleMin) / (scaleMax - 1);
        }

    }
}
