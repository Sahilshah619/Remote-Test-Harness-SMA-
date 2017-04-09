///////////////////////////////////////////////////////////////////////
// MainWindow.xaml.cs.cs - WPF Application, Client as GUI            //
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
 *Partial Class MainWindow
 * 
 */
using CommChannelDemo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows;
using Utilities;
using GUIHarnessTest;
using CSE681SMA;
using System.ServiceModel;

namespace GUI_Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Comm<MainWindow> comm { get; set; }
        public string endPoint { get; }
        private Thread rcvThread = null;
        public List<String> fileList;
        public List<String> fileList1;
        public FileInfo[] MyCollection { get; set; }
        public FileInfo[] MyCollection1 { get; set; }
        IStreamService channel;
        string ToSendPath;
        string SavePath;
        int BlockSize = 1024;
        byte[] block;
        HRTimer.HiResTimer hrt = null;

        public MainWindow()
        { //---<Creates the dropdown list for combobox and channel for message passing>--------
            InitializeComponent();
            var di = new DirectoryInfo(@"..\..\..\GUI-Client\Test's");
            var dir = new DirectoryInfo(@"..\..\..\GUI-Client\Only Dll's");
            MyCollection = di.GetFiles();
            //  MyCollection1 = dir.GetFiles();
            comm = new Comm<MainWindow>();
            endPoint = Comm<MainWindow>.makeEndPoint("http://localhost", 9092);
            fileList = new List<string>();
            fileList1 = new List<string>();
            DataContext = this;
            comm.rcvr.CreateRecvChannel(endPoint);
            rcvThread = comm.rcvr.start(rcvThreadProc);
            ToSendPath = Path.GetFullPath(@"..\..\..\GUI-Client\ToSend");
            SavePath = Path.GetFullPath(@"..\..\..\GUI-Repository\SelectedDll's");
            block = new byte[BlockSize];
            hrt = new HRTimer.HiResTimer();
        }
        public void wait()
        {
            rcvThread.Join();
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
            catch (Exception ex)
            {
                Console.Write("\n  can't find the file\"{0}\"------->Requirement #3", fqname);
            }
        }
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
        void rcvThreadProc()
        { //----< use private service method to receive a message >--------          
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

        public string GUIRequest()
        { //---<Creates message from user Inputs>------
            TestElement te1 = new TestElement("Test1");
            te1.addDriver("TestDriver1.dll");
            te1.addCode(textBox1.ToString());
            TestRequest tr = new TestRequest();
            tr.author = textBox.ToString();
            tr.tests.Add(te1);
            return tr.ToXml();
        }

        //----< construct a basic message >------------------------------
        public CommChannelDemo.Message makeMessage(string author, string fromEndPoint, string toEndPoint)
        {
            CommChannelDemo.Message msg = new CommChannelDemo.Message();
            msg.author = author;
            msg.from = fromEndPoint;
            msg.to = toEndPoint;
            return msg;
        }
        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (textBox.Text.Trim() != "" && comboBox1.Text.Trim() != "")
            {
                textBox1.Text += comboBox.SelectedItem.ToString() + "\n";
                fileList.Add(comboBox.SelectedItem.ToString());
                textBox1.Text += comboBox1.SelectedItem.ToString() + "\n";
                channel = CreateServiceChannel("http://localhost:9002/StreamService");
                Console.Write("\n File Streaming Without Queue------>Requirement #6\n");
                hrt.Start();
                uploadFile(comboBox.Text.ToString());
                uploadFile(comboBox1.Text.ToString());
                hrt.Stop();
                channel = CreateServiceChannel("http://localhost:9093/StreamService");
                Console.Write("\n File Streaming Without Queue------>Requirement #6\n");
                hrt.Start();
                uploadFile(comboBox.Text.ToString());
                uploadFile(comboBox1.Text.ToString());
                hrt.Stop();
            }
            else
            {
                MessageBox.Show("Please provide test Author name and files to test.");
                return;
            }
        }
        //----< On clicking send the xml message to the test harness >------------------------------
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            ThreadStart ts = new ThreadStart(() =>
            {
                Console.Write("\n Client Sending Message to Test Harness");
                Console.Write("\n =====================\n");
                CommChannelDemo.Message msg = makeMessage(textBox.ToString(), endPoint, endPoint);
                string remoteEndPoint = Comm<MainWindow>.makeEndPoint("http://localhost", 9090);
                msg.body = GUIRequest();
                msg.to = remoteEndPoint;
                comm.sndr.PostMessage(msg);

            });
            new Thread(ts).Start();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            TextReader reader = new StreamReader(@"..\..\..\service\DLL\logfile.txt");
            textBox3.Text = reader.ReadToEnd();
            reader.Close();
        }

        private void comboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (comboBox.SelectedIndex == 0)
            {
                comboBox1.Items.Add("TestDriver1.dll");
            }

            else if (comboBox.SelectedIndex == 1)
            {
                comboBox1.Items.Clear();
                comboBox1.Items.Add("TestDriver2.dll");
            }

            else if (comboBox.SelectedIndex == 2)
            {
                comboBox1.Items.Clear();
                comboBox1.Items.Add("TestDriver3.dll");
            }

            else if (comboBox.SelectedIndex == 3)
            {
                comboBox1.Items.Clear();
                comboBox1.Items.Add("TestDriver4.dll");
            }
        }


    }
}
