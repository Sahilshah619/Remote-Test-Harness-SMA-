﻿///////////////////////////////////////////////////////////////////////
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

namespace RepoToMain
{
    class Clientx
    {
        IStreamService channel;
        string ToSendPath = @"..\..\..\Service\DLL";
        string SavePath = @"..\..\..\Server\Test's";
        int BlockSize = 1024;
        byte[] block;
        HRTimer.HiResTimer hrt = null;

        Clientx()
        {
            block = new byte[BlockSize];
            hrt = new HRTimer.HiResTimer();
        }
        static IStreamService CreateServiceChannel(string url)
        {
            // Can't configure SecurityMode other than none with streaming.
            // This is the default for BasicHttpBinding.
            //   BasicHttpSecurityMode securityMode = BasicHttpSecurityMode.None;
            //   BasicHttpBinding binding = new BasicHttpBinding(securityMode);

            BasicHttpSecurityMode securityMode = BasicHttpSecurityMode.None;
            BasicHttpBinding binding = new BasicHttpBinding(securityMode);
            binding.TransferMode = TransferMode.Streamed;
            binding.MaxReceivedMessageSize = 500000000;
            EndpointAddress address = new EndpointAddress(url);

            ChannelFactory<IStreamService> factory
              = new ChannelFactory<IStreamService>(binding, address);
            return factory.CreateChannel();
        }
        //---<Uploads file through communication Channel>------
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
            catch (Exception e)
            {
                Console.Write("\n  can't find \"{0}\" Error:\n{1}", fqname, e.Message);
            }
        }
        //---<Downloads a file from a  directory>------
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
                hrt.Stop();   //To measure Time required to communicate
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
            Console.Write(" ================================================================================================\n");
            Console.Write("\n                                    REPOSITORY WINDOW\n");
            Console.Write("\n ================================================================================================\n");
            Console.Write("\n  Sending DLL files from Repository to Test Harness Server");
            Console.Write("\n ==========================================\n");
            Clientx clnt = new Clientx();
            clnt.channel = CreateServiceChannel("http://localhost:8082/StreamService");
            HRTimer.HiResTimer hrt = new HRTimer.HiResTimer();
            hrt.Start();
            clnt.uploadFile("DivideTest.dll");
            clnt.uploadFile("TestCode2.dll");
            clnt.uploadFile("TestCode3.dll");
            clnt.uploadFile("TestCode4.dll");
            clnt.uploadFile("TestDriver1.dll");
            clnt.uploadFile("TestDriver2.dll");
            clnt.uploadFile("TestDriver3.dll");
            clnt.uploadFile("TestDriver4.dll");
            hrt.Stop();
            Console.Write("\n\n  total elapsed time for uploading = {0} microsec.\n", hrt.ElapsedMicroseconds);
            Console.Write(" ================================================================================================\n");
            Console.Write("\n\n  Logs Stored in Repository:------->Requirement #7\n");
            clnt.download("logfile.txt");
            Console.Write("\n  Total elapsed time for downloading = {0}", hrt.ElapsedMicroseconds);
            Console.Write("\n\n  Press key to terminate Repository");
            Console.ReadKey();
            Console.Write("\n\n");
            ((IChannel)clnt.channel).Close();
        }
    }
}
