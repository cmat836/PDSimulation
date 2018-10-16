using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDSimulation.src
{
    class Actor
    {
        // Messages that have been sent from this actor and are pending reply
        public List<Message> outbox = new List<Message>();

        public double maxHoursWorkedPerDay = 8;
        public double workingHoursLeftInDay;
        public double timeWorkedThisDay = 0;

        // How fast is the actor at responding to messages
        public double messageResponseRate;

        public double experience;

        public SubSystem subSystem { get; set; }

        // Total time every day allocated to responding to messages
        public double totalMessageResponseTime { get;set; }

        public double centralization { get; set; }

        // Chance of actor making an assumption
        public double assumptionChance { get; set; }

        // How likely the assumption is going to be correct
        public double assumptionAccuracy { get; set; }

        // How much of an effect an incorrect assumption has
        public double assumptionEffect { get; set; }

        public bool generatemessage()
        {
            Random rand = new Random();
            if (rand.NextDouble() < assumptionChance)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Actor(SubSystem subSystem, double totalmessagetime, double centralization, double assumptionchance, double messageResponseRate, double assumptionaccuracy, double assumptioneffect, double experience)
        {
            this.subSystem = subSystem;
            this.totalMessageResponseTime = totalmessagetime;
            this.centralization = centralization;
            this.assumptionChance = assumptionchance;
            this.messageResponseRate = messageResponseRate;
            this.assumptionAccuracy = assumptionaccuracy;
            this.assumptionEffect = assumptioneffect;
            this.experience = experience;
        }

        public void resetDay()
        {
            workingHoursLeftInDay = maxHoursWorkedPerDay;
            timeWorkedThisDay = 0;
        }
    }
}
