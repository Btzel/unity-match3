using Match3.SO;
using TMPro;
using UnityEngine;

namespace Match3.Manager
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text scoreText;
        public EconomyDataSO EconomyData;

        public int score;
        private void Start()
        {
            score = 100;
            scoreText.text = this.score.ToString();
        }

        public void AddScore(int score)
        {
            this.score += score;
            scoreText.text = this.score.ToString();
        }

        public void SubtractScore(int score)
        {
            this.score -= score;
            if(this.score < 0)
            {
                score = 0;
            }

            scoreText.text = this.score.ToString();
        }
    }
}
