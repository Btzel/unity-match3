using UnityEngine;

namespace Match3.Helpers
{
    public class CameraFitter : MonoBehaviour
    {
        private Camera targetCamera;

        private float referenceWidth = 1920f;
        private float referenceHeight = 1080f;

        private float referenceAspect;
        private int lastScreenWidth, lastScreenHeight;

        private void Start()
        {
            
            targetCamera = Camera.main;
            referenceAspect = referenceWidth / referenceHeight;
            AdjustCamera();
        }

        private void Update()
        {
            if (Screen.width != lastScreenWidth || Screen.height != lastScreenHeight)
                AdjustCamera();
        }

        private void AdjustCamera()
        {
            lastScreenWidth = Screen.width;
            lastScreenHeight = Screen.height;

            float currentAspect = (float)Screen.width / Screen.height;

            if (targetCamera.orthographic)
            {
                if (currentAspect >= referenceAspect)
                {
                    targetCamera.orthographicSize = referenceHeight / 200f;
                }
                else
                {
                    float scaleFactor = referenceAspect / currentAspect;
                    targetCamera.orthographicSize = (referenceHeight / 200f) * scaleFactor;
                }
            }
        }
    }
}