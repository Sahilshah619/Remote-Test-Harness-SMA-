///////////////////////////////////////////////////////////////////////
// TestHarnessServer.cs - Server for the GUI client                  //
//                                                                   //
//                                                                   //                                        
//  Author: Sahil Shah                                               //
///////////////////////////////////////////////////////////////////////
/*
 * Note:
 * - Uses xaml to create a graphical user Interface
 * - Used combobox, textbox, label and button
 * - Expected to perform all client activities
 * - uses WCF communication for message and file 
 *
 * PUBLIC Classes:-
 * StreamService, uploadfile
 * Server
 *
 */
using CommChannelDemo;
using CSE681SMA;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GUIHarnessTest
{

    //----< Uses ChannelFactory to create proxy programmatically>-------
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class StreamService : IStreamService
    {
        //--< Will create SavedFiles directory if it does not already exist>-----
        //--<Expects to find ToSend directory under application with files to send>----
        string filename;
        string savePath1 = @"..\..\..\GUI-Repository\SelectedDll's";
        string ToSendPath1 = @"..\..\..\GUI-Client\ToSend";
        int BlockSize1 = 1024;
        byte[] block1;
        HRTimer.HiResTimer hrt1 = null;

        StreamService()
        {
            block1 = new byte[BlockSize1];
            hrt1 = new HRTimer.HiResTimer();
        }
        //----< use service method to send files to a remote server >--------
        public void upLoadFile(FileTransferMessage msg)
        {
            int totalBytes = 0;
            hrt1.Start();
            filename = msg.filename;
            string rfilename = Path.Combine(savePath1, filename);
            if (!Directory.Exists(savePath1))
                Directory.CreateDirectory(savePath1);
            using (var outputStream = new FileStream(rfilename, FileMode.Create))
            {
                while (true)
                {
                    int bytesRead = msg.transferStream.Read(block1, 0, BlockSize1);
                    totalBytes += bytesRead;
                    if (bytesRead > 0)
                        outputStream.Write(block1, 0, bytesRead);
                    else
                        break;
                }
            }
            hrt1.Stop();
            Console.Write(
              "\n  Received file \"{0}\" of {1} bytes in {2} microsec.",
              filename, totalBytes, hrt1.ElapsedMicroseconds
            );
        }
        //---<Downloads a file from a  directory>------

        public Stream downLoadFile(string filename)
        {
            hrt1.Start();
            string sfilename = Path.Combine(ToSendPath1, filename);
            FileStream outStream = null;
            if (File.Exists(sfilename))
            {
                outStream = new FileStream(sfilename, FileMode.Open);
            }
            else
                throw new Exception("open failed for \"" + filename + "\"");
            hrt1.Stop();
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
            Type service = typeof(GUIHarnessTest.StreamService);
            ServiceHost host = new ServiceHost(service, baseAddress);
            host.AddServiceEndpoint(typeof(IStreamService), binding, baseAddress);
            return host;
        }

        public class Server
        {
            public Comm<Server> comm { get; set; }
            public string endPoint { get; }
            private Thread rcvThread = null;

            public Server()
            {
                comm = new Comm<Server>();
                endPoint = Comm<Server>.makeEndPoint("http://localhost", 9090);
                comm.rcvr.CreateRecvChannel(endPoint);
                rcvThread = comm.rcvr.start(rcvThreadProc);
            }

            public void wait()
            {
                rcvThread.Join();
            }
            public Message makeMessage(string author, string fromEndPoint, string toEndPoint)
            {
                Message msg = new Message();
                msg.author = author;
                msg.from = fromEndPoint;
                msg.to = toEndPoint;
                return msg;
            }

            void rcvThreadProc()
            {
                while (true)
                {
                    Message msg = comm.rcvr.GetMessage();
                    msg.time = DateTime.Now;
                    Console.Write("\n  {0} received message:", comm.name);
                    msg.showMsg();
                    TestH.ApplicationDomain newdomain = new TestH.ApplicationDomain();
                    newdomain.CreateChildDomain();
                    if (msg.body == "quit")
                        break;
                }
            }

            static void Main(string[] args)
            {
                Console.Write(" ================================================================================================\n");
                Console.Write("\n                                   GUI TEST HARNESS WINDOW\n");
                Console.Write("\n ================================================================================================\n");
                Server Server = new Server();
                Console.Write("\n\n");
                ServiceHost host = CreateServiceChannel("http://localhost:9002/StreamService");
                host.Open();
                Console.ReadKey();
                Console.ReadLine();
                Console.Write("\n");
            }
        }
    }
}

