# Remote-Test-Harness-SMA-
Developed an implementation for accessing and using a remote Test Harness server from multiple concurrent clients. The Test Harness retrieves test code from a Repository server.
                                                       SOFTWARE MODELING AND ANALYSIS (SMA)
                                                            
                                                              PROJECT #4- C# coding

                                                               REMOTE TEST HARNESS
 
SAHIL SHAH
FALL-2016
SUID:- 654427435


I have two clients, one is a console client and GUI client, using WPF.
How to use the GUI:-

1)First the user inputs the name which beomes the author name.
2)Then the user selects the test code to test and by conditional code, the user can only select a relevant test driver in the next combobox.
3)The button "Send Message" button send the message to the test harness, marking the beginning of the test.
4)The button "Select Test" button chooses the test to be sent to test harness and the dll files to the repository.
5)The "Get Logs" button retrieve the test logs that were performed.

Requirements put forward in the project:-

1)Implement a Test Harness Program that accepts one or more Test Requests, each in the form of an XML file that specifies the test developer's 
identity and the names of a set of one or more test drivers with the code to be tested. Each test driver and the code it will be testing is 
implemented as a dynamic link library (DLL). The XML file names one or more of these DLLs to execute. 

2)The Test Harness enqueues Test Requests in a Blocking Queue and execute them serially in dequeued order.

3)Each test driver derives from an ITest interface that declares a method test() that takes no arguments and returns the test pass status, 
 a boolean true or false value. Some interface also declares a getLog() function that returns a string representation of the log.

4)Test execution for each Test Request run in an AppDomain is isolated test processing from Test Harness processing.
 Because we use a child AppDomain to run test executions, an unhandled exception in the test execution will not affect Test Harness processing. 

5)Test libraries and Test Requests are sent to the Repository and Test Harness server, respectively, and results sent back to a requesting client, 
using an asynchronous message-passing communication channel. The Test Harness receives test libraries from the Repository using the same
communication processing. 

6)The Test Harness stores test results and logs for each of the test executions using a key that combines the test developer identity and 
the current date-time.

7)Test logs are stored3 by the Test Request's AppDomain and may be retrieved using the getLog() function.

8) A seperate repository that holds all the Dll files and no usage of absolute path.

9)Use of stubs in all possible packages.

10)If a Test Request specifies test DLLs not available from the Repository, the Test Harness server will send back an error message to the client.

11)Tests are not relied on each other.

12)File transfer shall use streams or a chunking file transfer that does not depend on enqueuing messages.

13)At the end of test execution the Test Harness shall store the test results and logs in the Repository and send test results to the requesting client.

14)The Repository supports client queries about Test Results from the Repository storage.

15)Include means to time test executions and communication latency.

All the requirements have been mentioned on the output windows.

Not Implemented from Project #3, the OCD are:-

1)There is no usage of MongoDB.

3)If an error occurs the process will continue and not crash, but no way to handle the error such that the test case can go back to the exceution phase,
unless re-run of tests with proper inputs.

3)No service level agreement.

Thank you.

