using Match3.Model;
using Match3.SO;
using UnityEngine;

namespace Match3.View
{
    public class VisualGrid : MonoBehaviour
    {
        public int GridWidth { get; private set; }
        public int GridHeight { get; private set; }
        public float GridWorldStartPoint { get; private set; }
        public float GridWorldEndPoint { get; private set; }
        public TileView[,] TileViews { get; private set; }

        [SerializeField] private TileView tilePrefab;
        [SerializeField] private Transform tileViewsParent;

        public void InitializeGrid(LogicalGrid logicalGrid, float gridWorldStartPoint, float gridWorldEndPoint)
        {
            GridWidth = logicalGrid.GridWidth;
            GridHeight = logicalGrid.GridHeight;
            GridWorldStartPoint = gridWorldStartPoint;
            GridWorldEndPoint = gridWorldEndPoint;


            
            TileViews = new TileView[GridWidth, GridHeight];

            for(int x = 0; x < GridWidth; x++)
            {
                for(int y = 0; y < GridHeight; y++)
                {
                    TileViews[x, y] = CreateTileView(x, y, 0, 0, Vector3.one, logicalGrid.Tiles[x,y].Fruit);
                }
            }
        }

        private TileView CreateTileView(int gridPositionX, int gridPositionY, float worldPositionX, float worldPositionY,Vector3 scale, FruitDataSO fruit)
        {
            TileView tileView = Instantiate(tilePrefab, tileViewsParent);
            tileView.SetGridPosition(gridPositionX,gridPositionY);
            tileView.SetWorldPosition(worldPositionX, worldPositionY);
            tileView.SetSprite(fruit.Sprite);
            tileView.SetScale(scale);

            tileView.transform.position = new Vector2(worldPositionX, worldPositionY);
            tileView.transform.localScale = scale;
            tileView.transform.name = $"{fruit.FruitName} ({gridPositionX}:{gridPositionY})";

            return tileView;
        }
    }
}