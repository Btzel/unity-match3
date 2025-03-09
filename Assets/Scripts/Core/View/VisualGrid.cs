using Match3.Model;
using Match3.Presenter;
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

        private TileView[,] tileViews;

        private void Start()
        {
            gridPresenter = FindFirstObjectByType<GridPresenter>();
            gridPresenter.OnTilesSelected += HighlightSelectedTiles;
        }

        private void OnDestroy()
        {
            gridPresenter.OnTilesSelected -= HighlightSelectedTiles;
        }

        private void HighlightSelectedTiles(List<Tile> selectedTiles)
        {
            foreach (Tile tile in selectedTiles)
            {
                TileView tileView = GetTileView(tile.PositionX, tile.PositionY);
                if (tileView != null)
                {
                    tileView.SetHighlight(true);
                }
            }
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
