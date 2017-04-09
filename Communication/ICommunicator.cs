/////////////////////////////////////////////////////////////////////
// ICommunicator.cs - Peer-To-Peer Communicator Service Contract   //
//                                                                 //
//Source: Jim Fawcett                                              //
//Author: Sahil Shah                                               //
/////////////////////////////////////////////////////////////////////
/*
 * Maintenance History:
 * ====================
 * ver 2.0 : 10 Oct 11
 * - removed [OperationContract] from GetMessage() so only local client
 *   can dequeue messages
 */

using System;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace CommChannelDemo
{
  [ServiceContract]
  public interface ICommunicator
  {
    [OperationContract(IsOneWay = true)]
    void PostMessage(Message msg);

    // used only locally so not exposed as service method
    Message GetMessage();
  }

  // The class Message is defined in CommChannelDemo.Messages as [Serializable]
  // and that appears to be equivalent to defining a similar [DataContract]

}
