using DG.Tweening;
using Match.SO;
using Match3.SO;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Match3.Manager
{
    public class UIManager : MonoBehaviour
    {
        [Header("UI Data")]
        [SerializeField] public UIDataSO uiData;

        [Header("Level Data")]
        [SerializeField] private LevelManager levelManager;
        [SerializeField] private Transform levelRequirementParent;

        [Header("Game - Background")]
        [SerializeField] private Image background;

        [Header("Game - TopPanel")]
        [SerializeField] private GameObject topPanel;

        [SerializeField] private Image levelBackground;
        [SerializeField] private TMP_Text levelText;

        [SerializeField] private Image goldBackground;
        [SerializeField] private Image goldImage;
        [SerializeField] public TMP_Text goldText;

        [Header("Game - BottomPanel")]
        [SerializeField] private Image bottomPanel;
        [SerializeField] public Button destroyButton;
        [SerializeField] public TMP_Text destroyButtonText;

        [SerializeField] public Image swapBoosterImage;
        [SerializeField] public Image hintBoosterImage;

        public class RequirementText
        {
            public TextMeshProUGUI requirementText;
            public string fruitName;
        }

        public List<RequirementText> requirementTexts = new List<RequirementText>();
        private void Start()
        {
            background.gameObject.SetActive(true);
            background.sprite = uiData.BackgroundSprite;

            topPanel.gameObject.SetActive(true);

            levelBackground.sprite = uiData.LevelBackgroundSprite;
            levelText.font = uiData.TextFont;
            levelText.text = levelManager.currentLevel.LevelName;

            foreach(LevelFruit levelFruit in levelManager.currentLevel.LevelFruits)
            {
                GameObject newLevelRequirement = new GameObject($"{levelFruit.Fruit.FruitName}_{levelFruit.fruitCount}");
                Image levelRequirementImage = newLevelRequirement.AddComponent<Image>();
                levelRequirementImage.sprite = levelFruit.Fruit.Sprite;

                AspectRatioFitter imageRatioFitter = levelRequirementImage.AddComponent<AspectRatioFitter>();
                imageRatioFitter.aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth;
                imageRatioFitter.aspectRatio = 1;

                GameObject newLevelRequirementText = new GameObject($"{levelFruit.Fruit.FruitName}_{levelFruit.fruitCount}_Text");
                TextMeshProUGUI levelRequirementText = newLevelRequirementText.AddComponent<TextMeshProUGUI>();

                levelRequirementText.text = levelFruit.fruitCount.ToString();
                levelRequirementText.font = uiData.TextFont;
                levelRequirementText.alignment = TextAlignmentOptions.Center;
                levelRequirementText.color = new Color(50f / 255f, 50f / 255f, 50f / 255f, 255f / 255f);
                levelRequirementText.fontSize = 60;
                
                RectTransform textRect = levelRequirementText.GetComponent<RectTransform>();
                textRect.position = new Vector3(0,-70,0);
                
                levelRequirementText.transform.SetParent(levelRequirementImage.transform);
                levelRequirementImage.transform.SetParent(levelRequirementParent,false);

                levelFruit.fruitTransform = levelRequirementImage.transform;

                RequirementText newRequirementText = new RequirementText();
                newRequirementText.requirementText = levelRequirementText;
                newRequirementText.fruitName = levelFruit.Fruit.FruitName;

                requirementTexts.Add(newRequirementText);

            }

            goldBackground.sprite = uiData.GoldBackgroundSprite;
            goldImage.sprite = uiData.GoldSprite;
            goldText.font = uiData.TextFont;

            bottomPanel.gameObject.SetActive(true);
            bottomPanel.sprite = uiData.BottomPanelSprite;
            
            destroyButton.GetComponent<Image>().sprite = uiData.DestroyButtonSprite;
            destroyButtonText.font = uiData.TextFont;

            swapBoosterImage.sprite = uiData.SwapBoosterSprite;
            hintBoosterImage.sprite = uiData.HintBoosterSprite;


        }
        public void ButtonOnClick(Image image)
        {
            image.transform.DOScale(image.transform.localScale * 1.2f,
                                          0.3f).SetEase(Ease.InBack);
        }

        public void ButtonOffClick(Image image)
        {
            image.transform.DOScale(1, 0.3f).SetEase(Ease.InBack);
        }
        
    }
}