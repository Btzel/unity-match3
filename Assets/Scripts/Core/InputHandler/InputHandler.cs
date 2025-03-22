using Match3.Manager;
using Match3.SO;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Match3.Inputs
{
    public class InputHandler : MonoBehaviour
    {

        public event Action<Vector2> OnSelectionStart;
        public event Action<Vector2> OnSelectionContinue;
        public event Action OnSelectionEnd;
        public event Action<FruitDataSO[]> OnDestroySelectedTiles;
        public event Action<Vector2> OnSwapTile;
        public event Action OnHintBooster;



        [SerializeField] private Button destroyButton;
        [SerializeField] private GameManager manager;
        [SerializeField] private Toggle swapBooster;
        [SerializeField] private Button hintBooster;

        private bool isSwap;

        private void Start()
        {
            destroyButton.onClick.AddListener(() => OnDestroySelectedTiles?.Invoke(manager.fruits));
            swapBooster.onValueChanged.AddListener(UpdateSwapTileState);
            hintBooster.onClick.AddListener(() => OnHintBooster?.Invoke());
        }

        private void Update()
        {
            if (isSwap)
            {
                if (Input.GetMouseButtonDown(0)) { OnSwapTile?.Invoke(GetMouseWorldPosition()); }
               
            }
            else
            {
                if (Input.GetMouseButtonDown(0)) { OnSelectionStart?.Invoke(GetMouseWorldPosition()); }
                if (Input.GetMouseButton(0)) { OnSelectionContinue?.Invoke(GetMouseWorldPosition()); }
                if (Input.GetMouseButtonUp(0)) { OnSelectionEnd?.Invoke(); }
                
            }
            
        }
        #region Helper Functions
        private Vector2 GetMouseWorldPosition()
        {
            return Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        private void UpdateSwapTileState(bool swapState)
        {
            isSwap = swapState;
        }


        #endregion
    }
}