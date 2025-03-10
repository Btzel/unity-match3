﻿using DG.Tweening;
using Match3.Helpers;
using Match3.Model;
using Match3.Presenter;
using Match3.SO;
using System;
using System.Collections.Generic;
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

        private TileView[,] tileViews;

        private void Start()
        {
            gridPresenter = FindFirstObjectByType<GridPresenter>();
            gridPresenter.OnTileSelected += AnimateSelectedTile;
            gridPresenter.OnTilesDeSelected += AnimateDeSelectedTiles;
        }

        private void OnDestroy()
        {
            gridPresenter.OnTileSelected -= AnimateSelectedTile;
            gridPresenter.OnTilesDeSelected -= AnimateDeSelectedTiles;
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

        public void PlayDestroyAnimationForSelectedTiles(List<Tile> selectedTiles, Action onComplete)
        {
            int tilesRemaining = selectedTiles.Count;

            foreach (Tile tile in selectedTiles)
            {
                TileView tileView = GetTileView(tile.PositionX, tile.PositionY);
                if (tileView != null)
                {
                    tileView.PlayDestroyAnimation(() =>
                    {
                        tilesRemaining--;
                        if (tilesRemaining == 0)
                        {
                            onComplete?.Invoke();
                        }
                    });
                }
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
            Vector2 gridPos = CalculateTileViewPos(new Vector2Int(x, GridHeight + 5), startPos, tileSize);
            float duration = .5f;
            for (int y = 0; y < tileList.Count; y++)
            {
                Tile tile = tileList[y];
                tileViews[x, y] = tileViewColumn[tile.PositionY];
                tileViews[x, y].SetGridPosition(new Vector2Int(x, y));
                if (tile.IsSelected)
                {
                    tile.SetSelected(false);

                    tileViews[x, y].transform.position = gridPos;
                    tileViews[x, y].spriteRenderer.sprite = tile.Fruit.Sprite;
                    tileViews[x, y].transform.localScale = tileViews[x, y].DefaultScale;
                }
                Vector2 targetPos = CalculateTileViewPos(new Vector2Int(x, y), startPos, tileSize);
                tileViews[x, y].transform.DOMove(targetPos, duration).SetEase(Ease.OutQuint);
                tileViews[x, y].transform.name = $"{tile.Fruit.name} ({x}, {y})";
     
            }
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
