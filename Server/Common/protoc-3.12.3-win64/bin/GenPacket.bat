protoc.exe -I=./ --csharp_out=./ Protocol_Enum.proto
protoc.exe -I=./ --csharp_out=./ Protocol_Data.proto
protoc.exe -I=./ --csharp_out=./ Protocol_Common.proto
protoc.exe -I=./ --csharp_out=./ Protocol_Lobby.proto
protoc.exe -I=./ --csharp_out=./ Protocol_Match.proto
protoc.exe -I=./ --csharp_out=./ Protocol_Game.proto

START ../../../PacketGenerator/bin/Debug/net8.0/PacketGenerator.exe ^
    Protocol_Enum.proto ^
    Protocol_Data.proto ^
    Protocol_Common.proto ^
    Protocol_Lobby.proto ^
    Protocol_Match.proto ^
    Protocol_Game.proto

XCOPY /Y ProtocolEnum.cs "../../../Server/Packet/Protocol"
XCOPY /Y ProtocolEnum.cs "../../../UnityClient/Assets/Scripts/Network/Packet/Protocol"
XCOPY /Y ProtocolData.cs "../../../Server/Packet/Protocol"
XCOPY /Y ProtocolData.cs "../../../UnityClient/Assets/Scripts/Network/Packet/Protocol"
XCOPY /Y ProtocolCommon.cs "../../../Server/Packet/Protocol"
XCOPY /Y ProtocolCommon.cs "../../../UnityClient/Assets/Scripts/Network/Packet/Protocol"
XCOPY /Y ProtocolLobby.cs "../../../Server/Packet/Protocol"
XCOPY /Y ProtocolLobby.cs "../../../UnityClient/Assets/Scripts/Network/Packet/Protocol"
XCOPY /Y ProtocolMatch.cs "../../../Server/Packet/Protocol"
XCOPY /Y ProtocolMatch.cs "../../../UnityClient/Assets/Scripts/Network/Packet/Protocol"
XCOPY /Y ProtocolGame.cs "../../../Server/Packet/Protocol"
XCOPY /Y ProtocolGame.cs "../../../UnityClient/Assets/Scripts/Network/Packet/Protocol"

XCOPY /Y ServerPacketManager.cs "../../../Server/Packet"
XCOPY /Y ClientPacketManager.cs "../../../UnityClient/Assets/Scripts/Network/Packet"

pause