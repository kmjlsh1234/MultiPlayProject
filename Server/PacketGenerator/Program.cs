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
        public static string filePathPrefix = "../../../Common/protoc-3.12.3-win64/bin/";
        static void Main(string[] args)
        {
            List<string> files = new List<string>(args);

            foreach (string file in files)
            {
                bool startParsing = false;
                foreach (string line in File.ReadAllLines(file))
                {
                    if (!startParsing && line.Contains("enum MsgId"))
                    {
                        startParsing = true;
                        continue;
                    }

                    if (!startParsing)
                        continue;

                    if (line.Contains("}"))
                        break;

                    string[] names = line.Trim().Split(" =");
                    if (names.Length == 0)
                        continue;

                    string name = names[0];

                    if (name.StartsWith("S_"))
                    {
                        string msgName = RemoveUnderscorePascal(name);
                        string packetName = ToPascalCaseSingleUnderscore(name);

                        clientRegister += string.Format(PacketFormat.managerRegisterFormat, msgName, packetName);
                    }
                    else if (name.StartsWith("C_"))
                    {
                        string msgName = RemoveUnderscorePascal(name);
                        string packetName = ToPascalCaseSingleUnderscore(name);

                        serverRegister += string.Format(PacketFormat.managerRegisterFormat, msgName, packetName);
                    }
                }
            }

            string clientManagerText = string.Format(PacketFormat.managerFormat, clientRegister);
            File.WriteAllText("ClientPacketManager.cs", clientManagerText);
            string serverManagerText = string.Format(PacketFormat.managerFormat, serverRegister);
            File.WriteAllText("ServerPacketManager.cs", serverManagerText);
        }

        // C_EXIT_ROOM → CExitRoom
        static string RemoveUnderscorePascal(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            string[] parts = input.Split('_', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0)
                return input;

            string prefix = parts[0]; // S or C
            string result = prefix;

            for (int i = 1; i < parts.Length; i++)
            {
                string lower = parts[i].ToLower();
                result += char.ToUpper(lower[0]) + lower.Substring(1);
            }

            return result;
        }

        // C_EXIT_ROOM → C_ExitRoom
        static string ToPascalCaseSingleUnderscore(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            string[] parts = input.Split('_', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length <= 1)
                return input;

            string prefix = parts[0]; // S or C

            // 첫 번째 단어만 유지하고, 나머지는 PascalCase로 연결
            string first = char.ToUpper(parts[1].ToLower()[0]) + parts[1].ToLower().Substring(1);
            string rest = string.Concat(parts.Skip(2).Select(p =>
            {
                string lower = p.ToLower();
                return char.ToUpper(lower[0]) + lower.Substring(1);
            }));

            return $"{prefix}_{first}{rest}";
        }
    }
}