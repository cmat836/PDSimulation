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
using System.IO;

namespace PDSimulation
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            surveyTextbox.Text = "../../data/actualtest.csv";
            prelimSurveyTextbox.Text = "../../data/subsystems.csv";
            simNumberTextbox.Text = "1";
            dataoutPathTextbox.Text = "../../data/dataout.csv";
        }

        private void goButton_Click(object sender, EventArgs e)
        {
            

            int simnumber = Int32.Parse(simNumberTextbox.Text);
            double totaldays = 0;

            string DATA = "";

            for (int i = 0; i < simnumber; i++)
            {
                Simulator simulator = new Simulator(surveyTextbox.Text, prelimSurveyTextbox.Text);
                if (simulator.didloadsuccessfully)
                {
                    simulator.populate();
                    simulator.simulate();
                    totaldays += simulator.daysTaken;
                    DATA += "," + simulator.daysTaken.ToString() + "\n";
                }
            }

            totaldays /= simnumber;

            StreamWriter w = new StreamWriter(dataoutPathTextbox.Text);
            w.WriteLine(DATA);
            w.Close();

           daysTextBox.Text = totaldays.ToString();
        }
    }
}
