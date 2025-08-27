protoc.exe -I=./ --csharp_out=./ Protocol.proto
IF ERRORLEVEL 1 PAUSE

START ../../../PacketGenerator/bin/Debug/net8.0/PacketGenerator.exe ./Protocol.proto

XCOPY /Y Protocol.cs "../../../Server/Packet"
XCOPY /Y Protocol.cs "../../../UnityClient/Assets/Scripts/Network/Packet"

XCOPY /Y ServerPacketManager.cs "../../../Server/Packet"
XCOPY /Y ClientPacketManager.cs "../../../UnityClient/Assets/Scripts/Network/Packet"