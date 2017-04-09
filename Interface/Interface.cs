/////////////////////////////////////////////////////////////////////
//                                                                 //
// Author: Sahil Shah                                              //
// Interface, CSE681 - Software Modeling and Analysis, FALL,2016   //
// 04/10/2016                                                      //
/////////////////////////////////////////////////////////////////////

/*Interface contains definations for a group of related functionalaties that canbe used by a class.
 * It is safe to say that Interface solves the problem of multiple inhertance in c#.
 * ------------------
 * INTERFACE - 
 * ITest
 */

using System;
using System.Text;
using System.IO;


//Test drivers derived from the ITest Interface that declares a method test() that takes no arguments and returns bool value, true or false.
namespace TestH
{
    public interface ITest
    {
        bool test();

    }

}

