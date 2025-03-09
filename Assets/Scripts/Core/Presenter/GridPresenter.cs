using Match3.InputH;
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

        private FruitDataSO currentFruitType;
        [SerializeField] private List<TileView> currentTileViews = new List<TileView>();
        [SerializeField] private List<TileView> selectedTileViews = new List<TileView>();

        private InputHandler inputHandler;

        private void Start()
        {
            inputHandler = FindFirstObjectByType<InputHandler>(); // InputHandler'ı buluyoruz.
            inputHandler.OnSelectionStart += HandleSelectionStart;
            inputHandler.OnSelectionContinue += HandleSelectionContinue;
            inputHandler.OnSelectionEnd += HandleSelectionEnd;
        }

        private void OnDestroy()
        {
            inputHandler.OnSelectionStart -= HandleSelectionStart;
            inputHandler.OnSelectionContinue -= HandleSelectionContinue;
            inputHandler.OnSelectionEnd -= HandleSelectionEnd;
        }

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

        public void HandleSelectionStart(Vector2 worldPosition)
        {

            RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);
            if (hit.collider != null && hit.collider.CompareTag("TileView"))
            {

                TileView tileView = hit.collider.GetComponent<TileView>();
                Tile tile = GetTileAt(tileView.GridPositionX, tileView.GridPositionY);
                if (!tile.IsSelected)
                {
                    currentFruitType = tile.Fruit;
                    currentTileViews.Add(tileView);
                }
            }
            
        }
        public void HandleSelectionContinue(Vector2 worldPosition)
        {
            RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);
            if (hit.collider != null && hit.collider.CompareTag("TileView"))
            {

                // check if the adjacent of the selected fruits (on 8 direction)
                TileView tileView = hit.collider.GetComponent<TileView>();
                Tile tile = GetTileAt(tileView.GridPositionX, tileView.GridPositionY);
                List<Tile> tileNeighbors = GetNeighborsAt(tile);
                Debug.Log(tileNeighbors.Count);
                foreach (Tile tileNeighbor in tileNeighbors)
                {
                    if (!tile.IsSelected && currentTileViews.Contains(GetTileViewAt(tileNeighbor.PositionX, tileNeighbor.PositionY)))
                    {
                        if (tile.Fruit == currentFruitType)
                        {
                            if (!currentTileViews.Contains(tileView))
                            {
                                currentTileViews.Add(tileView);
                                break;
                            }
                        }

                    }
                }


            }
            
        }
        public void HandleSelectionEnd()
        {
            
            if (currentTileViews.Count >= 3)
            {
                foreach (TileView tileView in currentTileViews)
                {
                    GetTileAt(tileView.GridPositionX, tileView.GridPositionY).SetSelected(true);
                }
                selectedTileViews.AddRange(currentTileViews);
                currentTileViews.Clear();
            }
            else
            {
                currentTileViews.Clear();
            }
            
        }
    }
}
