using DG.Tweening;
using Match3.Inputs;
using Match3.Manager;
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

        public event Action<List<Tile>,Tile> OnLineSelected;
        public event Action OnLineDeSelected;
        public event Action OnLineComplete;
        public event Action OnLineDestroy;


        private LogicalGrid logicalGrid;
        private VisualGrid visualGrid;

        private FruitDataSO currentFruitType;
        private InputHandler inputHandler;
        private List<Tile> selectedTiles = new List<Tile>();
        private List<Tile> selectedSwapTiles = new List<Tile>();
        private List<Tile> hintedTiles = new List<Tile>();

        private GoldManager goldManager;
        private UIManager uiManager;


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
            inputHandler.OnHintBooster += HandleShowHint;
            
            logicalGrid.OnColumnShifted += HandleColumnShift;
            logicalGrid.OnTilesSwapped += HandleTileSwap;

            goldManager = FindFirstObjectByType<GoldManager>();
            uiManager = FindFirstObjectByType<UIManager>();
        }

        private void HandleShowHint()
        {
            List<List<Tile>> possibleSelections = logicalGrid.GetPossibleSelections();
            possibleSelections.RemoveAll(selection => selection[0].IsSelected);
            if (possibleSelections.Count > 0 && goldManager.gold > goldManager.EconomyData.DestroyCost)
            {
                List<Tile> bestPossibleSelection = possibleSelections[0];
                if (!hintedTiles.Contains(bestPossibleSelection[0]) && bestPossibleSelection.Count > 0)
                {
                    Sequence animationSequence = DOTween.Sequence();
                    animationSequence.AppendCallback(() =>
                    {
                        StartCoroutine(goldManager.PlaySpendGoldAnimation(
                        uiManager.goldText.transform.position,
                        uiManager.hintBoosterImage.transform.position,
                        goldManager.EconomyData.HintCost));
                    });

                    animationSequence.AppendInterval(0.6f);

                    animationSequence.AppendCallback(() => 
                    {
                        hintedTiles.AddRange(bestPossibleSelection);
                        visualGrid.PlayHintAnimation(bestPossibleSelection);
                    });

                    animationSequence.Play();
                    
                }
            }
            
            
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
            if(goldManager.gold > goldManager.EconomyData.DestroyCost)
            {
                List<Tile> selectedTiles = logicalGrid.GetSelectedTiles();
                if(selectedTiles.Count > 0)
                {
                    Sequence animationSequence = DOTween.Sequence();
                    animationSequence.AppendCallback(() => {
                        StartCoroutine(goldManager.PlaySpendGoldAnimation(
                            uiManager.goldText.transform.position,
                            uiManager.destroyButtonText.transform.position,
                            goldManager.EconomyData.DestroyCost));
                    });
                    animationSequence.AppendInterval(0.6f);
                    animationSequence.AppendCallback(() => {
                        foreach (Tile selectedTile in selectedTiles)
                        {
                            foreach (Fruit fruit in goldManager.EconomyData.Fruits)
                            {
                                if (fruit.FruitName == selectedTile.Fruit.FruitName)
                                {
                                    TileView tileView = GetTileViewAt(selectedTile.PositionX, selectedTile.PositionY);
                                    StartCoroutine(goldManager.PlayEarnGoldAnimation(
                                        tileView.transform.position,
                                        uiManager.goldText.transform.position,
                                        fruit.FruitPoint));
                                    break;
                                }
                            }
                        }

                        visualGrid.StopHintAnimation(hintedTiles);
                        hintedTiles.Clear();

                        OnLineDestroy?.Invoke();
                        visualGrid.PlayDestroyAnimationForSelectedTiles(selectedTiles, () =>
                        {
                            logicalGrid.ShiftSelectedTilesUp(fruits);

                        });
                    });

                    animationSequence.Play();
                    
                }
            }
            else
            {
                // Game Over;
            }
        }

        public void HandleSwapTile(Vector2 worldPosition)
        {
            if(goldManager.gold > goldManager.EconomyData.SwapCost)
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
                        Sequence animationSequence = DOTween.Sequence();
                        animationSequence.AppendCallback(() =>
                        {
                            StartCoroutine(goldManager.PlaySpendGoldAnimation(
                            uiManager.goldText.transform.position,
                            uiManager.swapBoosterImage.transform.position,
                            goldManager.EconomyData.SwapCost));
                        });
                        animationSequence.AppendInterval(0.6f);

                        animationSequence.AppendCallback(() =>
                        {
                            logicalGrid.SwapTiles(selectedSwapTiles[0], selectedSwapTiles[1]);
                            selectedSwapTiles.Clear();
                        });

                        animationSequence.Play();
                        
                        
                    }
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
                    OnLineSelected?.Invoke(selectedTiles,tile);
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
                                OnLineSelected?.Invoke(selectedTiles, tile);
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
                OnLineComplete?.Invoke();
                selectedTiles.Clear();
                
            }
            else
            {
                OnTilesDeSelected?.Invoke(selectedTiles);
                OnLineDeSelected?.Invoke();
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
            inputHandler.OnHintBooster -= HandleShowHint;



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
