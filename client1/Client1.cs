///////////////////////////////////////////////////////////////////////
// Client.cs - WCF Timed, SelfHosted, File StreamService client      //
//                                                                   //
// Source: Dr. Jim Fawcett                                           //                                        
//Author: Sahil Shah                                                 //
///////////////////////////////////////////////////////////////////////
/*
 * Note:
 * - Uses Programmatic configuration, no app.config file used.
 * - Uses ChannelFactory to create proxy programmatically. 
 * - Expects to find ToSend directory under application with files
 *   to send.
 * - Will create SavedFiles directory if it does not already exist.
 *
 * PUBLIC Classes:-
 * No Public Class
 * 
 */

using CommChannelDemo;
using System;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;

namespace CSE681SMA
{
    class Client
    {
        public Comm<Client> comm { get; set; } = new Comm<Client>();
        public string endPoint { get; } = Comm<Client>.makeEndPoint("http://localhost", 8081);
        private Thread rcvThread = null;
        //----< initialize receiver >------------------------------------
        Client()
        {
            comm.rcvr.CreateRecvChannel(endPoint);
            rcvThread = comm.rcvr.start(rcvThreadProc);
        }
        //----< join receive thread >------------------------------------
        void wait()
        {
            rcvThread.Join();
        }
        //----< construct a basic message >------------------------------
        CommChannelDemo.Message makeMessage(string author, string fromEndPoint, string toEndPoint)
        {
            CommChannelDemo.Message msg = new CommChannelDemo.Message();
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
                CommChannelDemo.Message msg = comm.rcvr.GetMessage();
                msg.time = DateTime.Now;
                Console.Write("\n  {0} received message:", comm.name);
                msg.showMsg();
                if (msg.body == "quit")
                    break;
            }
        }
        class Client1
        {
            IStreamService channel;
            string ToSendPath = @"..\\..\\ToSend";
            string SavePath = @"../../../Service\TestDll's";
            int BlockSize = 1024;
            byte[] block;
            HRTimer.HiResTimer hrt = null;

            Client1()
            {
                block = new byte[BlockSize];
                hrt = new HRTimer.HiResTimer();
            }
            static IStreamService CreateServiceChannel(string url)
            {
                BasicHttpSecurityMode securityMode = BasicHttpSecurityMode.None;
                BasicHttpBinding binding = new BasicHttpBinding(securityMode);
                binding.TransferMode = TransferMode.Streamed;
                binding.MaxReceivedMessageSize = 500000000;
                EndpointAddress address = new EndpointAddress(url);
                ChannelFactory<IStreamService> factory
                  = new ChannelFactory<IStreamService>(binding, address);
                return factory.CreateChannel();
            }
            //----< use service method to send files to a remote server >--------
            void uploadFile(string filename)
            {
                string fqname = Path.Combine(ToSendPath, filename);
                try
                {
                    hrt.Start();
                    using (var inputStream = new FileStream(fqname, FileMode.Open))
                    {
                        FileTransferMessage msg = new FileTransferMessage();
                        msg.filename = filename;
                        msg.transferStream = inputStream;
                        channel.upLoadFile(msg);
                    }
                    hrt.Stop();
                    Console.Write("\n  Uploaded file \"{0}\" in {1} microsec.", filename, hrt.ElapsedMicroseconds);
                }
                catch
                {
                    Console.Write("\n  can't find the file\"{0}\"------->Requirement #3", fqname);
                }
            }
            //----< use service method to download a message >--------
            void download(string filename)
            {
                int totalBytes = 0;
                try
                {
                    hrt.Start();
                    Stream strm = channel.downLoadFile(filename);
                    string rfilename = Path.Combine(SavePath, filename);
                    if (!Directory.Exists(SavePath))
                        Directory.CreateDirectory(SavePath);
                    using (var outputStream = new FileStream(rfilename, FileMode.Create))
                    {
                        while (true)
                        {
                            int bytesRead = strm.Read(block, 0, BlockSize);
                            totalBytes += bytesRead;
                            if (bytesRead > 0)
                                outputStream.Write(block, 0, bytesRead);
                            else
                                break;
                        }
                    }
                    hrt.Stop();
                    ulong time = hrt.ElapsedMicroseconds;
                    Console.Write("\n  Received file \"{0}\" of {1} bytes in {2} microsec.", filename, totalBytes, time);
                }
                catch (Exception ex)
                {
                    Console.Write("\n  {0}", ex.Message);
                }
            }

            static void Main()
            {
                HRTimer.HiResTimer hrt = new HRTimer.HiResTimer();
                hrt.Start();
                Client client = new Client();
                CommChannelDemo.Message msg = client.makeMessage("Sahil Shah", client.endPoint, client.endPoint);
                CommChannelDemo.Message msg1 = client.makeMessage("Rishi Dabre", client.endPoint, client.endPoint);
                string remoteEndPoint = Comm<Client>.makeEndPoint("http://localhost", 8080);
                msg.body = MessageTest.makeTestRequest();
                msg.to = remoteEndPoint;
                msg1.body = MessageTest.makeAnotherTestRequest();
                msg1.to = remoteEndPoint;
                client.comm.sndr.PostMessage(msg);
                client.comm.sndr.PostMessage(msg1);
                Console.Write("\n");
                Console.Write("\n");
                Console.Write(" ================================================================================================\n");
                Console.Write("\n                                    CLIENT WINDOW\n");
                Console.Write("\n ================================================================================================\n");
                Console.Write("\n\n  Client Sending Files to Repository for the test request---------------->Requirement #2");
                Console.Write("\n =======================================================\n");
                Client1 clnt = new Client1();
                clnt.channel = CreateServiceChannel("http://localhost:8083/StreamService");
                Console.Write("\n File Streaming Without Queue------>Requirement #6\n");
                hrt.Start();
                clnt.uploadFile("TryToTest.dll");
                clnt.uploadFile("DivideTest.dll");
                clnt.uploadFile("TestCode2.dll");
                clnt.uploadFile("TestCode3.dll");
                clnt.uploadFile("TestCode4.dll");
                clnt.uploadFile("TestDriver1.dll");
                clnt.uploadFile("TestDriver2.dll");
                clnt.uploadFile("TestDriver3.dll");
                clnt.uploadFile("TestDriver4.dll");
                hrt.Stop();
                Console.Write("\n\n  total elapsed time for uploading = {0} microsec------>Requirement #12.\n", hrt.ElapsedMicroseconds);
                Console.WriteLine("\n\n===============================TEST RESULTS=========================------------->Requirement #6\n\n");
                string text = System.IO.File.ReadAllText(@"../../../Service/DLL/logfile.txt");
                System.Console.WriteLine("{0}", text);
                hrt.Stop();
                Console.Write("\n\n  total elapsed time for Entire Execution = {0} micro seconds------>Requirement #12.\n", hrt.ElapsedMicroseconds);
                ((IChannel)clnt.channel).Close();
            }
        }
    }
}
