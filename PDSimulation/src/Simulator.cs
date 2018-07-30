using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Wison wuz here
// Chris wuz here

namespace PDSimulation.src
{
    class Simulator
    {
        // List of actors
        private List<Actor> actorsList = new List<Actor>();
        // List of subsystems
        private Dictionary<String, SubSystem> subSystemList = new Dictionary<string, SubSystem>();

        // Data from the survey
        DataReader mainData;
        // Data from the subsystems
        DataReader subSystems;

        public double daysTaken { get; set; }

        public double dependencyThreshold = 0.4;

        public Simulator()
        {
            mainData = new DataReader("../../data/surveyout.csv");
            subSystems = new DataReader("../../data/subsystems.csv");
            daysTaken = 0;
        }

        // Build the data 
        public void populate()
        {
            while (subSystems.data.ReadNextRecord())
            {
                SubSystem sub = new SubSystem(subSystems.data["name"], Convert.ToDouble(subSystems.data["length"]), false, true);
                subSystemList.Add(subSystems.data["name"], sub);
            }

            while (mainData.data.ReadNextRecord())
            {
                SubSystem sub = subSystemList[mainData.data["subsystem"]];
                double totalmessagetime = Convert.ToDouble(mainData.data["totalmessageresponsetime"];
                double centralization = DataReader.getProbabilityFromScale(mainData.data["centralization"], 1, 5);
                double assumptions = DataReader.getProbabilityFromScale(mainData.data["assumptions"], 1, 5);
                double messageresponsetime = Convert.ToDouble(mainData.data["messageresponsetime"]);

                Actor actor = new Actor(sub, totalmessagetime, centralization, assumptions, messageresponsetime);
                actorsList.Add(actor);

                // Go through all the subsystems
                foreach (KeyValuePair<String, SubSystem> kvp in subSystemList)
                {
                    SubSystem subOther = kvp.Value;
                    // Get the subSystem the actor is assigned to
                    SubSystem subActor = subSystemList[mainData.data["subsystem"]];
                    // Get the dependency the actors subSystem has on the other subSystem
                    double dependency = Convert.ToDouble(mainData.data["subsystemdependency [" + subOther.name + "]"]);
                    // Store it
                    //if (subOther != subActor)
                    //{
                        subSystemList[mainData.data["subsystem"]].subSystemDependencies.Add(kvp.Value, dependency);
                    //}
                }
            }

            //Console.WriteLine("Sub x depend");
            //foreach (KeyValuePair<SubSystem, double> kvp in subSystemList["sub x"].subSystemDependencies)
            //{
            //    Console.WriteLine(kvp.Value);
            //}    
            Console.WriteLine(subSystemList["sub x"].subSystemDependencies.Count);
        }

        public void simulateOld()
        {
            int days = 0;

            //set finished counter
            bool finished = false;
            while (finished == false)
            {
                //for every substystem
                foreach (KeyValuePair<String, SubSystem> currentsub in subSystemList)
                {
                    //if the subsystem can be started
                    if (currentsub.Value.canStart == true)
                    {
                        //if subsystem not blocked
                        if (currentsub.Value.isBlocked == false)
                        {
                            //find all avaliable actors for this task
                            foreach (Actor currentactor in actorsList)
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
                foreach (KeyValuePair<String, SubSystem> sub2 in subSystemList)
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

        public void simulate()
        {
            bool finished = false;
            while (!finished)
            {
                daysTaken++;

                // MESSAGE SECTION

                // Go through all the actors
                foreach (Actor actor in actorsList)
                {
                    double messageTimeLeft = actor.totalMessageResponseTime;
                    foreach (Message message in actor.inbox)
                    {
                        // If enough time left, respond to message and subtract time
                        if (actor.messageResponseTime > messageTimeLeft)
                        {
                            message.answered = true;
                            messageTimeLeft -= actor.messageResponseTime;
                            actor.workingHoursLeftInDay -= actor.messageResponseTime;
                        }
                    }
                }

                // SUBSYSTEMS

                bool everythingIsDone = true;

                // Go through all the subsystems
                foreach (KeyValuePair<String, SubSystem> subsystem in subSystemList)
                {
                    // Go through all of the dependencies of the subsystem
                    foreach (KeyValuePair<SubSystem, double> dependency in subsystem.Value.subSystemDependencies) {
                        // If the dependency is required AND it has not been completed, block the subsystem
                        if (dependency.Value > dependencyThreshold && dependency.Key.hoursTillCompletion <= 0) 
                        {
                            subsystem.Value.isBlocked = true;
                        }
                    }

                    // If blocked, continue to the next subsystem
                    if (subsystem.Value.isBlocked)
                    {
                        continue;
                    }

                    // Go through actors
                    foreach (Actor actor in actorsList)
                    {
                        // If actor can work on the subsystem, work on it
                        if (actor.subSystem == subsystem.Value)
                        {
                            subsystem.Value.hoursTillCompletion -= actor.workingHoursLeftInDay;
                            actor.workingHoursLeftInDay = 0;
                        }
                    }

                    // If time still left, we're obviously not done
                    if (subsystem.Value.hoursTillCompletion > 0)
                    {
                        everythingIsDone = false;
                    }
                }

                // CHECK IF WE'RE DONE

                if (everythingIsDone)
                {
                    finished = true;
                    break;
                }

                // ASSUMPTIONS

                
                
        }
    }
}
