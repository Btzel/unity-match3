using Match3.SO;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace Match3.Model
{
    public class LogicalGrid
    {
        public event Action OnTilesShifted;
        public event Action<List<Tile>> OnColumnShifted;

        public int GridWidth { get; private set; }
        public int GridHeight { get; private set; }
        public FruitDataSO[] Fruits;
        public Tile[,] Tiles;

        public void InitializeGrid(int gridWidth, int gridHeight, FruitDataSO[] fruits)
        {
            GridWidth = gridWidth;
            GridHeight = gridHeight;
            Fruits = fruits;
            Tiles = new Tile[GridWidth, GridHeight];

            for(int x = 0; x < GridWidth; x++)
            {
                for(int y = 0; y < GridHeight; y++)
                {
                    FruitDataSO randomFruit = Fruits[UnityEngine.Random.Range(0, Fruits.Length)];
                    CreateTile(x, y, false, randomFruit);
                }
            }
        }

        private void CreateTile(int x, int y, bool isSelected, FruitDataSO fruit)
        {
            Tiles[x, y] = new Tile(x, y, isSelected, fruit);
        }

        public Tile GetTile(int x, int y)
        {
            if (Tiles[x,y] != null)
            {
                return Tiles[x, y];
            }
            else
            {
                return null;
            }
        }

        public List<Tile> GetNeighbors(Tile tile)
        {
            int[,] directions = new int[,]
            {
                {-1,-1 },
                {-1, 0 },
                {-1, 1 },
                { 0,-1 },
                { 0, 1 },
                { 1,-1 },
                { 1, 0 },
                { 1, 1 }
            };

            List<Tile> neighbors = new List<Tile>();

            for(int i = 0; i < directions.GetLength(0); i++)
            {
                int newX = tile.PositionX + directions[i, 0];
                int newY = tile.PositionY + directions[i, 1];

                if (newX >= 0 && newY >= 0 && newX < GridWidth && newY < GridHeight)
                {
                    Tile neighbor = GetTile(newX, newY);
                    if (neighbor != null)
                    {
                        neighbors.Add(neighbor);
                    }
                }
            }

            return neighbors;

        }

        public void ShiftSelectedTilesUp()
        {
            for (int x = 0; x < GridWidth; x++)
            {
                List<Tile> columnTiles = new List<Tile>();
                for (int y = 0; y < GridHeight; y++)
                {
                    columnTiles.Add(Tiles[x, y]);
                }
                columnTiles.Sort((a, b) => a.IsSelected.CompareTo(b.IsSelected));
                OnColumnShifted?.Invoke(columnTiles);
                for (int y = 0; y < GridHeight; y++)
                {
                    Tiles[x, y] = columnTiles[y];
                    Tiles[x, y].SetPosition(x, y);
                }

            }
            OnTilesShifted?.Invoke();
        }
    }
}