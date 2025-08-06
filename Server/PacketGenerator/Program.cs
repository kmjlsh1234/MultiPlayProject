using System.Net;
using System.Reflection;
using System.Xml;

namespace PacketGenerator
{
    public class Program
    {
        public static string pdlPath = "../../../PDL.xml";
        
        public static string genPacket = string.Empty;
        public static string packetEnums = string.Empty;
        
        public static string serverRegister = string.Empty;
        public static string clientRegister = string.Empty;

        static void Main(string[] args)
        {
            XmlReaderSettings settings = new XmlReaderSettings()
            {
                IgnoreComments = true,
                IgnoreWhitespace = true
            };
            using(XmlReader r = XmlReader.Create(args[0], settings))
            {
                while (r.Read())
                {
                    ParsePacket(r);
                }
                string result = string.Format(PacketFormat.fileFormat, packetEnums, genPacket); // {0} : 패킷 ID ENUM // {1} : 패킷 클래스 모음
                string clientManager = string.Format(PacketFormat.managerFormat, clientRegister);
                string serverManager = string.Format(PacketFormat.managerFormat, serverRegister);

                File.WriteAllText("GenPackets.cs", result);

                File.WriteAllText("ClientPacketManager.cs", clientManager);
                File.WriteAllText("ServerPacketManager.cs", serverManager);
            }
        }

        static void ParsePacket(XmlReader r)
        {
            int enumCount = 0;
            while (r.Read())
            {

                if(r.NodeType == XmlNodeType.Element && r.Depth == 1)
                {
                    Console.WriteLine($"ParsPacket : {r.Name} : {r["name"]} / Depth : {r.Depth}");
                    Console.WriteLine("");
                    
                    string packetName = r["name"];
                    Tuple<string, string, string> t = ParseMembers(r);

                    // {1} : 멤버 변수들, {2} : Read, {3} : Write
                    genPacket += string.Format(PacketFormat.packetFormat, packetName, t.Item1, t.Item2, t.Item3);
                    packetEnums += string.Format(PacketFormat.packetEnumFormat, packetName, enumCount++);
                    if (packetName.StartsWith("C_"))
                    {
                        serverRegister += string.Format(PacketFormat.managerRegistFormat, packetName);
                    }
                    else
                    {
                        clientRegister += string.Format(PacketFormat.managerRegistFormat, packetName);
                    }
                }

            }
        }

        // {1} : 멤버 변수들
        // {2} : Read
        // {3} : Write
        static Tuple<string, string, string> ParseMembers(XmlReader r)
        {
            string memberCode = string.Empty;
            string readCode = string.Empty;
            string writeCode = string.Empty;

            while (r.Read())
            {
                if(r.NodeType == XmlNodeType.EndElement && r.Depth == 1)
                {
                    break;
                }

                string memberType = r.Name;
                string memberName = r["name"];

                switch(memberType){
                    case "byte":
                        memberCode += string.Format(PacketFormat.memberFormat, memberType, memberName);             // {0} : 변수 타입, {1} : 변수 명
                        readCode += string.Format(PacketFormat.readFormat, memberName, FindTransFun(memberType));   // {0} : 변수 명, {1} : 변환 함수
                        writeCode += string.Format(PacketFormat.writeFormat, memberName, memberType);               // {0} : 변수 명, {1} : 변수 타입
                        break;
                    case "bool":
                        memberCode += string.Format(PacketFormat.memberFormat, memberType, memberName);             // {0} : 변수 타입, {1} : 변수 명
                        readCode += string.Format(PacketFormat.readFormat, memberName, FindTransFun(memberType));   // {0} : 변수 명, {1} : 변환 함수
                        writeCode += string.Format(PacketFormat.writeFormat, memberName, memberType);               // {0} : 변수 명, {1} : 변수 타입
                        break;
                    case "ushort":
                        memberCode += string.Format(PacketFormat.memberFormat, memberType, memberName);             // {0} : 변수 타입, {1} : 변수 명
                        readCode += string.Format(PacketFormat.readFormat, memberName, FindTransFun(memberType));   // {0} : 변수 명, {1} : 변환 함수
                        writeCode += string.Format(PacketFormat.writeFormat, memberName, memberType);               // {0} : 변수 명, {1} : 변수 타입
                        break;
                    case "int":
                        memberCode += string.Format(PacketFormat.memberFormat, memberType, memberName);             // {0} : 변수 타입, {1} : 변수 명
                        readCode += string.Format(PacketFormat.readFormat, memberName, FindTransFun(memberType));   // {0} : 변수 명, {1} : 변환 함수
                        writeCode += string.Format(PacketFormat.writeFormat, memberName, memberType);               // {0} : 변수 명, {1} : 변수 타입
                        break;
                    case "float":
                        memberCode += string.Format(PacketFormat.memberFormat, memberType, memberName);             // {0} : 변수 타입, {1} : 변수 명
                        readCode += string.Format(PacketFormat.readFormat, memberName, FindTransFun(memberType));   // {0} : 변수 명, {1} : 변환 함수
                        writeCode += string.Format(PacketFormat.writeFormat, memberName, memberType);               // {0} : 변수 명, {1} : 변수 타입
                        break;
                    case "long":
                        memberCode += string.Format(PacketFormat.memberFormat, memberType, memberName);             // {0} : 변수 타입, {1} : 변수 명
                        readCode += string.Format(PacketFormat.readFormat, memberName, FindTransFun(memberType));   // {0} : 변수 명, {1} : 변환 함수
                        writeCode += string.Format(PacketFormat.writeFormat, memberName, memberType);               // {0} : 변수 명, {1} : 변수 타입
                        break;
                    case "string":
                        memberCode += string.Format(PacketFormat.memberFormat, memberType, memberName);             // {0} : 변수 타입, {1} : 변수 명
                        readCode += string.Format(PacketFormat.readStringFormat, memberName);                       // {0} : 변수 명
                        writeCode += string.Format(PacketFormat.writeStringFormat, memberName);                     // {0} : 변수 명
                        break;
                    case "list":
                        break;
                }
            }
            return new Tuple<string, string, string>(memberCode, readCode, writeCode);
        }

        static string FindTransFun(string type)
        {
            switch (type)
            {
                case "byte":
                    return null;
                case "bool":
                    return "ToBoolean";
                case "ushort":
                    return "ToUInt16";
                case "int":
                    return "ToInt32";
                case "float":
                    return "ToSingle";
                case "long":
                    return "ToInt64";
                default:
                    return null;
            }
        }
       
    }
}
