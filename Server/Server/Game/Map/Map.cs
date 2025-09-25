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
        public HashSet<GameObject> GameObjects = new HashSet<GameObject>(); 
    }

    public class Map
    {
        public GameRoom room { get; set; }

        private readonly float cellSize;
        private readonly int minX;
        private readonly int minY;
        private readonly int width;
        private readonly int height;

        private Dictionary<Cellinfo, Cell> cells = new Dictionary<Cellinfo, Cell>();

        public Map(float cellSize, int minX, int minY, int maxX , int maxY)
        {
            this.cellSize = cellSize;
            this.minX = minX;
            this.minY = minY;

            this.width = (int)Math.Ceiling((maxX - minX) / cellSize);
            this.height = (int)Math.Ceiling((maxY - minY) / cellSize);

            
            for(int x = minX; x< maxX; x++)
            {
                for(int y = minY; y< maxY; y++)
                {
                    Cellinfo key = new Cellinfo() { X = x, Y = y };
                    cells.Add(key, new Cell());
                }
            }
        }

        public void Update()
        {
            //TODO : 
        }

        public Cellinfo WorldToCell(Positioninfo pos)
        {
            Cellinfo cell = new Cellinfo();

            cell.X = (int)Math.Floor(pos.PosX / cellSize);
            cell.Y = (int)Math.Floor(pos.PosZ / cellSize);

            return cell;
        }

        public void UpdateObjectPosition(GameObject go, Cellinfo oldCellPos, Cellinfo newCellPos)
        {
            if (cells.TryGetValue(oldCellPos, out Cell oldCell))
            {
                oldCell.GameObjects.Remove(go);
                Add(go, newCellPos);
            }
        }

        public void Remove(GameObject go)
        {
            Cell cell = GetCell(go.objectinfo.CellInfo);
            if (cell == null)
                return;

            cell.GameObjects.Remove(go);

        }
        public void Add(GameObject go, Cellinfo cellPos)
        {
            Cell cell = GetCell(cellPos);

            if (cell != null)
            {
                cell.GameObjects.Add(go);
            }
        }

        private Cell GetCell(Cellinfo info)
        {
            Cell cell = null;
            
            if (cells.TryGetValue(info, out cell) == false)
            {
                cell = new Cell();
                cells.Add(info, cell);
            }

            return cell;
        }

        // 두 셀 사이의 거리 계산 (맨하탄 거리)
        public int GetCellDistance(Cellinfo cell1, Cellinfo cell2)
        {
            return Math.Abs(cell1.X - cell2.X) + Math.Abs(cell1.Y - cell2.Y);
        }
    }
}
