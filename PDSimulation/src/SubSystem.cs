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

        // Messages sent to this subsystem that are pending reply
        public List<Message> inbox = new List<Message>();
        public string name { get; set; }
        // Time left untill the task is completed
        public double hoursTillCompletion { get; set; }
        // Total time needed to complete the tast
        public double totalTimeNeeded { get; set; }
        public bool isBlocked { get; set; }

        public SubSystem(String name, bool isBlocked)
        {
            this.name = name;
            this.isBlocked = isBlocked;
        }

        public void setTime(double timeToComplete)
        {
            hoursTillCompletion = timeToComplete;
            totalTimeNeeded = timeToComplete;
        }

        // Gets the dependency THIS subsystem has on the passed subsystem
        public double getDependencyOnSubSystem(SubSystem sub)
        {
            return subSystemDependencies[sub];
        }

        // Check if the subsystem has no pending messages, and update whether or not it can be worked on
        public void checkStatus()
        {
            this.isBlocked = false;
            foreach (Message m in inbox)
            {
                if (!m.answerAssumed || !m.answered)
                {
                    this.isBlocked = true;
                    return;
                }
            }
        }

        public double getRandomLength()
        {
            return totalTimeNeeded;
        }
    }
}
