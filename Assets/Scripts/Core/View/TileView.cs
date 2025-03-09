using UnityEngine;

namespace Match3.View
{
    public class TileView : MonoBehaviour
    {
        public int GridPositionX { get; private set; }
        public int GridPositionY { get; private set; }
        public float WorldPositionX { get; private set; }
        public float WorldPositionY { get; private set; }

        public SpriteRenderer spriteRenderer;

        public TileView(int gridPositionX, int gridPositionY, float worldPositionX, float worldPositionY)
        {
            GridPositionX = gridPositionX;
            GridPositionY = gridPositionY;
            WorldPositionX = worldPositionX;
            WorldPositionY = worldPositionY;
        }

        public void SetHighlight(bool isHighlighted)
        {
            spriteRenderer.color = isHighlighted ? Color.yellow : Color.white; // Örnek: Seçili olunca sar?ya boyar
        }

        public void SetGridPosition(Vector2Int gridPosition)
        {
            GridPositionX = gridPosition.x;
            GridPositionY = gridPosition.y;
        }

        public void SetWorldPosition(Vector2 worldPosition)
        {
            WorldPositionX = worldPosition.x;
            WorldPositionY = worldPosition.y;
        }

        public void SetSprite(Sprite sprite)
        {
            spriteRenderer.sprite = sprite;
        }

        public void SetScale(Vector3 scale)
        {
            transform.localScale = scale;
        }
    }
}
