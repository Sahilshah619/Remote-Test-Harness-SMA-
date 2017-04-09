/////////////////////////////////////////////////////////////////////
// MessageTest.cs - defines specialized communication messages     //
//                                                                 //
//  Source: Jim Fawcett                                            \
//  Author: Sahil Shah
/////////////////////////////////////////////////////////////////////
/*
 * Package Operations:
 * -------------------
 * Messages provides helper code for building and parsing XML messages.
 *
 * Required files:
 * ---------------
 * - Messages.cs
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Utilities;

namespace CommChannelDemo
{
    //----<Parses the XML that is generated to be sent as a request>--------
    public class TestElement
    {
        public string testName { get; set; }
        public string testDriver { get; set; }
        public List<string> testCodes { get; set; } = new List<string>();
        public TestElement() { }
        public TestElement(string name)
        {
            testName = name;
        }
        public void addDriver(string name)
        {
            testDriver = name;
        }
        public void addCode(string name)
        {
            testCodes.Add(name);
        }

        //----<Converts each XML to string, to send over the channelt>--------
        public override string ToString()
        {
            string te = "\ntestName:\t" + testName;
            te += "\ntestDriver:\t" + testDriver;
            foreach (string code in testCodes)
            {
                te += "\ntestCode:\t" + code;
            }
            return te += "\n";
        }
    }
    public class TestRequest
    {
        public TestRequest() { }
        public string author { get; set; }
        public List<TestElement> tests { get; set; } = new List<TestElement>();

        public override string ToString()
        {
            string tr = "\nAuthor:\t" + author + "\n";
            foreach (TestElement te in tests)
            {
                tr += te.ToString();
            }
            return tr;
        }
    }
    //-----<Default Message for all messages sent >-------
    public class MessageTest
    {
        public static string makeTestRequest()
        {
            TestElement te1 = new TestElement("test1");
            te1.addDriver("TestDriver1.dll");
            te1.addCode("DivideTest.dll");
            TestElement te2 = new TestElement("test2");
            te2.addCode("TestDriver2");
            te2.addCode("TestCode2.dll");
            TestRequest tr = new TestRequest();
            tr.author = "Sahil Shah";
            tr.tests.Add(te1);
            tr.tests.Add(te2);
            return tr.ToXml();
        }

        public static string errorMessage()
        {
            TestElement te1 = new TestElement("test1");
            te1.addDriver("Cannot Find the file TryToTest.dll---------->Requirement #3");
            TestRequest tr = new TestRequest();
            tr.tests.Add(te1);
            tr.author = "Sahil Shah";
            return tr.ToString();
        }
        //-----< Request from client to Test Harness for "Console Client" >-------
        public static string makeTestRequest1()
        {
            TestElement te1 = new TestElement("test1");
            te1.addDriver("TestDriver1.dll");
            te1.addCode("DivideTest.dll");
            TestElement te2 = new TestElement("test2");
            te2.addCode("TestDriver2");
            te2.addCode("TestCode2.dll");
            TestRequest tr = new TestRequest();
            tr.author = "Sahil Shah";
            tr.tests.Add(te1);
            tr.tests.Add(te2);
            return tr.ToXml();
        }
        //-----<Another Request from client to Test Harness for "Console Client" >-------
        public static string makeAnotherTestRequest()
        {
            TestElement te3 = new TestElement("test3");
            te3.addDriver("TestDriver3.dll");
            te3.addCode("TestCode3.dll");
            TestElement te4 = new TestElement("test4");
            te4.addDriver("TestDriver4.dll");
            te4.addCode("TestCode4.dll");
            TestRequest tr = new TestRequest();
            tr.author = "Rishi Dabre";
            tr.tests.Add(te3);
            tr.tests.Add(te4);
            return tr.ToXml();

        }
#if (TEST_MESSAGETEST)
        static void Main(string[] args)
        {
            Message msg = new Message();
            msg.to = "http://localhost:8080/ICommunicator";
            msg.from = "http://localhost:8081/ICommunicator";
            msg.author = "Sahil Shah";
            msg.type = "TestRequest";
            Console.Write("\n  Testing Message with Serialized TestRequest");
            Console.Write("\n ---------------------------------------------\n");
            TestElement te1 = new TestElement("test1");
            te1.addDriver("td1.dll");
            te1.addCode("tc1.dll");
            te1.addCode("tc2.dll");
            Console.Write("\n  Serialized TestRequest:");
            Console.Write("\n -------------------------\n");
            Console.Write(msg.body.shift());
            msg.showMsg();
#endif
        }
    }
}

