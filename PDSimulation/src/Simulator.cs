using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDSimulation.src
{
    class Simulator
    {
        private List<Actor> actorslist = new List<Actor>();
        private Dictionary<String, SubSystem> subsystemlist = new Dictionary<string, SubSystem>();

        DataReader mainData;
        DataReader subSystems;

        public double daysTaken { get; set; }

        public Simulator()
        {
            mainData = new DataReader("../../data/surveyout.csv");
            subSystems = new DataReader("../../data/subsystems.csv");
        }

        public void populate()
        {
            while (subSystems.data.ReadNextRecord())
            {
                SubSystem sub = new SubSystem(subSystems.data["name"], Convert.ToDouble(subSystems.data["length"]), false, true);
                subsystemlist.Add(subSystems.data["name"], sub);
            }

            while (mainData.data.ReadNextRecord())
            {
                Actor actor = new Actor(subsystemlist[mainData.data["subsystem"]], Convert.ToDouble(mainData.data["totalmessageresponsetime"]), Convert.ToDouble(mainData.data["centralization"]), Convert.ToDouble(mainData.data["assumptions"]));
                actorslist.Add(actor);
            }

            Console.WriteLine(actorslist.Count);
        }

        public void simulate()
        {
            int days = 0;

            //set finished counter
            bool finished = false;
            while (finished == false)
            {
                //for every substystem
                foreach (KeyValuePair<String, SubSystem> currentsub in subsystemlist)
                {
                    //if the subsystem can be started
                    if (currentsub.Value.canStart == true)
                    {
                        //if subsystem not blocked
                        if (currentsub.Value.isBlocked == false)
                        {
                            //find all avaliable actors for this task
                            foreach (Actor currentactor in actorslist)
                            {
                                if (currentactor.subSystem == currentsub.Value)
                                {
                                    //check if message generated
                                    bool localmessage = currentactor.generatemessage();
                                    if (localmessage == false)
                                    {
                                        currentsub.Value.daysTillCompletion--;
                                    }
                                    //Console.WriteLine("days till completion = " + currentsub.Value.daysTillCompletion);
                                }
                            }
                        }
                    }
                }

                //check each subsystem to see if work remaining
                foreach (KeyValuePair<String, SubSystem> sub2 in subsystemlist)
                {
                    //if works still remaning break out of loop
                    bool v = sub2.Value.daysTillCompletion >= 0;
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
