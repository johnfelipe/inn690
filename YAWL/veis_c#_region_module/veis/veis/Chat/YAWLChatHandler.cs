﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Veis.Workflow.YAWL;
using Veis.Workflow;

namespace Veis.Chat
{
    /// <summary>
    /// When the YAWLAgent is implemented, it needs to be passed
    /// into the chat handler so queries can be made on it.
    /// </summary>
    public class YAWLChatHandler : ChatHandler
    {
        private readonly YAWLAgent _yawlAgent;
        private readonly YAWLWorkflowProvider _yawlProvider;
        
        public YAWLChatHandler(YAWLAgent agent, YAWLWorkflowProvider yawlWorkflowProvider)
        {
            _yawlAgent = agent;
            _yawlProvider = yawlWorkflowProvider;
        }
        
        public override bool CanHandleMessage(string message)
        {
            return message.Split(':')[0] == "YAWL";
        }

        protected override string ProcessMessage(string answer, string pre, string post)
        {
            //System.Console.WriteLine("Answer1: " + answer);

            string output = "I DON'T KNOW WHAT WORKFLOW IS!";

            string request = answer.Split(':')[1];

            if (request == "CURRENTWORK")
            {
                //System.Console.WriteLine("A1");
                string tasks = "";

                lock (_yawlAgent.processing)
                {
                    //System.Console.WriteLine("A2: ");
                    //System.Console.WriteLine(_yawlAgent.processing.Count);
                    for (int i = 0; i < _yawlAgent.processing.Count; i++ )
                    {
                        tasks += _yawlAgent.processing[i].taskName;// +", ";
                        if (i != _yawlAgent.processing.Count - 1) tasks += ", ";
                    }
                }

                output = pre + tasks + post;
            }
            else if (request == "WORKQUEUE")
            {

                string tasks = "";

                lock (_yawlAgent.started)
                {
                    foreach (WorkItem workItem in _yawlAgent.started)
                    {
                        tasks += workItem.taskName + ", ";
                    }
                }

                output = pre + tasks + post;
            }
            else if (request == "COMPLETEDWORK")
            {

                string tasks = "";

                lock (_yawlAgent.completed)
                {
                    foreach (WorkItem workItem in _yawlAgent.completed)
                    {
                        tasks += workItem.taskName + ", ";
                    }
                }

                output = pre + tasks + post;
            }
            else if (request == "SUSPENDEDWORK")
            {

                string tasks = "";

                lock (_yawlAgent.completed)
                {
                    foreach (WorkItem workItem in _yawlAgent.suspended)
                    {
                        tasks += workItem.taskName + ", ";
                    }
                }

                output = pre + tasks + post;
            }
            else if (request == "OFFEREDWORK")
            {

                string tasks = "";

                lock (_yawlAgent.completed)
                {
                    foreach (WorkItem workItem in _yawlAgent.offered)
                    {
                        tasks += workItem.taskName + ", ";
                    }
                }

                output = pre + tasks + post;
            }
            else if (request == "ALLOCATEDWORK")
            {

                string tasks = "";

                lock (_yawlAgent.completed)
                {
                    foreach (WorkItem workItem in _yawlAgent.allocated)
                    {
                        tasks += workItem.taskName + ", ";
                    }
                }

                output = pre + tasks + post;
            }
            else if (request == "DELEGATEDWORK")
            {

                string tasks = "";

                lock (_yawlAgent.completed)
                {
                    foreach (WorkItem workItem in _yawlAgent.delegated)
                    {
                        tasks += workItem.taskName + ", ";
                    }
                }

                output = pre + tasks + post;
            }
            
            else if (request == "CAPABILITIES")
            {
                string capabilities = "";

                foreach (string cap in _yawlAgent.Capabilities)
                {
                    capabilities += cap + ", ";
                }

                output = pre + capabilities + post;
            }
            else if (request == "ROLES")
            {
                string roles = "";

                foreach (string role in _yawlAgent.Roles)
                {
                    roles += role + ", ";
                }

                output = pre + roles + post;
            }

            else if (request == "ALLAGENTS")
            {
                string agents = "";

                foreach (KeyValuePair<string, WorkAgent> kvp in _yawlProvider.AllParticipants)
                {
                    agents += kvp.Value + ", ";
                }

                output = pre + agents + post;
            }
            else
            {
                _yawlProvider.Send(answer.Split(new char[] {':'}, 2)[1]);
            }

            return output;
        }
    }
}
