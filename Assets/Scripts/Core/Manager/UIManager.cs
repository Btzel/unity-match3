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
        [SerializeField] private GameObject topPanel;

        [SerializeField] private Image goldBackground;
        [SerializeField] private Image goldImage;
        [SerializeField] private TMP_Text goldText;

        [Header("Game - BottomPanel")]
        [SerializeField] private Image bottomPanel;
        [SerializeField] private Button destroyButton;
        [SerializeField] private TMP_Text destroyButtonText;

        [SerializeField] private Image swapBoosterImage;
        [SerializeField] private Image hintBoosterImage;



        private void Start()
        {
            background.gameObject.SetActive(true);
            background.sprite = uiData.BackgroundSprite;

            topPanel.SetActive(true);
            
            goldBackground.sprite = uiData.goldBackgroundSprite;
            goldImage.sprite = uiData.goldSprite;
            goldText.font = uiData.TextFont;

            bottomPanel.gameObject.SetActive(true);
            bottomPanel.sprite = uiData.bottomPanelSprite;
            
            destroyButton.GetComponent<Image>().sprite = uiData.DestroyButtonSprite;
            destroyButtonText.font = uiData.TextFont;

            swapBoosterImage.sprite = uiData.SwapBoosterSprite;
            hintBoosterImage.sprite = uiData.HintBoosterSprite;
        }
    }
}