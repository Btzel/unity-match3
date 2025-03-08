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
        [SerializeField] private int gridWidth;
        [SerializeField] private int gridHeight;

        [SerializeField] private Vector2 gridWorldStartPoint;
        [SerializeField] private Vector2 gridWorldEndPoint;

        [Header("Tile Settings")]
        [SerializeField] private FruitDataSO[] fruits;

        private void Awake()
        {
            gridPresenter.InitializeGrid(visualGrid, gridWidth, gridHeight, gridWorldStartPoint, gridWorldEndPoint,fruits);
        }
    }
}