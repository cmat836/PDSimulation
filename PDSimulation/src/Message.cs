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
        public SubSystem recipient;

        public int daysSinceSent;
        public bool answered;

        // How much time is left for the average person to respond to the message
        public double messageResponseTime;

        public bool answerAssumed;

        public Message(Actor sender, SubSystem recipient, double length)
        {
            this.sender = sender;
            this.recipient = recipient;
            daysSinceSent = 0;
            answered = false;
            answerAssumed = false;
            messageResponseTime = length;
        }

        // Checks if the mesage has been answered and deletes it if it has been
        public void checkStatus()
        {
            if (this.answered)
            {
                this.sender.outbox.Remove(this);
                this.recipient.inbox.Remove(this);
            }
        }

        public void delete()
        {
            this.answered = true;
            this.sender.outbox.Remove(this);
            this.recipient.inbox.Remove(this);
        }
    }
}
