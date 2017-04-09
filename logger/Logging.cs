/////////////////////////////////////////////////////////////////////
// Source: Dr.Jim Fawcett                                          //
// Author: Sahil Shah                                              //
// Logger, CSE681 - Software Modeling and Analysis, FALL,2016   //
// 04/10/2016                                                      //
/////////////////////////////////////////////////////////////////////
/*
 * Logging function created here that can be called and appended 
 * in the file as well as the console window using showAll() or ShowCurrentItem()
 * Created a method to append or create a file based on the presence of the .txt file.
 * --------------------
 * INTERFACE:-
 * ILogger
 * ---------------------
 * PUBLIC classses:- 
 * CosoleLogger
 */

using System;
using System.Text;
using System.IO;

namespace TestH
{
    public interface ILogger
    {
        ILogger add(string logitem);
        void showCurrentItem();
        void showAll();
    }

    public class ConsoleLogger : ILogger
    {
        StringBuilder logtext = new StringBuilder();
        string currentItem = null;
        string newFile = @"c";   //Giving location to create log file

        public ConsoleLogger()    //function to print log on the console window.
        {
            string time = DateTime.Now.ToString();     //Prints date and time when function is called.
            string title = "\n\n  Current Log: " + time;
            logtext = new StringBuilder(title);
            logtext.Append("\n " + new string('=', title.Length));
        }
        public ILogger add(string logitem)
        {
            currentItem = logitem;
            logtext.Append("\n" + logitem);
            if (!File.Exists(newFile))     //Checks the availiblity of the logFile, if not it creates one
            {
                FileStream newFileStream = File.Create(newFile);
                newFileStream.Close();
            }
            File.AppendAllText(newFile, "\n" + logitem + Environment.NewLine);  //appends to the file.
            return this;
        }

        public void showCurrentItem()
        {
            StreamWriter wr = new StreamWriter(@"..\..\logFile.txt");
            //wr.WriteLine(currentItem);
            Console.Write("\n" + currentItem);
        }

        public void showAll()
        {
            System.IO.StreamWriter wr = new System.IO.StreamWriter(@"..\..\..\Service\DLL\logfile.txt");
            wr.WriteLine(logtext);
            wr.Close();
            Console.Write(logtext + "\n");
        }

        static void Main(string[] args)   //Stub that writes to the File and Console window
        {
            ILogger i = new ConsoleLogger();
            i.add("Logging Check......");
            i.showAll();
            Console.WriteLine("\nLog also printed in Log file in the project folder!\n");
        }
    }
}
