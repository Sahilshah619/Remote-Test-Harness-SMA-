//////////////////////////////////////////////////////////////////////////
// Source: Dr.Jim Fawcett                                              //
// Author: Sahil Shah                                                 //
// LoadAndExecute, CSE681 - Software Modeling and Analysis, FALL,2016//
// 04/10/2016                                                       //
/////////////////////////////////////////////////////////////////////
/*
 * Central procedure of the Project, Loading and run functionalities are created that are implemented in the AppDomain
 * Error handling, even loading error will not let the system crash.
 * ----------------------------
 * PUBLIC classes:-
 *  TestHarness
 *  ----------------------------
 * Private struct or classes:-
 * TestData
 */

using System;
using System.Collections.Generic;
using System.Reflection;
using TestH;

namespace TestH
{
    public class TestHarness : MarshalByRefObject     //Needed in AppDomain to avoid serialization error.
    {
        private struct TestData
        {
            public string Name;
            public ITest testDriver;
        }

        private List<TestData> testDriver = new List<TestData>();

        public TestHarness()
        {

        }

        public bool LoadTests(string path)    //Load the Dll files for execution in AppDomain.
        {
            try
            {
                string[] files = System.IO.Directory.GetFiles(path, "*.dll");

                foreach (string file in files)
                {
                    Console.Write("\n  loading: \"{0}\"", file);

                    Assembly assem = Assembly.LoadFrom(file);
                    Type[] types = assem.GetExportedTypes();

                    foreach (Type t in types)
                    {
                        if (t.IsClass && typeof(ITest).IsAssignableFrom(t))
                        {
                            ITest tdr = (ITest)Activator.CreateInstance(t);    // creates instance of test driver

                            // save type name and reference to created type on managed heap

                            TestData td = new TestData();
                            td.Name = t.Name;
                            td.testDriver = tdr;
                            testDriver.Add(td);
                        }
                    }
                }
                Console.Write("\n");
            }
            catch (Exception ex)
            {
                Console.Write("\n\n  {0}\n\n", ex.Message);
                return false;
            }
            return testDriver.Count > 0;   // Load succeeds if there are items in the list 
        }

        public void run()
        {
            ConsoleLogger x = new ConsoleLogger();
            if (testDriver.Count == 0)
                return;
            foreach (TestData td in testDriver)  // enumerate the test list
            {
                Console.Write("\n  testing {0}", td.Name);
                if (td.testDriver.test() == true)
                {
                    x.add(td.testDriver + " \n" + td.Name + "\n " + "Test1pass\n\n");
                    Console.Write("\n  test passed");  //Running the tests on the test codes that were loaded.
                }
                else
                {
                    x.add("test failed\n\n");
                    Console.Write("\n  test failed");
                }
                x.showAll();
            }
        }
        static void Main(string[] args)
        {
            string path = "../../../TestDll's";
            TestHarness th = new TestHarness();
            if (th.LoadTests(path))
                th.run();
            else
                Console.Write("\n  couldn't load tests");
            Console.Write("\n\n");
        }
    }
}
