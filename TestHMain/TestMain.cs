/////////////////////////////////////////////////////////////////////////
//                                                                    //
// Author: Sahil Shah                                                //
// Main-Function, CSE681 - Software Modeling and Analysis, FALL,2016//
// 04/10/2016                                                      //
////////////////////////////////////////////////////////////////////

/*This is the main function where all objects are created and printed to the output window.
 * Here we convert the FileStream documnet to a string to parse it in the AppDomain
 * Logger fuction is called to log the file and the console window.
 * Enqueue process of the test cases is done here.
 * Child domain creation
 * ------------------------
 * PUBLIC classes:-
 * TestMain
 */

using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace TestH
{
    public class TestMain
    {
        //  public static Queueing newQueue;              //creating queue object 
        //  public static XmlTest demo = new XmlTest();
        static void Main(string[] args)
        {
            //Converting FileStream to the string to be able to  enqueue the test requests.
            try
            {
                Console.Write("\n =====================================================================================\n");
                Console.WriteLine(" Test Requests Queued!");
                Console.Write(" =======================================================================================\n");
                string path = "../../TestRequest.xml";
                string path1 = "../../TestRequest1.xml";
                System.IO.FileStream xml = new System.IO.FileStream(path, System.IO.FileMode.Open);
                XDocument xmldoc = XDocument.Load(xml);
                System.IO.FileStream xml1 = new System.IO.FileStream(path1, System.IO.FileMode.Open);
                XDocument xmldoc1 = XDocument.Load(xml1);
                //    newQueue = new Queueing();
                //    newQueue.enQ(xmldoc.ToString());    //Converting to string
                //   newQueue.enQ(xmldoc1.ToString());
                Console.WriteLine("==================");
                Console.WriteLine("\nTest Request #1");
                Console.WriteLine("==================");
                Console.WriteLine("\n{0}", xmldoc.ToString());
                Console.WriteLine("==================");
                Console.WriteLine("\nTest Request #2");
                Console.WriteLine("==================");
                Console.WriteLine("\n{0}", xmldoc1.ToString());
            }
            catch (Exception ex)
            {
                Console.Write("\n\n  {0}", ex.Message);
            }
            TestH.Creator newDomain = new TestH.Creator(); 
            newDomain.CreateChildDomain(/*newQueue*/);
            Console.ReadKey();
        }
    }
}
