using Match3.Inputs;
using Match3.Model;
using Match3.SO;
using Match3.View;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Match3.Presenter
{
    public class GridPresenter : MonoBehaviour
    {
        public event Action<Tile> OnTileSelected;
        public event Action<List<Tile>> OnTilesDeSelected;
        public event Action<List<Tile>> OnCreateLineRenderer;
        public event Action OnDestroyLineRenderer;

        private LogicalGrid logicalGrid;
        private VisualGrid visualGrid;

        private FruitDataSO currentFruitType;
        private InputHandler inputHandler;
        private List<Tile> selectedTiles = new List<Tile>();
        private List<Tile> selectedSwapTiles = new List<Tile>();

        public void InitializeGrid(VisualGrid visualGrid, Vector2Int gridSize, Vector2 gridStartPos, Vector2 gridEndPos, FruitDataSO[] fruits)
        {
            this.visualGrid = visualGrid;
            logicalGrid = new LogicalGrid();
            logicalGrid.InitializeGrid(gridSize.x, gridSize.y, fruits);
            visualGrid.InitializeGrid(logicalGrid, gridStartPos, gridEndPos);
        }

        private void Start()
        {
            inputHandler = FindFirstObjectByType<InputHandler>();
            inputHandler.OnSelectionStart += HandleSelectionStart;
            inputHandler.OnSelectionContinue += HandleSelectionContinue;
            inputHandler.OnSelectionEnd += HandleSelectionEnd;
            inputHandler.OnDestroySelectedTiles += HandleDestroySelectedTiles;
            inputHandler.OnSwapTile += HandleSwapTile;
            
            logicalGrid.OnColumnShifted += HandleColumnShift;
            logicalGrid.OnTilesSwapped += HandleTileSwap;
        }
        #region Event Handlers
        private void HandleTileSwap(Tile[] tiles)
        {
            StartCoroutine(visualGrid.UpdateSwappedTiles(tiles));
        }

        private void HandleColumnShift(List<Tile> list)
        {
            visualGrid.UpdateTileColumn(list);
        }


        private void HandleDestroySelectedTiles(FruitDataSO[] fruits)
        {
            List<Tile> selectedTiles = logicalGrid.GetSelectedTiles();
            OnDestroyLineRenderer?.Invoke();
            visualGrid.PlayDestroyAnimationForSelectedTiles(selectedTiles, () =>
            {
                logicalGrid.ShiftSelectedTilesUp(fruits);
            });
        }

        public void HandleSwapTile(Vector2 worldPosition)
        {
            RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);
            if (hit.collider != null && hit.collider.CompareTag("TileView"))
            {
                TileView tileView = hit.collider.GetComponent<TileView>();
                Tile tile = GetTileAt(tileView.GridPositionX, tileView.GridPositionY);
                if (!tile.IsSelected)
                {
                    if (selectedSwapTiles.Count == 0)
                    {
                        selectedSwapTiles.Add(tile);
                        OnTileSelected?.Invoke(tile);
                    }
                    else
                    {
                        List<Tile> neighbors = logicalGrid.GetAllNeighbors(selectedSwapTiles[0]);
                        if (neighbors.Contains(tile))
                        {
                            selectedSwapTiles.Add(tile);
                            OnTileSelected?.Invoke(tile);
                        }
                        else
                        {
                            OnTilesDeSelected?.Invoke(selectedSwapTiles);
                            selectedSwapTiles.Clear();
                        }
                    }
                }
                if (selectedSwapTiles.Count == 2)
                {
                    logicalGrid.SwapTiles(selectedSwapTiles[0], selectedSwapTiles[1]);
                    selectedSwapTiles.Clear();
                }
            }
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
                    selectedTiles.Add(tile);
                    OnTileSelected?.Invoke(tile);
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

                foreach (Tile tileNeighbor in tileNeighbors)
                {
                    if (!tile.IsSelected && selectedTiles.Contains(tileNeighbor))
                    {
                        if (tile.Fruit == currentFruitType)
                        {
                            if (!selectedTiles.Contains(tile))
                            {
                                selectedTiles.Add(tile);
                                OnTileSelected?.Invoke(tile);
                                break;
                            }
                        }

                    }
                }
            }
        }
        public void HandleSelectionEnd()
        {

            if (selectedTiles.Count >= 3)
            {
                OnCreateLineRenderer?.Invoke(selectedTiles);
                foreach (Tile tile in selectedTiles)
                {
                    tile.SetSelected(true);
                }
                selectedTiles.Clear();
            }
            else
            {
                OnTilesDeSelected?.Invoke(selectedTiles);
                selectedTiles.Clear();
            }

        }
#endregion

        private void OnDestroy()
        {
            inputHandler.OnSelectionStart -= HandleSelectionStart;
            inputHandler.OnSelectionContinue -= HandleSelectionContinue;
            inputHandler.OnSelectionEnd -= HandleSelectionEnd;
            inputHandler.OnDestroySelectedTiles -= HandleDestroySelectedTiles;
            inputHandler.OnSwapTile -= HandleSwapTile;

            logicalGrid.OnColumnShifted -= HandleColumnShift;
            logicalGrid.OnTilesSwapped -= HandleTileSwap;
        }

        #region Helper Functions
        public Tile GetTileAt(int x, int y)
        {
            return logicalGrid.GetTile(x, y);
        }

        public TileView GetTileViewAt(int x, int y)
        {
            return visualGrid.GetTileView(x, y);
        }

        public List<Tile> GetNeighborsAt(Tile tile)
        {
            return logicalGrid.GetFruitNeighbors(tile);
        }
        #endregion
    }
}
