/////////////////////////////////////////////////////////////////////////
//                                                                    //
// Author: Sahil Shah                                                //
// TestDriver2, CSE681 - Software Modeling and Analysis, FALL,2016  //
// 04/10/2016                                                      //
/////////////////////////////////////////////////////////////////////

/*This is the test Driver that will are complied in seperate Dll's
 * Input of the test cases is using Dll's
 * The Dll of all thed drivers are stored at a specific location
 -----------------------------------
 *  PUBLIC:- 
 *  TestDriver2
 *  -----------------------
 * INTERFACE
 * ITest
 */

using System;


namespace TestH
{
    public class TestDriver2 : ITest
    {
        private TestCode2 code;  // will be compiled into separate DLL

        public TestDriver2()
        {
            code = new TestCode2();
        }

        public static ITest create()
        {
            return new TestDriver2();
        }

        public bool test()
        {
            code.Subtract("Testing the Second Test");   //Test method where all the testing takes place
            TestCode2 newTestCode2 = new TestCode2();
            bool i = newTestCode2.Subtract("Testing......");
            if (i == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        static void Main(string[] args)            // test stub - not run in test harness
        {
            Console.Write("\n  Local test:\n");
            ITest test = TestDriver2.create();
            if (test.test() == true)
                Console.Write("\n  test passed");
            else
                Console.Write("\n  test failed");
            Console.Write("\n\n");
        }
    }
}
