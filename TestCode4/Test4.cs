/////////////////////////////////////////////////////////////////////
//                                                                 //
// Author: Sahil Shah                                              //
// TestCode4, CSE681 - Software Modeling and Analysis, FALL,2016   //
// 04/10/2016                                                      //
/////////////////////////////////////////////////////////////////////

/*This is the test case that will go through the testing phase in th AppDomain using test driver
 * Input of the test cases is using Dll's
 * Simple test cases that can handle excptions.
 * This is a multiplication of two numbers test case
 * ---------------------------
 * PUBLIC:- 
 * TestCode4
 */

using System;


namespace TestH
{
    public class TestCode4
    {
        public bool Multiply(string messsage)
        {
            int nos1 = 20;
            int nos2 = 10;
            int subtract;
            try
            {
                subtract = nos1 * nos2;
                if (subtract == 200)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (System.InvalidCastException ex1)
            {
                Console.WriteLine("Cannot convert int to long or float or... {0}", ex1);
                return false;
            }
        }
        static void Main(string[] args)
        {
            TestCode4 newCode4 = new TestCode4();
            bool testresult = newCode4.Multiply("test stub");
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