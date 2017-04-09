///////////////////////////////////////////////////////////////////////
//                                                                   //
// Author: Sahil Shah                                               //
// TestDriver1, CSE681 - Software Modeling and Analysis, FALL,2016  //
// 04/10/2016                                                      //
/////////////////////////////////////////////////////////////////////

/*This is the test Driver that will are complied in seperate Dll's
 * Input of the test cases is using Dll's
 * The Dll of all thed drivers are stored at a specific location
 * -----------------------------
 * PUBLIC:- 
 * TestDriver1
 * -------------------------
 * INTERFACE- ITest
 */

using System;
using TestH;

namespace TestH
{
    public class TestDriver1 : ITest
    {
        private TestCode1 code; // will be compiled into separate DLL


        public TestDriver1()
        {
            code = new TestCode1();
        }


        public static ITest create()
        {
            return new TestDriver1();
        }

        public bool test()
        {
            ConsoleLogger x = new ConsoleLogger();
            code.Division("Testing the First Test");  //Test method where all the testing takes place 
            TestCode1 newTestCode1 = new TestCode1();
            bool k = newTestCode1.Division("Testing......");
            if (k == true)
            {
                x.add("Test passed\n\n\n\n");
                x.showAll();
                return true;
            }
            else
            {
                x.add("test passed \n\n\n");
                x.showAll();
                return false;
            }

        }


        static void Main(string[] args)  //Tets stub for the particular Driver
        {
            Console.Write("\n  Local test:\n");

            ITest test = TestDriver1.create();

            if (test.test() == true)
                Console.Write("\n  test passed");
            else
                Console.Write("\n  test failed");
            Console.Write("\n\n");
        }
    }
}

