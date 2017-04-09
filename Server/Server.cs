/////////////////////////////////////////////////////////////////////
// Server.cs - Demonstrate application use of channel              //
//                                                                 //
// Author: Sahil Shah                                              //
/////////////////////////////////////////////////////////////////////
/*
 * Package Operations:
 * -------------------
 * The Server package defines one class, Server, that uses the Comm<Server>
 * class to receive messages from a remote endpoint.
 * 
 * Required Files:
 * ---------------
 * - Server.cs
 * - ICommunicator.cs, CommServices.cs
 * - Messages.cs, MessageTest, Serialization
 * - first release   
 */
using System;
using System.Threading;
using System.ServiceModel;
using System.IO;
using CommChannelDemo;

namespace RepoToMain
{
   //----< Uses ChannelFactory to create proxy programmatically>-------
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class StreamService : IStreamService
    {
        //--< Will create SavedFiles directory if it does not already exist>-----
        //--<Expects to find ToSend directory under application with files to send>----
        string filename;
        string savePath = @"..\..\..\Server\Test's";
        string ToSendPath = @"..\..\..\Service\DLL";
        int BlockSize = 1024;
        byte[] block;
        HRTimer.HiResTimer hrt = null;

        StreamService()
        {
            block = new byte[BlockSize];
            hrt = new HRTimer.HiResTimer();
        }
        //----< use service method to send files to a remote server >--------
        public void upLoadFile(FileTransferMessage msg)
        {
            int totalBytes = 0;
            hrt.Start();
            filename = msg.filename;
            string rfilename = Path.Combine(savePath, filename);
            if (!Directory.Exists(savePath))
                Directory.CreateDirectory(savePath);
            using (var outputStream = new FileStream(rfilename, FileMode.Create))
            {
                while (true)
                {
                    int bytesRead = msg.transferStream.Read(block, 0, BlockSize);
                    totalBytes += bytesRead;
                    if (bytesRead > 0)
                        outputStream.Write(block, 0, bytesRead);
                    else
                        break;
                }
            }
            hrt.Stop();
            Console.Write(
              "\n  Received file \"{0}\" of {1} bytes in {2} microsec.",
              filename, totalBytes, hrt.ElapsedMicroseconds
            );
        }
        //---<Downloads a file from a  directory>------

        public Stream downLoadFile(string filename)
        {
            hrt.Start();
            string sfilename = Path.Combine(ToSendPath, filename);
            FileStream outStream = null;
            if (File.Exists(sfilename))
            {
                outStream = new FileStream(sfilename, FileMode.Open);
            }
            else
                throw new Exception("open failed for \"" + filename + "\"");
            hrt.Stop();
            return outStream;
        }

        static ServiceHost CreateServiceChannel(string url)
        {
            // Can't configure SecurityMode other than none with streaming.
            // This is the default for BasicHttpBinding.
            //   BasicHttpSecurityMode securityMode = BasicHttpSecurityMode.None;
            //   BasicHttpBinding binding = new BasicHttpBinding(securityMode);
            BasicHttpBinding binding = new BasicHttpBinding();
            binding.TransferMode = TransferMode.Streamed;
            binding.MaxReceivedMessageSize = 50000000;
            Uri baseAddress = new Uri(url);
            Type service = typeof(RepoToMain.StreamService);
            ServiceHost host = new ServiceHost(service, baseAddress);
            host.AddServiceEndpoint(typeof(IStreamService), binding, baseAddress);
            return host;
        }
        class Server
        {
            //----< initialize receiver >------------------------------------
            public Comm<Server> comm { get; set; } = new Comm<Server>();
            public string endPoint { get; } = Comm<Server>.makeEndPoint("http://localhost", 8080);
            private Thread rcvThread = null;
            public Server()
            {
                comm.rcvr.CreateRecvChannel(endPoint);
                rcvThread = comm.rcvr.start(rcvThreadProc);
            }
            //----< join receive thread >------------------------------------
            public void wait()
            {
                rcvThread.Join();
            }
            public Message makeMessage(string author, string fromEndPoint, string toEndPoint)
            {
                //----< construct a basic message >------------------------------
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
            static void Main(string[] args)
            {
                Console.Write(" ================================================================================================\n");
                Console.Write("\n                                    TEST HARNESS WINDOW\n");
                Console.Write("\n ================================================================================================\n");
                Console.Write("\n  Files from Repository Receieved");
                Console.Write("\n ========================================\n");
                ServiceHost host = CreateServiceChannel("http://localhost:8082/StreamService");
                host.Open();
                Server Server = new Server();
                Console.Write("\n  press key for Testing! ");
                Console.ReadKey();
                host.Close();
                TestH.Creator newDomain = new TestH.Creator();   //Calling the create AppDomain function using object
                newDomain.CreateChildDomain();
                Message msg = Server.makeMessage("Sahil Shah", Server.endPoint, Server.endPoint);
                string remoteEndPoint = Comm<Server>.makeEndPoint("http://localhost", 8081);
                msg.to = remoteEndPoint;
                msg.body = MessageTest.errorMessage();
                Server.comm.sndr.PostMessage(msg);
                Console.ReadKey();
                Console.Write("\n  press key to exit: ");
            }
        }
    }
}

