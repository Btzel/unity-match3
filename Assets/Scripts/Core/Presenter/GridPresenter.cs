using Match3.Model;
using Match3.SO;
using Match3.View;
using System.Collections.Generic;
using UnityEngine;

namespace Match3.Presenter
{
    public class GridPresenter : MonoBehaviour
    {
        private LogicalGrid logicalGrid;
        private VisualGrid visualGrid;

        public void InitializeGrid(VisualGrid visualGrid,Vector2Int gridSize,Vector2 gridStartPos, Vector2 gridEndPos, FruitDataSO[] fruits)
        {
            this.visualGrid = visualGrid;
            logicalGrid = new LogicalGrid();
            logicalGrid.InitializeGrid(gridSize.x, gridSize.y, fruits);
            visualGrid.InitializeGrid(logicalGrid, gridStartPos, gridEndPos);
        }

        // Get Visual and Logical Tiles
        public Tile GetTileAt(int x, int y)
        {
            return logicalGrid.GetTile(x, y);
        }

        public TileView GetTileViewAt(int x, int y)
        {
            return visualGrid.GetTileView(x, y);
        }

        // get Tile Neighbors
        public List<Tile> GetNeighborsAt(Tile tile)
        {
            return logicalGrid.GetNeighbors(tile);
        }

        // VisualGrid Calculations
        public float CalculateTileSize(int gridWidth,int gridHeight,Vector2 startPos,Vector2 endPos)
        {
            float gridRealWidth = Mathf.Abs(endPos.x - startPos.x);
            float gridRealHeight = Mathf.Abs(endPos.y - startPos.y);
            float tileSizeX = gridRealWidth / gridWidth;
            float tileSizeY = gridRealHeight / gridHeight;

            return Mathf.Min(tileSizeX, tileSizeY); ;
        }

        public Vector2 CalculateStartPos(int gridWidth, int gridHeight, Vector2 startPos, Vector2 endPos, float tileSize)
        {
            Vector2 gridCenter = new Vector2((startPos.x + endPos.x) / 2f, (startPos.y + endPos.y) / 2f);

            float startX = gridCenter.x - (gridWidth * tileSize) / 2f;
            float startY = gridCenter.y - (gridHeight * tileSize) / 2f;

            return new Vector2(startX, startY);

        }

        public Vector2 CalculateTileViewPos(Vector2Int gridPos,Vector2 startPos, float tileSize)
        {
            float posX = startPos.x + gridPos.x * tileSize + tileSize / 2f;
            float posY = startPos.y + gridPos.y * tileSize + tileSize / 2f;

            return new Vector2(posX, posY);
        }
    }
}
