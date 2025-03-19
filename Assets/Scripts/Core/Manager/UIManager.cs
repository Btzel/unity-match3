using Match.SO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Match3.Manager
{
    public class UIManager : MonoBehaviour
    {
        [Header("UI Data")]
        [SerializeField] private UIDataSO uiData;

        [Header("Game - Background")]
        [SerializeField] private Image background;

        [Header("Game - TopPanel")]
        [SerializeField] private Image topPanel;

        [SerializeField] private Image scoreBackground;
        [SerializeField] private TMP_Text scoreText;

        [Header("Game - BottomPanel")]
        [SerializeField] private Image bottomPanel;
        [SerializeField] private Button destroyButton;
        [SerializeField] private TMP_Text destroyButtonText;

        [SerializeField] private Toggle swapToggle;
        [SerializeField] private Image swapToggleImage;
        [SerializeField] private TMP_Text swapToggleText;


        private void Start()
        {
            background.gameObject.SetActive(true);
            background.sprite = uiData.BackgroundSprite;

            topPanel.gameObject.SetActive(true);
            topPanel.sprite = uiData.TopPanelSprite;
            
            scoreBackground.sprite = uiData.ScoreBackgroundSprite;
            scoreText.font = uiData.TextFont;

            bottomPanel.gameObject.SetActive(true);
            bottomPanel.sprite = uiData.bottomPanelSprite;
            
            destroyButton.GetComponent<Image>().sprite = uiData.DestroyButtonSprite;
            destroyButtonText.font = uiData.TextFont;

            swapToggle.GetComponent<Image>().sprite = uiData.SwapToggleSprite;
            swapToggleImage.sprite = uiData.SwapToggleImageSprite;
            swapToggleText.font = uiData.TextFont;



        }
    }
}