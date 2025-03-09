using Match3.Model;
using Match3.Presenter;
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

        public void InitializeGrid(LogicalGrid logicalGrid, Vector2 startPos, Vector2 endPos)
        {
            GridWidth = logicalGrid.GridWidth;
            GridHeight = logicalGrid.GridHeight;
            GridStartPos = startPos;
            GridEndPos = endPos;
            tileViews = new TileView[GridWidth, GridHeight];


            float tileSize = gridPresenter.CalculateTileSize(GridWidth, GridHeight, GridStartPos, GridEndPos);
            Vector2 newStartPos = gridPresenter.CalculateStartPos(GridWidth, GridHeight, GridStartPos, GridEndPos, tileSize);

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
                        Vector2 tilePosition = gridPresenter.CalculateTileViewPos(gridPosition, newStartPos, tileSize);
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
