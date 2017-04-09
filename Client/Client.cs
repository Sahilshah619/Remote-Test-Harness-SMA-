/////////////////////////////////////////////////////////////////////
// Client.cs - Demonstrate application use of channel              //
// Ver 1.0                                                         //
// Jim Fawcett, CSE681 - Software Modeling and Analysis, Fall 2016 //
/////////////////////////////////////////////////////////////////////
/*
 * Package Operations:
 * -------------------
 * The Client package defines one class, Client, that uses the Comm<Client>
 * class to pass messages to a remote endpoint.
 * 
 * Required Files:
 * ---------------
 * - Client.cs
 * - ICommunicator.cs, CommServices.cs
 * - Messages.cs, MessageTest, Serialization
 *
 * Maintenance History:
 * --------------------
 * Ver 1.0 : 10 Nov 2016
 * - first release 
 *  
 */
/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Utilities;
using System.ServiceModel;
using System.IO;

namespace CommChannelDemo
{
    ///////////////////////////////////////////////////////////////////
    // Client class demonstrates how an application uses Comm
    //

    

        class Client
        {
            public Comm<Client> comm { get; set; } = new Comm<Client>();

            public string endPoint { get; } = Comm<Client>.makeEndPoint("http://localhost", 8081);

            private Thread rcvThread = null;

            //----< initialize receiver >------------------------------------

            public Client()
            {
                comm.rcvr.CreateRecvChannel(endPoint);
                rcvThread = comm.rcvr.start(rcvThreadProc);
            }
            //----< join receive thread >------------------------------------

            public void wait()
            {
                rcvThread.Join();
            }
            //----< construct a basic message >------------------------------

            public Message makeMessage(string author, string fromEndPoint, string toEndPoint)
            {
                Message msg = new Message();
                msg.author = author;
                msg.from = fromEndPoint;
                msg.to = toEndPoint;
                return msg;
            }
            //----< use private service method to receive a message >--------

            void rcvThreadProc()
            {
                while (true)
                {
                    Message msg = comm.rcvr.GetMessage();
                    msg.time = DateTime.Now;
                    Console.Write("\n  {0} received message:", comm.name);
                    msg.showMsg();
                    if (msg.body == "quit")
                        break;
                }
            }
            //----< run client demo >----------------------------------------

            static void Main(string[] args)
            {
                Console.Write("\n  Testing Client Demo");
                Console.Write("\n =====================\n");

                Client client = new Client();

                Message msg = client.makeMessage("Sahil Shah", client.endPoint, client.endPoint);
                Message msg1 = client.makeMessage("Rishi Dabre", client.endPoint, client.endPoint);
                //  client.comm.sndr.PostMessage(msg); //SENDS TO CLIENT ONLY
                //  msg = client.makeMessage("Sahil Shah", client.endPoint, client.endPoint);
                //msg.body = MessageTest.makeTestRequest();  //BODY IS SET TO NULL
                //client.comm.sndr.PostMessage(msg);

                string remoteEndPoint = Comm<Client>.makeEndPoint("http://localhost", 8080); //THIS SENDS IT TO SERVER
                                                                                             //   string remoteEndPoint1 = Comm<Client>.makeEndPoint("http://localhost", 8000);
                Console.WriteLine("=====TEST REQUEST #1=======");  //MULTIPLE TEST REQUEST
                msg.body = MessageTest.makeTestRequest();
                msg.to = remoteEndPoint;
                //   msg.to = remoteEndPoint1;
                Console.WriteLine("=====TEST REQUEST #2=======");
                msg1.body = MessageTest.makeAnotherTestRequest();
                msg1.to = remoteEndPoint;
                client.comm.sndr.PostMessage(msg);
                client.comm.sndr.PostMessage(msg1);

                Console.Write("\n  press key to exit: ");
                Console.ReadKey();
                msg.to = client.endPoint;
                msg.body = "quit";
                client.comm.sndr.PostMessage(msg);
                client.wait();
                Console.Write("\n\n");

            }
        }
    }
    */

