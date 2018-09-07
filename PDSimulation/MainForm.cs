using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PDSimulation.src;

namespace PDSimulation
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void goButton_Click(object sender, EventArgs e)
        {
            

            int simnumber = 10000;
            double totaldays = 0;

            for (int i = 0; i < simnumber; i++)
            {
                Simulator simulator = new Simulator();
                simulator.populate();
                simulator.simulate();
                totaldays += simulator.daysTaken;
            }

            totaldays /= simnumber;

           daysTextBox.Text = totaldays.ToString();
        }
    }
}
