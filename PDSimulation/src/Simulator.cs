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

        BetterRandom r;

        public bool sequential = false;

        public double daysTaken { get; set; }

        public double dependencyThreshold = 0.4;

        public Simulator()
        {
            r = new BetterRandom();
            mainData = new DataReader("../../data/actualtest.csv");
            subSystems = new DataReader("../../data/subsystems.csv");
            daysTaken = 0;
        }

        // Build the data 
        public void populate()
        {
            while (subSystems.data.ReadNextRecord())
            {
                SubSystem sub = new SubSystem(subSystems.data["name"], false);
                subSystemList.Add(subSystems.data["name"], sub);
            }

            while (mainData.data.ReadNextRecord())
            {
                SubSystem sub = subSystemList[mainData.data["subsystem"]];
                double totalmessagetime = Convert.ToDouble(mainData.data["totalmessageresponsetime"]);
                double centralization = DataReader.getProbabilityFromScale(mainData.data["centralization"], 1, 5);
                double assumptions = DataReader.getProbabilityFromScale(mainData.data["assumptions"], 1, 5);
                double messageresponserate = DataReader.getProbabilityFromScale(mainData.data["messageresponserate"], 1, 5) + 0.5;
                double assumptionaccuracy = DataReader.getProbabilityFromScale(mainData.data["assumptionaccuracy"], 1, 5);
                double assumptioneffect = DataReader.getProbabilityFromScale(mainData.data["assumptioneffect"], 1, 5);
                double tasktime = Convert.ToDouble(mainData.data["tasktime"]);

                sub.setTime(tasktime);

                Actor actor = new Actor(sub, totalmessagetime, centralization, assumptions, messageresponserate, assumptionaccuracy, assumptioneffect);
                actorsList.Add(actor);
                actor.resetDay();

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
           // Console.WriteLine(subSystemList["sub x"].subSystemDependencies.Count);
        }

        public void simulate()
        {
            daysTaken = 0;
            bool finished = false;
            while (!finished)
            {
                daysTaken++;

                if (daysTaken > 362)
                {
                    //Console.WriteLine(":(");
                }

                if (daysTaken > 365)
                {
                    
                    finished = true;
                    daysTaken = -12345;
                }

                /* Day structure
                 *  - Reset Day
                 *  - Answer Messages
                 *  - Determine which tasks are blocked
                 *  - Do work (record amount of work done)
                 *  - Do passive rework
                 *  - Send messages
                 *  - Make assumptions from unanswered messages
                */

                // RESET DAY


                // ANSWER MESSAGES

                // Go through all the actors
                foreach (Actor actor in actorsList)
                {
                    try
                    {

                        if (actor.subSystem.inbox.Count > 10)
                        {
                            throw new Exception("HELP");
                        }
                    } catch (Exception e)
                    {
                        //Console.WriteLine(":)");
                    }

                    double messageTimeLeft = actor.totalMessageResponseTime;
                    foreach (Message message in actor.subSystem.inbox)
                    {
                        // If enough time left, respond to message and subtract time
                        if (messageTimeLeft >= (message.messageResponseTime / actor.messageResponseRate) && !message.answered)
                        {
                            message.answered = true;
                            messageTimeLeft -= (message.messageResponseTime / actor.messageResponseRate);
                            actor.workingHoursLeftInDay -= (message.messageResponseTime / actor.messageResponseRate);
                        }
                    }

                    foreach (Message m in actor.outbox.ToList())
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
                                }
                            }

                            // Delete the message
                            m.sender.outbox.Remove(m);
                            m.recipient.inbox.Remove(m);
                        }
                    }
                }



                if (daysTaken > 50)
                {
                   // Console.WriteLine(":(");
                }

                // DO WORK

                bool everythingIsDone = true;

                // Go through all the subsystems
                foreach (KeyValuePair<String, SubSystem> subsystem in subSystemList)
                {
                    // Check if the subsystem has been unblocked
                    subsystem.Value.checkStatus();

                    // Go through actors
                    foreach (Actor actor in actorsList)
                    {
                        if (!sequential)
                        {
                            // If actor can work on the subsystem, work on it
                            if (actor.subSystem == subsystem.Value && !subsystem.Value.isBlocked && (subsystem.Value.hoursTillCompletion > 0))
                            {
                                subsystem.Value.hoursTillCompletion -= actor.workingHoursLeftInDay;
                                actor.timeWorkedThisDay = actor.workingHoursLeftInDay;
                                actor.workingHoursLeftInDay = 0;
                            }
                        }
                        else
                        {
                            if (actor.subSystem.hoursTillCompletion < 0)
                            {
                                continue;
                            }
                            actor.subSystem.hoursTillCompletion -= actor.workingHoursLeftInDay;
                            actor.timeWorkedThisDay = actor.workingHoursLeftInDay;
                            actor.workingHoursLeftInDay = 0;
                            break;
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

                // PASSIVE REWORK

                foreach (Actor actor in actorsList)
                {
                    /*
                     * Probability of rework occuring during a day = 
                     * Probability of an assumption happening x The effect of the assumption
                     * 
                     * If an assumption occurs, add one day of work
                     */
                    double pRework = actor.assumptionChance * actor.assumptionEffect;
                    if (doesEventOccur(pRework) && !actor.subSystem.isBlocked)
                    {
                        actor.subSystem.hoursTillCompletion += actor.timeWorkedThisDay;
                        //Console.WriteLine("rework was done");
                    }
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
                        if (doesEventOccur(messagechance) && !actor.subSystem.isBlocked)
                        {
                            // if a message is sent, add it to the actor who sent it and the subsystem that will receive it, and block the subsystem that sent the message
                            Message m = new Message(actor, kvp.Key, 1);
                            actor.outbox.Add(m);
                            kvp.Key.inbox.Add(m);
                            actor.subSystem.isBlocked = true;
                        }
                    }
                }

                // CHECK IF ACTORS MAKE ASSUMPTIONS BASED ON UNANSWERED MESSAGES

                // Go through all the actors 
                foreach (Actor actor in actorsList)
                {
                    // If the actors subsystem is blocked
                    if (actor.subSystem.isBlocked)
                    {
                        foreach (Message m in actor.outbox)
                        {
                            if (m.daysSinceSent > 2)
                            {
                                // Check if the actor makes an assumption
                                if (doesEventOccur(actor.assumptionChance))
                                {
                                    m.answerAssumed = true;
                                }
                            }
                        }
                    }
                }


                // RESET DAY

                // Loop through all the messages in the system
                foreach (Actor actor in actorsList)
                {
                    foreach (Message m in actor.outbox.ToList())
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
                                }
                            }

                            // Delete the message
                            m.sender.outbox.Remove(m);
                            m.recipient.inbox.Remove(m);
                        }

                        // increment the number of days the message has been out for
                        m.daysSinceSent++;
                    }
                    actor.resetDay();
                }
                //  ALL MESSAGES ARE BEING ANSWERED AT THE BEGINNING OF THE DAY, BUT NO WORK WAS BEING DONE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! READMEPLS 
                //
                if (daysTaken > 300)
                {
                    //Console.WriteLine(":(");
                }
            }
        }

        // returns true if the event occurs (0->1 probability)
        public bool doesEventOccur(double chance)
        {
            return (r.NextDouble() < chance);
        }

        public bool doesEventOccur(double chance, double weighting)
        {
            return ((r.NextDouble() * weighting) < chance);
        }
    }

    }
