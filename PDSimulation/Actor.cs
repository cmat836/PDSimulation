using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDSimulation
{
    class Actor
    {
        public String subSystem { get; set; }
        public double maxMessages
        {
            get;set;
        }

        public double centralization
        {
            get; set;
        }

        public double assumption
        {
            get; set;
        }

        public bool generatemessage()
        {
            Random rand = new Random();
            if (rand.NextDouble() < assumption)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Actor(String subSystem, double maxmessage, double centralization, double assumption)
        {
            this.subSystem = subSystem;
            maxMessages = maxmessage;
            this.centralization = centralization;
            this.assumption = assumption;
        }
    }
}
