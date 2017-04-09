/////////////////////////////////////////////////////////////////////
//                                                                 //
// Author: Sahil Shah                                              //
// TestCode3, CSE681 - Software Modeling and Analysis, FALL,2016   //
// 04/10/2016                                                      //
/////////////////////////////////////////////////////////////////////

/*This is the test case that will go through the testing phase in th AppDomain using test driver
 * Input of the test cases is using Dll's
 * Simple test cases that can handle excptions.
 * This is a Addition of two numbers test case
 * ---------------------------------------
 * PUBLIC:- 
 * TestCode3
 */

using System;


namespace TestH
{
    public class TestCode3
    {
        public bool Add(string messag)
        {
            int no1 = 20;
            int no2 = 10;
            int Add;
            try
            {
                Add = no1 + no2;
                if (Add == 30)
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
            TestCode3 newCode3 = new TestCode3();
            bool testresult = newCode3.Add("test stub");
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