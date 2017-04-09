/////////////////////////////////////////////////////////////////////
//                                                                 //
// Author: Sahil Shah                                              //
// TestDriver4, CSE681 - Software Modeling and Analysis, FALL,2016 //
// 04/10/2016                                                      //
/////////////////////////////////////////////////////////////////////

/*This is the test Driver that will are complied in seperate Dll's
 * Input of the test cases is using Dll's
 * The Dll of all thed drivers are stored at a specific location
 * ---------------------------
 *  PUBLIC:- 
 *  TestDriver4
 *  -------------------------
 * INTERFACE- 
 * ITest
 */

using System;

namespace TestH
{
    public class TestDriver4 : ITest
    {
        private TestCode4 code;  // will be compiled into separate DLL

        public TestDriver4()
        {
            code = new TestCode4();
        }

        public static ITest create()
        {
            return new TestDriver4();
        }

        public bool test()
        {
            code.Multiply("Testing the Fourth Test");   // test method is where all the testing gets done 
            TestCode4 newTestCode4 = new TestCode4();
            bool z = newTestCode4.Multiply("Testing......");
            if (z == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        static void Main(string[] args)    // test stub - not run in test harness
        {
            Console.Write("\n  Local test:\n");

            ITest test = TestDriver4.create();

            if (test.test() == true)
                Console.Write("\n  test passed");
            else
                Console.Write("\n  test failed");
            Console.Write("\n\n");
        }
    }
}
