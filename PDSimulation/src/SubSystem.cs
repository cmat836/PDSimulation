using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDSimulation.src
{
    class SubSystem
    {
        // Stores the dependency this subsystem has on other subsystems
        public Dictionary<SubSystem, double> subSystemDependencies = new Dictionary<SubSystem, double>();
        public string name { get; set; }
        public double hoursTillCompletion { get; set; }
        public bool isBlocked { get; set; }
        public bool canStart { get; set; }

        public SubSystem(String name, double completionTime, bool isBlocked, bool canStart)
        {
            this.name = name;
            this.hoursTillCompletion = completionTime;
            this.isBlocked = isBlocked;
            this.canStart = canStart;
        }

        // Gets the dependency THIS subsystem has on the passed subsystem
        public double getDependencyOnSubSystem(SubSystem sub)
        {
            return subSystemDependencies[sub];
        }
    }
}
