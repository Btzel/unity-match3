using DG.Tweening;
using System;
using UnityEngine;

namespace Match3.View
{
    public class TileView : MonoBehaviour
    {
        public int GridPositionX { get; private set; }
        public int GridPositionY { get; private set; }
        public float WorldPositionX { get; private set; }
        public float WorldPositionY { get; private set; }
        public Vector3 DefaultScale { get; private set; }

        public SpriteRenderer spriteRenderer;

        public TileView(int gridPositionX, int gridPositionY, float worldPositionX, float worldPositionY)
        {
            GridPositionX = gridPositionX;
            GridPositionY = gridPositionY;
            WorldPositionX = worldPositionX;
            WorldPositionY = worldPositionY;
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
            DefaultScale = scale;
            transform.localScale = scale;
        }

        public void PlayDestroyAnimation(Action onComplete)
        {
            transform.DOScale(Vector3.zero, 0.3f)
                .SetEase(Ease.InBack)
                .OnComplete(() => onComplete?.Invoke());
        }

        public void PlaySwapAnimationVanish(Action onComplete)
        {
            transform.DOScale(Vector3.zero, 0.3f)
                .SetEase(Ease.InBack)
                .OnComplete(() => onComplete?.Invoke());
        }

        public void PlaySwapAnimationAppear(Action onComplete)
        {
            transform.DOScale(DefaultScale, 0.3f)
                .SetEase(Ease.OutBack)
                .OnComplete(() => onComplete?.Invoke());
        }

        public void PlaySelectedAnimation()
        {
            transform.DOScale(new Vector3(DefaultScale.x * 1.15f,
                                          DefaultScale.y * 1.15f),
                                          0.15f).SetEase(Ease.InBack);
        }

        public void PlayDeSelectedAnimation()
        {
            transform.DOScale(new Vector3(DefaultScale.x,
                                          DefaultScale.y),
                                          0.15f).SetEase(Ease.OutBack);
        }

    }
}
