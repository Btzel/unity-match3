using UnityEngine;

namespace Match3.Helpers
{
    public class Calculator
    {
        public static Vector2 CalculateTileSize(int gridWidth, int gridHeight, Vector2 gridWorldStartPoint, Vector2 gridWorldEndPoint)
        {
            float tile_size_x = (gridWorldEndPoint.x - gridWorldStartPoint.x) / (gridWidth);
            float tile_size_y = (gridWorldEndPoint.y - gridWorldStartPoint.y) / (gridWidth);

            return new Vector2(tile_size_x, tile_size_y);
        }

        public static Vector2 CalculateWorldPosition(int x, int y, int gridWidth, int gridHeight, Vector2 gridWorldStartPoint, Vector2 gridWorldEndPoint)
        {
            Vector2 tile_size = CalculateTileSize(gridWidth, gridHeight, gridWorldStartPoint, gridWorldEndPoint);
            float worldPositionX = gridWorldStartPoint.x + x * tile_size.x;
            float worldPositionY = gridWorldStartPoint.y + y * tile_size.y;

            return new Vector2(worldPositionX, worldPositionY);
        }

        public static Vector2 CalculateGridPosition(Vector2 worldPosition, int gridWidth, int gridHeight, Vector2 gridWorldStartPoint, Vector2 gridWorldEndPoint)
        {
            Vector2 tile_size = CalculateTileSize(gridWidth, gridHeight, gridWorldStartPoint, gridWorldEndPoint);

            int x = Mathf.RoundToInt((worldPosition.x - gridWorldStartPoint.x) / tile_size.x);
            int y = Mathf.RoundToInt((worldPosition.y - gridWorldStartPoint.y) / tile_size.y);

            return new Vector2Int(x, y);
        }

        public static Vector3 CalculateTileScale(int gridWidth, int gridHeight, Vector2 gridWorldStartPoint, Vector2 gridWorldEndPoint)
        {
            Vector2 tile_size = CalculateTileSize(gridWidth, gridHeight, gridWorldStartPoint, gridWorldEndPoint);
            return new Vector3(tile_size.x, tile_size.y, 1f); // 2D olduğu için Z ekseni 1
        }
    }
}