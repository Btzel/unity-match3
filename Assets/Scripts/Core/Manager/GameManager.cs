using Match3.Presenter;
using Match3.SO;
using Match3.View;
using UnityEngine;

namespace Match3.Manager
{
    public class GameManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GridPresenter gridPresenter;
        [SerializeField] private VisualGrid visualGrid;

        [Header("Grid Settings")]
        [SerializeField] private Vector2Int gridSize;

        [SerializeField] private Vector2 gridStartPos;
        [SerializeField] private Vector2 gridEndPos;

        [Header("Tile Settings")]
        public FruitDataSO[] fruits;

        private void Awake()
        {
            gridPresenter.InitializeGrid(visualGrid, gridSize, gridStartPos, gridEndPos, fruits);
        }
    }
}