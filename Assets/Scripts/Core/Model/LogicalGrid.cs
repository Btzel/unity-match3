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
                    FruitDataSO randomFruit = Fruits[Random.Range(0, Fruits.Length)];
                    CreateTile(x, y, false, randomFruit);
                    Debug.Log(x + " " + y);
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
    }
}