using Match3.SO;
using TMPro;
using UnityEngine;

namespace Match3.Manager
{
    public class GoldManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text goldText;
        public EconomyDataSO EconomyData;

        public int gold;
        private void Start()
        {
            gold = 100;
            goldText.text = this.gold.ToString();
        }

        public void AddGold(int gold)
        {
            this.gold += gold;
            goldText.text = this.gold.ToString();
        }

        public void SubtractGold(int score)
        {
            this.gold -= score;
            if(this.gold < 0)
            {
                score = 0;
            }

            goldText.text = this.gold.ToString();
        }
    }
}
