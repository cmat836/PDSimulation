using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDSimulation
{
    class Simulator
    {
        private List<Actor> actorslist = new List<Actor>();
        private List<SubSystem> subsystemlist = new List<SubSystem>();

        public double daysTaken { get; set; }

        public Simulator()
        {
           
        }

        public void simulate()
        {
            Actor actor1 = new Actor("subA", 0.4, 1, 0.5);
            actorslist.Add(actor1);
            Actor actor2 = new Actor("subA", 0.2, 1, 0.3);
            actorslist.Add(actor2);

            SubSystem task1 = new SubSystem("subA", 90, false, true);
            subsystemlist.Add(task1);

            int days = 0;

            //set finished counter
            bool finished = false;
            while (finished == false)
            {
                //for every substystem
                foreach (SubSystem currentsub in subsystemlist)
                {
                    //if the subsystem can be started
                    if (currentsub.canStart == true)
                    {
                        //if subsystem not blocked
                        if (currentsub.isBlocked == false)
                        {
                            //find all avaliable actors for this task
                            foreach (Actor currentactor in actorslist)
                            {
                                if (currentactor.subSystem == currentsub.name)
                                {
                                    //check if message generated
                                    bool localmessage = currentactor.generatemessage();
                                    if (localmessage == false)
                                    {
                                        currentsub.daysTillCompletion--;
                                    }
                                    Console.WriteLine("days till completion = " + currentsub.daysTillCompletion);
                                }
                            }
                        }
                    }
                }

                //check each subsystem to see if work remaining
                foreach (SubSystem sub2 in subsystemlist)
                {
                    //if works still remaning break out of loop
                    bool v = sub2.daysTillCompletion >= 0;
                    if (v)
                    {
                        finished = false;
                        break;
                    }

                    finished = true;
                }
                if (days > 10000)
                {
                    finished = true;
                }

                //finished day
                days++;
            }

            daysTaken = days;
        }
    }
}
