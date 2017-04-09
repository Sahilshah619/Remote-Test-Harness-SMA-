@echo off
cd "client1\bin\Debug"
start Channel_Client.exe
cd "..\..\..\ReposToMain\bin\Debug"
start ReposToMain.exe
cd "..\..\..\Server\bin\Debug"
start Server.exe
cd "..\..\..\GUI-Client\bin\Debug"
start GUI-Client.exe
cd "..\..\..\GUIHarnessTest\bin\Debug"
start GUIHarnessTest.exe
cd "..\..\..\GUI-Repository\bin\Debug"
start GUI-Repository.exe
cd "..\..\..\Service\bin\Debug"
start service.exe

