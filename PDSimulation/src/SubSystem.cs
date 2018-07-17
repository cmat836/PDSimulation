using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDSimulation.src
{
    class SubSystem
    {
        public string name { get; set; }
        public double daysTillCompletion { get; set; }
        public bool isBlocked { get; set; }
        public bool canStart { get; set; }

        public SubSystem(String name, double completionTime, bool isBlocked, bool canStart)
        {
            this.name = name;
            this.daysTillCompletion = completionTime;
            this.isBlocked = isBlocked;
            this.canStart = canStart;
        }

 
    }
}
