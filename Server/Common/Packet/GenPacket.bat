START ../../PacketGenerator/bin/Debug/net8.0/PacketGenerator.exe ../../PacketGenerator/PDL.xml

XCOPY /Y GenPackets.cs "../../Server/Packet"
XCOPY /Y GenPackets.cs "../../TestClient/Packet"

XCOPY /Y ServerPacketManager.cs "../../Server/Packet"
XCOPY /Y ClientPacketManager.cs "../../TestClient/Packet"