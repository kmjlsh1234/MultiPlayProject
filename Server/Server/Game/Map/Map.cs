using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game
{
    public class Cell
    {
        public HashSet<int> GameObjects = new HashSet<int>(); 
    }

    public class Map
    {
        public GameRoom room { get; set; }

        private readonly float cellSize;
        private readonly float minX;
        private readonly float minY;
        private readonly int width;
        private readonly int height;
        private readonly Cell[,] cells;

        public Map(float cellSize, float minX, float minY, float maxX , float maxY)
        {
            this.cellSize = cellSize;
            this.minX = minX;
            this.minY = minY;

            this.width = (int)Math.Ceiling((maxX - minX) / cellSize);
            this.height = (int)Math.Ceiling((maxY - minY) / cellSize);

            cells = new Cell[width, height];
            for(int x = 0; x< width; x++)
            {
                for(int y = 0; y< height; y++)
                {
                    cells[x,y] = new Cell();
                }
            }
        }

        public void Update()
        {
            //TODO : 
        }

        public Cellinfo WorldToCell(Positioninfo pos)
        {
            Cellinfo cellPos = new Cellinfo();
            cellPos.X = (int)Math.Floor((pos.PosX - minX) / cellSize);
            cellPos.Y = 0; 
            cellPos.Z = (int)Math.Floor((pos.PosZ - minY) / cellSize);
            return cellPos;
        }

    }
}
