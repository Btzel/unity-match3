using UnityEngine;

public class CameraFitter : MonoBehaviour
{
    [SerializeField] private Camera targetCamera;

    [Header("Reference Settings")]
    [Tooltip("Reference Width (Design Resolution)")]
    [SerializeField] private float referenceWidth = 1920f;

    [Tooltip("Reference Height (Design Resolution)")]
    [SerializeField] private float referenceHeight = 1080f;

    private float referenceAspect;
    private int lastScreenWidth, lastScreenHeight;

    private void Start()
    {
        if (targetCamera == null)
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
            // **2D Mode (Orthographic)**

            if (currentAspect >= referenceAspect)
            {
                // Wider screens keep the original size (portrait or landscape)
                targetCamera.orthographicSize = referenceHeight / 200f;
            }
            else
            {
                // Taller screens scale up to fit everything, also ensuring no clipping happens
                float scaleFactor = referenceAspect / currentAspect;
                targetCamera.orthographicSize = (referenceHeight / 200f) * scaleFactor;
            }
        }

        Debug.Log($"Screen: {Screen.width}x{Screen.height}, " +
                  $"Aspect Ratio: {currentAspect:F2}, " +
                  $"Camera Size: {targetCamera.orthographicSize}");
    }
}