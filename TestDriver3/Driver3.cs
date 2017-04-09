/////////////////////////////////////////////////////////////////////
//                                                                 //
// Author: Sahil Shah                                              //
// TestDriver3, CSE681 - Software Modeling and Analysis, FALL,2016 //
// 04/10/2016                                                      //
/////////////////////////////////////////////////////////////////////

/*This is the test Driver that will are complied in seperate Dll's
 * Input of the test cases is using Dll's
 * The Dll of all thed drivers are stored at a specific location
 * --------------------------------
 *  PUBLIC:- 
 *  TestDriver3
 *  ---------------------
 * INTERFACE- 
 * ITest
 */

using System;


namespace TestH
{
    public class TestDriver3 : ITest
    {
        private TestCode3 code;  // will be compiled into separate DLL



        public TestDriver3()
        {
            code = new TestCode3();
        }


        public static ITest create()
        {
            return new TestDriver3();
        }


        public bool test()
        {
            code.Add("Testing the Third Test");  // test method is where all the testing gets done 
            TestCode3 newTestCode3 = new TestCode3();
            bool l = newTestCode3.Add("Testing......");
            if (l == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        static void Main(string[] args)   //test stub - not run in test harness 
        {
            Console.Write("\n  Local test:\n");

            ITest test = TestDriver3.create();

            if (test.test() == true)
                Console.Write("\n  test passed");
            else
                Console.Write("\n  test failed");
            Console.Write("\n\n");
        }
    }
}
