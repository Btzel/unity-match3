using Match3.InputH;
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
        public event Action<List<Tile>> OnTilesSelected;

        private LogicalGrid logicalGrid;
        private VisualGrid visualGrid;

        private FruitDataSO currentFruitType;
        private InputHandler inputHandler;
        private List<Tile> selectedTiles = new List<Tile>();

        private void Start()
        {
            inputHandler = FindFirstObjectByType<InputHandler>(); // InputHandler'ı buluyoruz.
            inputHandler.OnSelectionStart += HandleSelectionStart;
            inputHandler.OnSelectionContinue += HandleSelectionContinue;
            inputHandler.OnSelectionEnd += HandleSelectionEnd;
            inputHandler.OnDestroySelectedTiles += HandleDestroySelectedTiles;

            
            logicalGrid.OnColumnShifted += OnColumnShifted;
        }

        private void OnColumnShifted(List<Tile> list)
        {
            visualGrid.UpdateTileColumn(list);
        }

        

        private void HandleDestroySelectedTiles(FruitDataSO[] fruits)
        {
            logicalGrid.ShiftSelectedTilesUp(fruits); 
        }

        private void OnDestroy()
        {
            inputHandler.OnSelectionStart -= HandleSelectionStart;
            inputHandler.OnSelectionContinue -= HandleSelectionContinue;
            inputHandler.OnSelectionEnd -= HandleSelectionEnd;
            inputHandler.OnDestroySelectedTiles -= HandleDestroySelectedTiles;

            
            logicalGrid.OnColumnShifted -= OnColumnShifted;
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
                    selectedTiles.Add(tile);
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
                foreach (Tile tile in selectedTiles)
                {
                    tile.SetSelected(true);
                }
                OnTilesSelected?.Invoke(new List<Tile>(selectedTiles));
                selectedTiles.Clear();
            }
            else
            {
                selectedTiles.Clear();
            }
            
        }
    }
}
