﻿using System;
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
        // Messages sent to this actor that are pending reply
        public List<Message> inbox = new List<Message>();

        public double maxHoursWorkedPerDay = 8;
        public double workingHoursLeftInDay;

        // Time taken to respond to a message
        public double messageResponseTime;

        public SubSystem subSystem { get; set; }

        // Total time every day allocated to responding to messages
        public double totalMessageResponseTime
        {
            get;set;
        }

        public double centralization
        {
            get; private set;
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

        public Actor(SubSystem subSystem, double totalmessagetime, double centralization, double assumption, double messageResponseTime)
        {
            this.subSystem = subSystem;
            this.totalMessageResponseTime = totalmessagetime;
            this.centralization = centralization;
            this.assumption = assumption;
            this.messageResponseTime = messageResponseTime;
        }
    }
}
