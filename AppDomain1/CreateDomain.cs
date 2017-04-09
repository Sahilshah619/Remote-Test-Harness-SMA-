/////////////////////////////////////////////////////////////////////
//AppDomain- Creates Seperate Domains for each test request        //
//Source: Dr. Jim Fawcett.                                         //
//  Author: Sahil Shah                                             //
// APPDomain, CSE681 - Software Modeling and Analysis, FALL,2016   //
// 04/10/2016                                                      //
/////////////////////////////////////////////////////////////////////

/*
 *Creating seperate child AppDomains for each process from parent AppDomain
 * We show all the assemblis in the child domain
 * parse the xML file also in the AppDomain.
 * Failure of one precoess does not affect the execution other process
 * Stub for the same is created 
 * ---------------
 * PUBLIC Classes:-
 * Creator ,
 * CreateChileDomain
 */

using System;
using System.IO;
using System.Reflection;         // defines Assembly type
using System.Runtime.Remoting;   // provides remote communication between AppDomains
using System.Security.Policy;    // defines evidence needed for AppDomain construction
using System.Xml.Linq;

namespace TestH
{
    public class ApplicationDomain : MarshalByRefObject
    {
        static void showAssemblies(AppDomain ad)   //Shows assemblies loaded in the AppDomain
        {
            Assembly[] arrayOfAssems = ad.GetAssemblies();
            foreach (Assembly assem in arrayOfAssems)
                Console.Write("\n  {0}", assem);
        }
        //Used to set the default loader optimization policy for the main method of an executable application
        [LoaderOptimizationAttribute(LoaderOptimization.MultiDomainHost)]
        public void CreateChildDomain() //Creating child AppDomain
        {
            try
            {
                AppDomainSetup domaininfo = new AppDomainSetup();
                domaininfo.ApplicationBase
                  = "file:///" + System.Environment.CurrentDirectory; //Defining search path 
                Evidence adevidence = AppDomain.CurrentDomain.Evidence;
                AppDomain ad = AppDomain.CreateDomain("ChildDomain", adevidence, domaininfo); //Creating child AppDomain 
                ad.Load("LoadAndExecute");    //Loading Dll Files in the child AppDomain
                showAssemblies(ad);
                Console.Write("\n\n");
                try
                {
                    ObjectHandle oh
                    = ad.CreateInstance("LoadAndExecute", "TestH.TestHarness");
                    object ob = oh.Unwrap();               //Creates a proxy task that creates async operation, creates proxy.
                    Console.Write("\n  {0}", ob);
                    TestH.TestHarness h = (TestH.TestHarness)ob;
                    h.LoadTests(@"../../../GUI-Repository/SelectedDll's");   //Path for location of DLL files.
                    h.run();
                    AppDomain.Unload(ad);     //Unloading AppDomain for memory efficeiency 
                    Console.Write("\n\nUnloading App-Domain------->Requirement #7");
                    Array.ForEach(Directory.GetFiles(@"../../../GUI-Repository/SelectedDll's"), File.Delete);
                }
                catch (Exception except)
                {
                    Console.Write("test  {0}\n\n", except.Message);
                }
            }
            catch (Exception except)
            {
                Console.Write("\n  {0}\n\n", except.Message);
            }
        }
    }
    public class Creator : MarshalByRefObject    //MarshallByRefObjectIt enables access across AppDomain boundaries
    {
        static void showAssemblies(AppDomain ad)   //Shows assemblies loaded in the AppDomain
        {
            Assembly[] arrayOfAssems = ad.GetAssemblies();
            foreach (Assembly assem in arrayOfAssems)
                Console.Write("\n  {0}", assem);
        }
        //Used to set the default loader optimization policy for the main method of an executable application
        [LoaderOptimizationAttribute(LoaderOptimization.MultiDomainHost)]
        public void CreateChildDomain() //Creating child AppDomain
        {
            try
            {
                AppDomainSetup domaininfo = new AppDomainSetup();
                domaininfo.ApplicationBase
                  = "file:///" + System.Environment.CurrentDirectory; //Defining search path 
                Evidence adevidence = AppDomain.CurrentDomain.Evidence;
                AppDomain ad = AppDomain.CreateDomain("ChildDomain", adevidence, domaininfo); //Creating child AppDomain 
                ad.Load("LoadAndExecute");    //Loading Dll Files in the child AppDomain
                showAssemblies(ad);
                Console.Write("\n\n");
                try
                {
                    ObjectHandle oh
                    = ad.CreateInstance("LoadAndExecute", "TestH.TestHarness");
                    object ob = oh.Unwrap();               //Creates a proxy task that creates async operation, creates proxy.
                    Console.Write("\n  {0}", ob);
                    TestH.TestHarness h = (TestH.TestHarness)ob;
                    h.LoadTests(@"../../../Server/Test's");   //Path for location of DLL files.
                    h.run();
                    AppDomain.Unload(ad);     //Unloading AppDomain for memory efficeiency 
                    Console.Write("\n\n");
                }
                catch (Exception except)
                {
                    Console.Write("test  {0}\n\n", except.Message);
                }
            }
            catch (Exception except)
            {
                Console.Write("\n  {0}\n\n", except.Message);
            }
        }
        static void Main(string[] args)
        {
            TestH.Creator newDomain = new TestH.Creator();  //Calling the create AppDomain function using object                                                           
            string path = "../../TestRequest.xml";
            System.IO.FileStream xml = new System.IO.FileStream(path, System.IO.FileMode.Open);
            XDocument xmldoc = XDocument.Load(xml);
            Console.WriteLine("==================");
            Console.WriteLine("\nTest Request #1");
            Console.WriteLine("==================");
            Console.WriteLine("\n{0}", xmldoc.ToString());
            Console.WriteLine("==================");
        }
    }
}



