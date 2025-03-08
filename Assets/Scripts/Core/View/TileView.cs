using UnityEngine;

namespace Match3.View
{
    public class TileView : MonoBehaviour
    {
        public int GridPositionX { get; private set; }
        public int GridPositionY { get; private set; }
        public float WorldPositionX { get; private set; }
        public float WorldPositionY { get; private set; }

        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void SetGridPosition(int gridPositionX, int gridPositionY)
        {
            GridPositionX = gridPositionX;
            GridPositionY = gridPositionY;
        }

        public void SetWorldPosition(float worldPositionX, float worldPositionY)
        {
            WorldPositionX = worldPositionX;
            WorldPositionY = worldPositionY;
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
