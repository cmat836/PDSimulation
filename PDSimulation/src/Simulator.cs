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
                double totalmessagetime = Convert.ToDouble(mainData.data["totalmessageresponsetime"]);
                double centralization = DataReader.getProbabilityFromScale(mainData.data["centralization"], 1, 5);
                double assumptions = DataReader.getProbabilityFromScale(mainData.data["assumptions"], 1, 5);
                double messageresponsetime = Convert.ToDouble(mainData.data["messageresponsetime"]);
                double assumptionaccuracy = DataReader.getProbabilityFromScale(mainData.data["assumptionaccuracy"], 1, 5);
                double assumptioneffect = DataReader.getProbabilityFromScale(mainData.data["assumptioneffect"], 1, 5);

                Actor actor = new Actor(sub, totalmessagetime, centralization, assumptions, messageresponsetime, assumptionaccuracy, assumptioneffect);
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
                    bool unansweredMessages = false;

                    double messageTimeLeft = actor.totalMessageResponseTime;
                    foreach (Message message in actor.subSystem.inbox)
                    {
                        // If enough time left, respond to message and subtract time
                        if (actor.messageResponseTime > messageTimeLeft)
                        {
                            message.answered = true;
                            messageTimeLeft -= actor.messageResponseTime;
                            actor.workingHoursLeftInDay -= actor.messageResponseTime;
                        }

                        if (!(message.answered || message.answerAssumed))
                        {
                            unansweredMessages = true;
                        }
                    }

                    // If there are no pending messages for the actor's subsystem, unblock it
                    if (!unansweredMessages)
                    {
                        actor.subSystem.isBlocked = false;
                    }
                }

                // SUBSYSTEMS

                bool everythingIsDone = true;

                // Go through all the subsystems
                foreach (KeyValuePair<String, SubSystem> subsystem in subSystemList)
                {
                    // Go through all of the dependencies of the subsystem
                    foreach (KeyValuePair<SubSystem, double> dependency in subsystem.Value.subSystemDependencies)
                    {
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

                // SENDING MESSAGES

                // Go through all the actors 
                foreach (Actor actor in actorsList)
                {
                    // Go through his subsystems dependency's
                    foreach (KeyValuePair<SubSystem, double> kvp in actor.subSystem.subSystemDependencies)
                    {
                        // Chance to generate a message, calculated by weighting the dependency of the actors subsystem on the other subsystem  by the percentage of time left in the task
                        // I.e. if there is alot of time left in the task the chance for a message to occur will be higher (closer to the DSM) than if there is little time left
                        double messagechance = kvp.Value * (actor.subSystem.hoursTillCompletion / actor.subSystem.totalTimeNeeded);
                        if (doesEventOccur(messagechance))
                        {
                            // if a message is sent, add it to the actor who sent it and the subsystem that will receive it, and block the subsystem that sent the message
                            Message m = new Message(actor, kvp.Key);
                            actor.outbox.Add(m);
                            kvp.Key.inbox.Add(m);
                            actor.subSystem.isBlocked = true;
                        }
                    }
                }

                // ASSUMPTIONS

                // Go through all the actors 
                foreach (Actor actor in actorsList)
                {
                    // If the actors subsystem is blocked
                    if(actor.subSystem.isBlocked)
                    {
                        foreach (Message m in actor.subSystem.inbox)
                        {
                            // Check if the actor makes an assumption
                            if (doesEventOccur(actor.assumptionChance))
                            {
                                m.answerAssumed = true;
                            }
                        }
                    }
                }

                // Loop through all the messages in the system
                foreach (Actor actor in actorsList)
                {
                    foreach (Message m in actor.outbox)
                    {
                        if (m.answered)
                        {
                            // If the message had an answer assumed, and then has been answered properly
                            if (m.answerAssumed)
                            {
                                // Check if the assumption was not correct
                                if (doesEventOccur(m.sender.assumptionAccuracy))
                                {
                                    // Add more time the the subsystem based on how much of an effect bad assumptions have
                                    double addedTime = m.sender.assumptionEffect * m.daysSinceSent;
                                    m.sender.subSystem.hoursTillCompletion += addedTime;

///////////////////////////////////////////////////////////////////////////////////  CASCADING REWORK GOES HERE ////////////////////////////////////////////////////////////////////////////////
                                }
                            }

                            // Delete the message
                            m.sender.outbox.Remove(m);
                            m.recipient.inbox.Remove(m);
                        }

                        // increment the number of days the message has been out for
                        m.daysSinceSent++;
                    }
                }
            }
        }

        // returns true if the event occurs (0->1 probability)
        public bool doesEventOccur(double chance)
        {
            Random r = new Random();
            return (r.NextDouble() < chance);
        }

        public bool doesEventOccur(double chance, double weighting)
        {
            Random r = new Random();
            return ((r.NextDouble() * weighting) < chance);
        }
    }

    }
