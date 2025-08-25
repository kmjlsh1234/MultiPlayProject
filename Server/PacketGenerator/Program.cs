using System.Data;
using System.Net;
using System.Reflection;
using System.Xml;

namespace PacketGenerator
{
    public class Program
    {
        public static string serverRegister = string.Empty;
        public static string clientRegister = string.Empty;

        static void Main(string[] args)
        {
            string file = "../../../Common/protoc - 3.12.3 - win64/bin/Protocol.proto";
            if(args.Length >= 1)
            {
                file = args[0];
            }

            bool startParsing = false;

            foreach(string line in File.ReadAllLines(file))
            {
                if(!startParsing && line.Contains("enum MsgId"))
                {
                    startParsing = true;
                    continue;
                }

                if (!startParsing)
                {
                    continue;
                }

                string[] names = line.Trim().Split(" =");
                if(names.Length == 0)
                {
                    continue;
                }

                string name = names[0];

                if (name.StartsWith("S_"))
                {
                    string[] words = name.Split("_");

                    string msgName = "";
                    foreach(string word in words)
                    {
                        msgName += FirstCharToUpper(word);
                    }

                    string packetName = $"S_{msgName.Substring(1)}";
                    clientRegister += string.Format(PacketFormat.managerRegisterFormat, msgName, packetName);
                }
                else if (name.StartsWith("C_"))
                {
                    string[] words = name.Split("_");

                    string msgName = "";
                    foreach (string word in words)
                    {
                        msgName += FirstCharToUpper(word);
                    }

                    string packetName = $"C_{msgName.Substring(1)}";
                    serverRegister += string.Format(PacketFormat.managerRegisterFormat, msgName, packetName);
                }
            }

            string clientManagerText = string.Format(PacketFormat.managerFormat, clientRegister);
            File.WriteAllText("ClientPacketManager.cs", clientManagerText);
            string serverManagerText = string.Format(PacketFormat.managerFormat, serverRegister);
            File.WriteAllText("ServerPacketManager.cs", serverManagerText);

        }

        static string FirstCharToUpper(string input)
        {
            return input[0].ToString().ToUpper() + input.Substring(1).ToLower();
        }
        
    }
}
