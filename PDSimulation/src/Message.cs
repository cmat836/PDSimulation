using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDSimulation.src
{
    class Message
    {
        public Actor sender;
        public Actor recipient;

        public int daysSinceSent;
        public bool answered;

        public Message(Actor sender, Actor recipient)
        {
            this.sender = sender;
            this.recipient = recipient;
            daysSinceSent = 0;
            answered = false;
        }


    }
}
