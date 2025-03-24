using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace Match.SO
{
    [CreateAssetMenu(fileName = "NewUI", menuName = "Create New UI Data")]
    public class UIDataSO : ScriptableObject
    {
        [Header("General")]
        public TMP_FontAsset TextFont;

        [Header("Game - Background")]
        public Sprite BackgroundSprite;

        [Header("Game - TopPanel")]
        public Sprite TopPanelSprite;
        public Sprite LevelBackgroundSprite;

        public Sprite GoldBackgroundSprite;
        public Sprite GoldSprite;

        [Header("Game - BottomPanel")]
        public Sprite BottomPanelSprite;
        public Sprite DestroyButtonSprite;
        public Sprite SwapBoosterSprite;
        public Sprite HintBoosterSprite;
    }
}
