using Codice.Client.BaseCommands;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
public class MapEditor
{
    [MenuItem("Tools/Generate Map Data")]
    public static void GenerateMapData()
    {
        GameObject go = GameObject.Find("Map");
        if (go == null)
        {
            Debug.LogWarning("Map 오브젝트를 찾을 수 없습니다!");
            return;
        }

        int minX = -72;
        int maxX = 72;
        int minY = -72;
        int maxY = 72;

        int width = maxX - minX + 1;
        int height = maxY - minY + 1;

        int[,] map = new int[height, width];

        for (int i = minX; i <= maxX; i++)
        {
            for (int j = minY; j <= maxY; j++)
            {
                int x = i - minX; // 배열 인덱스로 변환
                int y = j - minY;

                Vector3 pos = new Vector3(i, 0, j);
                Vector3 halfExtents = new Vector3(0.5f, 0.5f, 0.5f);

                bool hasWall = Physics.CheckBox(
                    pos,
                    halfExtents,
                    Quaternion.identity,
                    LayerMask.GetMask("Wall")
                );

                map[y, x] = hasWall ? 1 : 0;
            }
        }

        SaveMapToTxt(map, minX, maxX, minY, maxY);
    }

    private static void SaveMapToTxt(int[,] map, int minX, int maxX, int minY, int maxY)
    {
        StringBuilder sb = new StringBuilder();

        // 좌표 범위 먼저 기록
        sb.AppendLine(minX.ToString());
        sb.AppendLine(maxX.ToString());
        sb.AppendLine(minY.ToString());
        sb.AppendLine(maxY.ToString());

        int height = map.GetLength(0);
        int width = map.GetLength(1);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                sb.Append(map[y, x]);
            }
            sb.AppendLine();
        }

        string clientPath = Path.Combine(Application.dataPath, "Resources/Data/MapData.txt");
        string serverPath = Path.Combine(Application.dataPath, "../../Server/Game/Map/MapData.txt");

        // 폴더가 없으면 생성
        Directory.CreateDirectory(Path.GetDirectoryName(clientPath));
        Directory.CreateDirectory(Path.GetDirectoryName(serverPath));

        File.WriteAllText(clientPath, sb.ToString());
        File.WriteAllText(serverPath, sb.ToString());

        Debug.Log($"맵 데이터가 저장됨: {clientPath}");
        Debug.Log($"맵 데이터가 저장됨: {serverPath}");
    }
}
