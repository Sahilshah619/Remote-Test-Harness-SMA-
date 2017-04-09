///////////////////////////////////////////////////////////////////////
// StreamService.cs - WCF StreamService in Self Hosted Configuration //
//                                                                   //
//  Source: Jim Fawcett                                              //                                            
//  Author: Sahil Shah                                                //
///////////////////////////////////////////////////////////////////////
/*
 * Note:
 * - Uses Programmatic configuration, no app.config file used.
 * - Uses ChannelFactory to create proxy programmatically. 
 * - Expects to find ToSend directory under application with files
 *   to send.
 * - Will create SavedFiles directory if it does not already exist.
 * - Users HRTimer.HiResTimer class to measure elapsed microseconds.
 */

using System;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace CSE681SMA
{
    // Can't configure SecurityMode other than none with streaming.
    // This is the default for BasicHttpBinding.
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class StreamService : IStreamService
    {
        string filename;
        string savePath = @"..\..\..\Service\TestDll's";
        string ToSendPath = @"..\..\ToSend";
        int BlockSize = 1024;
        byte[] block;
        HRTimer.HiResTimer hrt = null;

        StreamService()
        {
            block = new byte[BlockSize];
            hrt = new HRTimer.HiResTimer();
        }

        //---<Uploads file through communication Channel>------
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
            Console.Write("\n  Sent \"{0}\" in {1} microsec.", filename, hrt.ElapsedMicroseconds);
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
            Type service = typeof(CSE681SMA.StreamService);
            ServiceHost host = new ServiceHost(service, baseAddress);
            host.AddServiceEndpoint(typeof(IStreamService), binding, baseAddress);
            return host;
        }

        public static void Main()
        {
            Console.Write(" ================================================================================================\n");
            Console.Write("\n                                    REPOSITORY WINDOW\n");
            Console.Write("\n ================================================================================================\n");
            ServiceHost host = CreateServiceChannel("http://localhost:8083/StreamService");
            host.Open();
            Console.Write("\n  Files from Client Receieved---------------->Requirement #2");
            Console.Write("\n ========================================\n");
            Console.Write("\n  Press key to terminate service:\n");       
            Console.ReadKey();
            Console.Write("\n");
            host.Close();

        }
    }
}
