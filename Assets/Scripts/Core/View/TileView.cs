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

        private Tween hintTween;

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
            spriteRenderer.sortingOrder = 11;
        }

        public void SetScale(Vector3 scale)
        {
            DefaultScale = scale;
            transform.localScale = scale;
        }

        public void PlayDestroyAnimation(Action onComplete, bool isRequired, Transform requiredTransform)
        {
            Sequence seq = DOTween.Sequence();

            if (isRequired && requiredTransform != null)
            {
                seq.Append(transform.DOScale(DefaultScale * 1.3f, 0.2f).SetEase(Ease.OutBack));
                seq.Append(transform.DOMove(requiredTransform.position, 1f).SetEase(Ease.InBack));
                seq.Append(transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack));
            }
            else
            {
                seq.Append(transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack));
                seq.Append(transform.DOScale(Vector3.zero, 1.0f));
            }
            seq.Append(transform.DOScale(Vector3.zero, 0.2f));
            seq.OnComplete(() => onComplete?.Invoke());
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
            transform.DOScale(new Vector3(DefaultScale.x * 1.1f,
                                          DefaultScale.y * 1.1f),
                                          0.15f).SetEase(Ease.InBack);
        }

        public void PlayDeSelectedAnimation()
        {
            transform.DOScale(new Vector3(DefaultScale.x,
                                          DefaultScale.y),
                                          0.15f).SetEase(Ease.OutBack);
        }

        public void PlayHintAnimation()
        {
            if (hintTween != null && hintTween.IsActive()) return;

            hintTween = transform.DOScale(DefaultScale * 1.1f, 0.3f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutQuad);
        }
        public void StopHintAnimation()
        {
            if (hintTween != null)
            {
                hintTween.Kill();
                hintTween = null;
            }

            transform.DOScale(new Vector3(DefaultScale.x,
                                          DefaultScale.y),
                                          0.15f).SetEase(Ease.OutBack);
        }
    }
}
