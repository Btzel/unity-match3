using Match3.Model;
using Match3.SO;
using Match3.View;
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
    }
}
