using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game.Map
{
    public class Map
    {
        public int minX { get; set; }
        public int minY { get; set; }
        public int maxX { get; set; }
        public int maxY { get; set; }

        bool[,] collision;
        GameObject[,] objects;
        public void LoadMap(string mapName)
        {
            string text = File.ReadAllText($"../../../Game/Map/{mapName}.txt");
            StringReader reader = new StringReader(text);

            minX = int.Parse(reader.ReadLine());
            maxX = int.Parse(reader.ReadLine());
            minY = int.Parse(reader.ReadLine());
            maxY = int.Parse(reader.ReadLine());

            int xCount = maxX - minX + 1;
            int yCount = maxY - minY + 1;

            collision = new bool[yCount, xCount];
            objects = new GameObject[yCount, xCount];

            for (int y = 0; y < yCount; y++)
            {
                string line = reader.ReadLine();
                for (int x = 0; x < xCount; x++)
                {
                    collision[y, x] = (line[x] == '1' ? true : false);
                }
            }
        }

        public bool CanGo()
        {
            return true;
        }

    }
}
