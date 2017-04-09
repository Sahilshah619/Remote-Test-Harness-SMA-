///////////////////////////////////////////////////////////////////////////
// IStreamService.cs - WCF StreamService in Self Hosted Configuration    //
//                                                                       //
// Source: Jim Fawcett,                                                  //
//Author: Sahil Shah                                                     //
///////////////////////////////////////////////////////////////////////////

using System;
using System.IO;
using System.ServiceModel;

namespace CSE681SMA
{
    [ServiceContract(Namespace = "http://CSE681")]
    public interface IStreamService
    {
        [OperationContract(IsOneWay = true)]
        void upLoadFile(FileTransferMessage msg);
        [OperationContract]
        Stream downLoadFile(string filename);
    }

    [MessageContract]
    public class FileTransferMessage
    {
        [MessageHeader(MustUnderstand = true)]
        public string filename { get; set; }

        [MessageBodyMember(Order = 1)]
        public Stream transferStream { get; set; }
    }
}