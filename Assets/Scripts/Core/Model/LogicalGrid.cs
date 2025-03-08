using Match3.SO;
using UnityEngine;


namespace Match3.Model
{
    public class LogicalGrid
    {
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
                    Tiles[x, y] = new Tile(x, y, false, Fruits[Random.Range(0,Fruits.Length)]);
                }
            }
        }
    }
}