/////////////////////////////////////////////////////////////////////
//                                                                 //
// Author: Sahil Shah                                              //
// TestCode1, CSE681 - Software Modeling and Analysis, FALL,2016   //
// 04/10/2016                                                      //
/////////////////////////////////////////////////////////////////////

/*This is the test case that will go through the testing phase in th AppDomain using test driver
 * Input of the test cases is using Dll's
 * Simple test cases that can handle excptions.
 * This is a Division of two numbers test case
 * ------------------------------------
 * PUBLIC:- 
 * TestCode1
 */

using System;

namespace TestH
{
    public class TestCode1
    {
        public bool Division(string messg)
        {
            int number1 = 300;
            int number2 = 30;
            int ans;
            try
            {
                ans = number1 / number2;
                if (ans == 10)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            catch (DivideByZeroException) //exception Handling
            {
                Console.WriteLine("Division of {0} by zero.", number1);

                return false;
            }
            catch (System.InvalidCastException e)
            {
                Console.WriteLine("Cannot convert int to long {0}", e);
                return false;
            }
        }
        static void Main(string[] args)  //TestCase1 stub.
        {
            TestCode1 newCode1 = new TestCode1();
            bool testresult = newCode1.Division("test stub");
            if (testresult == true)
            {
                Console.WriteLine("Test Case has Passed");
            }
            else
            {
                Console.WriteLine("Test Case has Failed");
            }
        }
    }
}
