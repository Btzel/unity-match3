using Match3.Model;
using UnityEngine;

namespace Match3.View
{
    public class VisualGrid : MonoBehaviour
    {
        public Vector2 GridStartPos { get; private set; }
        public Vector2 GridEndPos { get; private set; }

        [SerializeField] private TileView tilePrefab;
        [SerializeField] private Transform tilesParent;

        private TileView[,] tileViews;

        public void InitializeGrid(LogicalGrid logicalGrid, Vector2 startPos, Vector2 endPos)
        {
            int width = logicalGrid.GridWidth;
            int height = logicalGrid.GridHeight;
            tileViews = new TileView[width, height];

            GridStartPos = startPos;
            GridEndPos = endPos;

            float gridWidth = Mathf.Abs(endPos.x - startPos.x);
            float gridHeight = Mathf.Abs(endPos.y - startPos.y);
            float tileSizeX = gridWidth / width;
            float tileSizeY = gridHeight / height;
            float tileSize = Mathf.Min(tileSizeX, tileSizeY);

            Vector2 gridCenter = new Vector2((startPos.x + endPos.x) / 2f, (startPos.y + endPos.y) / 2f);

            float startX = gridCenter.x - (width * tileSize) / 2f;
            float startY = gridCenter.y - (height * tileSize) / 2f;


            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Tile tileModel = logicalGrid.GetTile(x, y);
                    if (tileModel != null)
                    {
                        Sprite tileSprite = tileModel.Fruit.Sprite;
                        string fruitName = tileModel.Fruit.FruitName;

                        float posX = startX + x * tileSize + tileSize / 2f;
                        float posY = startY + y * tileSize + tileSize / 2f;
                        Vector2Int gridPosition = new Vector2Int(x, y);
                        Vector2 tilePosition = new Vector2(posX, posY);
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
    }
}
