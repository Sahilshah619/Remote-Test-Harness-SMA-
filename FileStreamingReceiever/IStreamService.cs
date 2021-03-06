﻿///////////////////////////////////////////////////////////////////////////
// IStreamService.cs - WCF StreamService in Self Hosted Configuration    //
//                                                                       //
// Jim Fawcett, CSE681 - Software Modeling and Analysis, Summer 2009     //
///////////////////////////////////////////////////////////////////////////

using System;
using System.IO;
using System.ServiceModel;

namespace FileStreams
{
  [ServiceContract(Namespace = "http://FileStreams")]
  public interface IStreamService
  {
    [OperationContract(IsOneWay=true)]
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

    [ServiceContract]
    public interface IFileService
    {
        [OperationContract]
        bool OpenFileForWrite(string name);

        [OperationContract]
        bool WriteFileBlock(byte[] block);

        [OperationContract]
        bool CloseFile();
    }
}
