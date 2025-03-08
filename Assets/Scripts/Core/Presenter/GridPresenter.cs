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

        public void InitializeGrid(VisualGrid visualGrid,int gridWidth,int gridHeight,Vector2 gridWorldStartPoint, Vector2 gridWorldEndPoint, FruitDataSO[] fruits)
        {
            this.visualGrid = visualGrid;
            logicalGrid = new LogicalGrid();
            logicalGrid.InitializeGrid(gridWidth, gridHeight, fruits);
            visualGrid.InitializeGrid(logicalGrid,gridWorldStartPoint,gridWorldEndPoint);
        }
    }
}
