using System.Collections;
using System.Threading;
using UnityEngine;

namespace Match3.Helpers
{
    public class FrameRateManager : MonoBehaviour
    {
        [Header("Frame Settings")]
        [SerializeField] private float targetFrameRate = 60.0f;

        private int maxRate = 9999;
        private float currentFrameTime;

        private void Awake()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = maxRate;
            currentFrameTime = Time.realtimeSinceStartup;
            StartCoroutine(nameof(WaitForNextFrame));
        }

        private IEnumerator WaitForNextFrame()
        {
            while (true)
            {
                yield return new WaitForEndOfFrame();
                currentFrameTime += 1.0f / targetFrameRate;
                float time = Time.realtimeSinceStartup;
                float sleepTime = currentFrameTime - time - 0.01f;
                if(sleepTime > 0)
                {
                    Thread.Sleep((int)(sleepTime * 1000));
                }
                while(time < currentFrameTime)
                {
                    time = Time.realtimeSinceStartup;
                }
            }
        }
    }
}
