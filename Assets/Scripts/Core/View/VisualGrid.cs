﻿using DG.Tweening;
using JetBrains.Annotations;
using Match3.Helpers;
using Match3.Model;
using Match3.Presenter;
using Match3.SO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Match3.View
{
    public class VisualGrid : MonoBehaviour
    {

        public int GridWidth { get; private set; }
        public int GridHeight { get; private set; }
        public Vector2 GridStartPos { get; private set; }
        public Vector2 GridEndPos { get; private set; }

        [SerializeField] private TileView tilePrefab;
        [SerializeField] private Transform tilesParent;
        [SerializeField] private GridPresenter gridPresenter;
        [SerializeField] private GameObject gridPrefab;
        [SerializeField] private Transform gridsParent;
        [SerializeField] private GridDataSO[] grids;

        [SerializeField] private GameObject lineRenderersParent;
        [SerializeField] private Sprite circleSprite;


        private TileView[,] tileViews;
        private LineRenderer currentLine;
        private List<LineRenderer> completeLines = new List<LineRenderer>();
        private List<TileView> previousTileViews = new List<TileView>();

        private void Start()
        {
            gridPresenter = FindFirstObjectByType<GridPresenter>();
            gridPresenter.OnTileSelected += AnimateSelectedTile;
            gridPresenter.OnTilesDeSelected += AnimateDeSelectedTiles;

            gridPresenter.OnLineSelected += HandleLineSelected;
            gridPresenter.OnLineComplete += HandleLineComplete;
            gridPresenter.OnLineDeSelected += HandleLineDeSelected;
            gridPresenter.OnLineDestroy += HandleLineDestroy;

        }

        private void HandleLineDestroy()
        {
            foreach(LineRenderer line in completeLines)
            {
                Destroy(line.gameObject);
            }
            completeLines.Clear();
        }

        private void HandleLineDeSelected()
        {
            if (currentLine != null)
            {
                Destroy(currentLine.gameObject);
                currentLine = null;
                previousTileViews.Clear();
            }
            
        }

        private void HandleLineComplete()
        { 
            completeLines.Add(currentLine);
            previousTileViews.Clear();
            currentLine = null;
        }

        private void HandleLineSelected(List<Tile> tileList, Tile currentTile)
        {
            if (tileList.Count == 1)
            {
                TileView tileView = GetTileView(currentTile.PositionX, currentTile.PositionY);

                GameObject lineObj = new GameObject($"Line ({tileList.Last().PositionX},{tileList.Last().PositionY})");
                LineRenderer lineRenderer = lineObj.AddComponent<LineRenderer>();
                lineRenderer.transform.parent = lineRenderersParent.transform;

                GameObject circle = new GameObject($"Circle ({tileView.GridPositionX},{tileView.GridPositionY})");
                SpriteRenderer circleRenderer = circle.AddComponent<SpriteRenderer>();
                circleRenderer.sprite = circleSprite;
                circleRenderer.color = currentTile.Fruit.SelectionColor;
                circleRenderer.sortingOrder = 10;
                circleRenderer.transform.localScale = tileView.transform.localScale * 0.85f;
                circleRenderer.transform.position = tileView.transform.position;
                circleRenderer.transform.parent = lineRenderer.transform;

                float lineWidth = tileView.transform.localScale.x * 0.2f;

                lineRenderer.startWidth = lineWidth;
                lineRenderer.endWidth = lineWidth;
                lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
                lineRenderer.sortingOrder = 9;
                lineRenderer.startColor = currentTile.Fruit.SelectionColor;
                lineRenderer.endColor = currentTile.Fruit.SelectionColor;
                lineRenderer.positionCount = 1;
                
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, tileView.transform.position);
                previousTileViews.Add(tileView);

                currentLine = lineRenderer;
            }
            else if(tileList.Count == 2)
            {
                currentLine.positionCount++;
                TileView tileView = GetTileView(currentTile.PositionX, currentTile.PositionY);
                currentLine.SetPosition(currentLine.positionCount - 1, tileView.transform.position);
                previousTileViews.Add(tileView);

                GameObject circle = new GameObject($"Circle ({tileView.GridPositionX},{tileView.GridPositionY})");
                SpriteRenderer circleRenderer = circle.AddComponent<SpriteRenderer>();
                circleRenderer.sprite = circleSprite;
                circleRenderer.color = currentTile.Fruit.SelectionColor;
                circleRenderer.sortingOrder = 10;
                circleRenderer.transform.localScale = tileView.transform.localScale * 0.85f;
                circleRenderer.transform.position = tileView.transform.position;
                circleRenderer.transform.parent = currentLine.transform;
            }
            else
            {
                Tile lastTile = gridPresenter.GetTileAt(previousTileViews.Last().GridPositionX, previousTileViews.Last().GridPositionY);
                List<Tile> lastTileNeighbors = gridPresenter.GetNeighborsAt(lastTile);
                if (lastTileNeighbors.Contains(currentTile))
                {
                    currentLine.positionCount++;
                    TileView tileView = GetTileView(currentTile.PositionX, currentTile.PositionY);
                    currentLine.SetPosition(currentLine.positionCount - 1, tileView.transform.position);
                    previousTileViews.Add(tileView);

                    GameObject circle = new GameObject($"Circle ({tileView.GridPositionX},{tileView.GridPositionY})");
                    SpriteRenderer circleRenderer = circle.AddComponent<SpriteRenderer>();
                    circleRenderer.sprite = circleSprite;
                    circleRenderer.color = currentTile.Fruit.SelectionColor;
                    circleRenderer.sortingOrder = 10;
                    circleRenderer.transform.localScale = tileView.transform.localScale * 0.85f;
                    circleRenderer.transform.position = tileView.transform.position;
                    circleRenderer.transform.parent = currentLine.transform;

                }
                else
                {
                    for(int i = previousTileViews.Count - 2; i >= 0; i--)
                    {
                        currentLine.positionCount++;
                        TileView previousTileView = previousTileViews[i];
                        currentLine.SetPosition(currentLine.positionCount - 1, previousTileView.transform.position);
                        previousTileViews.Add(previousTileView);

                        Tile previousTile = gridPresenter.GetTileAt(previousTileView.GridPositionX, previousTileView.GridPositionY);
                        List<Tile> previousTileNeighbours = gridPresenter.GetNeighborsAt(previousTile);

                        if (previousTileNeighbours.Contains(currentTile))
                        {
                            currentLine.positionCount++;
                            TileView tileView = GetTileView(currentTile.PositionX, currentTile.PositionY);
                            currentLine.SetPosition(currentLine.positionCount - 1, tileView.transform.position);
                            previousTileViews.Add(tileView);

                            GameObject circle = new GameObject($"Circle ({tileView.GridPositionX},{tileView.GridPositionY})");
                            SpriteRenderer circleRenderer = circle.AddComponent<SpriteRenderer>();
                            circleRenderer.sprite = circleSprite;
                            circleRenderer.color = currentTile.Fruit.SelectionColor;
                            circleRenderer.sortingOrder = 10;
                            circleRenderer.transform.localScale = tileView.transform.localScale * 0.85f;
                            circleRenderer.transform.position = tileView.transform.position;
                            circleRenderer.transform.parent = currentLine.transform;

                            break;
                        }
                    }
                }
            }
        }

        private void OnDestroy()
        {
            gridPresenter.OnTileSelected -= AnimateSelectedTile;
            gridPresenter.OnTilesDeSelected -= AnimateDeSelectedTiles;

            gridPresenter.OnLineSelected -= HandleLineSelected;
            gridPresenter.OnLineComplete -= HandleLineComplete;
            gridPresenter.OnLineDeSelected -= HandleLineDeSelected;
            gridPresenter.OnLineDestroy -= HandleLineDestroy;
        }
        
        public void InitializeGrid(LogicalGrid logicalGrid, Vector2 startPos, Vector2 endPos)
        {
            GridWidth = logicalGrid.GridWidth;
            GridHeight = logicalGrid.GridHeight;
            GridStartPos = startPos;
            GridEndPos = endPos;
            tileViews = new TileView[GridWidth, GridHeight];


            float tileSize = CalculateTileSize(GridWidth, GridHeight, GridStartPos, GridEndPos);
            Vector2 newStartPos = CalculateStartPos(GridWidth, GridHeight, GridStartPos, GridEndPos, tileSize);

            for (int x = 0; x < GridWidth; x++)
            {
                for (int y = 0; y < GridHeight; y++)
                {
                    Tile tileModel = logicalGrid.GetTile(x, y);
                    if (tileModel != null)
                    {
                        Sprite tileSprite = tileModel.Fruit.Sprite;
                        string fruitName = tileModel.Fruit.FruitName;
                        Vector2Int gridPosition = new Vector2Int(x, y);
                        Vector2 tilePosition = CalculateTileViewPos(gridPosition, newStartPos, tileSize);
                        Vector3 tileScale = new Vector3(tileSize,tileSize);

                        tileViews[x, y] = CreateTileView(gridPosition,tilePosition, tileSprite,tileScale, fruitName, tileSize);
                    }
                }
            }
        }

        private TileView CreateTileView(Vector2Int gridPosition, Vector2 worldPosition, Sprite sprite,Vector3 scale, string fruitName, float tileSize)
        {

            GameObject grid = Instantiate(gridPrefab, gridsParent);
            grid.GetComponent<SpriteRenderer>().sprite = GridPicker.GetGrid(gridPosition, new Vector2Int(GridWidth,GridHeight), grids).Sprite;
            grid.GetComponent<SpriteRenderer>().sortingOrder = 8;
            grid.transform.position = worldPosition;
            grid.transform.name = $"Grid ({gridPosition.x},{gridPosition.y})";
            grid.transform.localScale = scale;
            

            TileView newTileView = Instantiate(tilePrefab, tilesParent);
            newTileView.SetGridPosition(gridPosition);
            newTileView.SetWorldPosition(worldPosition);
            newTileView.SetSprite(sprite);
            newTileView.SetScale(scale);
            newTileView.transform.name = $"{fruitName} ({gridPosition.x}, {gridPosition.y})";
            newTileView.transform.position = worldPosition;

            return newTileView;
        }

        public TileView GetTileView(int x, int y)
        {
            if (tileViews[x, y] != null)
            {
                return tileViews[x, y];
            }
            else
            {
                return null;
            }
        }

        private void AnimateSelectedTile(Tile tile)
        {
            TileView tileView = GetTileView(tile.PositionX, tile.PositionY);
            if (tileView != null)
            {
                tileView.PlaySelectedAnimation();
            }
            
        }

        private void AnimateDeSelectedTiles(List<Tile> tileList)
        {
            foreach(Tile tile in tileList)
            {
                TileView tileView = GetTileView(tile.PositionX, tile.PositionY);
                if(tileView != null)
                {
                    tileView.PlayDeSelectedAnimation();
                }
            }
        }

        public void PlayDestroyAnimationForSelectedTiles(List<Tile> selectedTiles, Action onComplete, List<LevelFruit> requiredFruits)
        {
            int tilesRemaining = selectedTiles.Count;

            foreach (Tile tile in selectedTiles)
            {
                TileView tileView = GetTileView(tile.PositionX, tile.PositionY);
                
                if (tileView != null)
                {
                    LevelFruit matchedFruit = requiredFruits.FirstOrDefault(lf => lf.Fruit == tile.Fruit);
                    bool isRequired = matchedFruit != null;
                    Transform fruitTransform = matchedFruit?.fruitTransform;

                    tileView.PlayDestroyAnimation(() =>
                    {
                        tilesRemaining--;
                        if (tilesRemaining == 0)
                        {
                            onComplete?.Invoke();
                        }
                        
                    },isRequired,fruitTransform);
                }
            }
        }

        public void PlayHintAnimation(List<Tile> hintTiles)
        {
            foreach(Tile tile in hintTiles)
            {
                TileView tileView = GetTileView(tile.PositionX, tile.PositionY);

                tileView.PlayHintAnimation();
            }
        }


        public void StopHintAnimation(List<Tile> hintTiles)
        {
            foreach (Tile tile in hintTiles)
            {
                TileView tileView = GetTileView(tile.PositionX, tile.PositionY);

                tileView.StopHintAnimation();
            }
        }

        public void UpdateTileColumn(List<Tile> tileList)
        {
            List<TileView> tileViewColumn = new List<TileView>();
            int x = tileList[0].PositionX;
            float tileSize = CalculateTileSize(GridWidth, GridHeight, GridStartPos, GridEndPos);
            for (int y = 0; y < tileList.Count; y++)
            {
                tileViewColumn.Add(tileViews[x, y]);
            }
            Vector2 startPos = CalculateStartPos(GridWidth, GridHeight, GridStartPos, GridEndPos, tileSize);
            
            float animationSpeed = 20f;
            int tilePos = 0;
            for (int y = 0; y < tileList.Count; y++)
            {
                Tile tile = tileList[y];
                tileViews[x, y] = tileViewColumn[tile.PositionY];
                tileViews[x, y].SetGridPosition(new Vector2Int(x, y));
                
                if (tile.IsSelected)
                {
                    tile.SetSelected(false);
                    Vector2 gridPos = CalculateTileViewPos(new Vector2Int(x, GridHeight + tilePos), startPos, tileSize);
                    tilePos++;
                    tileViews[x, y].transform.position = gridPos;
                    tileViews[x, y].spriteRenderer.sprite = tile.Fruit.Sprite;
                    tileViews[x, y].transform.localScale = tileViews[x, y].DefaultScale;
                }
                Vector2 targetPos = CalculateTileViewPos(new Vector2Int(x, y), startPos, tileSize);
                tileViews[x, y].transform.DOMove(targetPos, animationSpeed).SetSpeedBased();
                tileViews[x, y].transform.name = $"{tile.Fruit.name} ({x}, {y})";
                
            }
        }

        public IEnumerator UpdateSwappedTiles(Tile[] tiles)
        {
            Tile tile1 = tiles[0];
            Tile tile2 = tiles[1];

            TileView tileView1 = GetTileView(tile1.PositionX, tile1.PositionY);
            TileView tileView2 = GetTileView(tile2.PositionX, tile2.PositionY);

            tileViews[tile1.PositionX, tile1.PositionY] = tileView2;
            tileViews[tile2.PositionX, tile2.PositionY] = tileView1;

            tileViews[tile1.PositionX, tile1.PositionY].SetGridPosition(new Vector2Int(tile1.PositionX,tile1.PositionY));
            tileViews[tile2.PositionX, tile2.PositionY].SetGridPosition(new Vector2Int(tile2.PositionX, tile2.PositionY));

            Vector2Int gridPosition1 = new Vector2Int(tileView1.GridPositionX, tileView1.GridPositionY);
            Vector2Int gridPosition2 = new Vector2Int(tileView2.GridPositionX, tileView2.GridPositionY);

            float tileSize = CalculateTileSize(GridWidth, GridHeight, GridStartPos, GridEndPos);

            Vector2 newStartPos = CalculateStartPos(GridWidth, GridHeight, GridStartPos, GridEndPos, tileSize);


            Vector2 tilePosition1 = CalculateTileViewPos(gridPosition1, newStartPos, tileSize);
            Vector2 tilePosition2 = CalculateTileViewPos(gridPosition2, newStartPos, tileSize);

            tileView1.SetWorldPosition(tilePosition1);
            tileView2.SetWorldPosition(tilePosition2);

            tileView1.transform.name = $"{tile1.Fruit.name} ({gridPosition1.x}, {gridPosition1.y})";
            tileView2.transform.name = $"{tile2.Fruit.name} ({gridPosition2.x}, {gridPosition2.y})";


            bool vanish1Complete = false;
            bool vanish2Complete = false;

            tileView1.PlaySwapAnimationVanish(() => vanish1Complete = true);
            tileView2.PlaySwapAnimationVanish(() => vanish2Complete = true);

            yield return new WaitUntil(() => vanish1Complete && vanish2Complete);

            tileView1.transform.position = tilePosition1;
            tileView2.transform.position = tilePosition2;

            bool appear1Complete = false;
            bool appear2Complete = false;

            tileView1.PlaySwapAnimationAppear(() => appear1Complete = true);
            tileView2.PlaySwapAnimationAppear(() => appear2Complete = true);

            yield return new WaitUntil(() => appear1Complete && appear2Complete);

        }




        public float CalculateTileSize(int gridWidth, int gridHeight, Vector2 startPos, Vector2 endPos)
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

        public Vector2 CalculateTileViewPos(Vector2Int gridPos, Vector2 startPos, float tileSize)
        {
            float posX = startPos.x + gridPos.x * tileSize + tileSize / 2f;
            float posY = startPos.y + gridPos.y * tileSize + tileSize / 2f;

            return new Vector2(posX, posY);
        }

        
    }
}
