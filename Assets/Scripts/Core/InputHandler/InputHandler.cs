using System;
using UnityEngine;
using UnityEngine.UI;

namespace Match3.InputH
{
    public class InputHandler : MonoBehaviour
    {

        public event Action<Vector2> OnSelectionStart;
        public event Action<Vector2> OnSelectionContinue;
        public event Action OnSelectionEnd;
        public event Action OnDestroySelectedTiles;

        [SerializeField] private Button destroyButton;

        private void Start()
        {
            destroyButton.onClick.AddListener(() => OnDestroySelectedTiles?.Invoke());
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0)) { OnSelectionStart?.Invoke(GetMouseWorldPosition()); }
            else if (Input.GetMouseButton(0)) { OnSelectionContinue?.Invoke(GetMouseWorldPosition()); }
            else if (Input.GetMouseButtonUp(0)) { OnSelectionEnd?.Invoke(); }
        }
        private Vector2 GetMouseWorldPosition()
        {
            return Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }
}