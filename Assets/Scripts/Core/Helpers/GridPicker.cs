using Match3.SO;
using UnityEngine;

namespace Match3.Helpers
{
    public class GridPicker
    {
        public static GridDataSO GetGrid(Vector2Int gridPos,Vector2Int gridSize, GridDataSO[] grids)
        {
            if (gridPos.x == 0 && gridPos.y == gridSize.y - 1) { return grids[0]; } // Top Left Grid
            else if (gridPos.x == 0 && gridPos.y != 0 && gridPos.y != gridSize.y - 1) { return grids[1]; } // Left Grid
            else if (gridPos.x == 0 && gridPos.y == 0) { return grids[2]; } // Bottom Left Grid
            else if (gridPos.x != 0 && gridPos.x != gridSize.x - 1 && gridPos.y == 0) { return grids[3]; } // Bottom Grid
            else if (gridPos.x == gridSize.x - 1 && gridPos.y == 0) { return grids[4]; } // Bottom Right Grid
            else if (gridPos.x == gridSize.x - 1 && gridPos.y != 0 && gridPos.y != gridSize.y - 1) { return grids[5]; } // Right Grid
            else if (gridPos.x == gridSize.x - 1 && gridPos.y == gridSize.y - 1) { return grids[6]; } // Top Right Grid
            else if (gridPos.x != 0 && gridPos.x != gridSize.x - 1 && gridPos.y == gridSize.y - 1) { return grids[7]; } // Top Grid
            else { return grids[8]; } // Center Grid
        }
    }
}
