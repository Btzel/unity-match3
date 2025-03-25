using DG.Tweening;
using Match.SO;
using Match3.SO;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Match3.Manager
{
    public class GoldManager : MonoBehaviour
    {
        public Action OnEarnFinish;

        [SerializeField] private Transform goldParent;
        [SerializeField] private TMP_Text goldText;
        [SerializeField] private UIDataSO uiData;
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

        public void PlayEarnGoldAnimation(GameObject goldObj, Vector3 endPos, int goldAmount)
        {

            
            goldObj.transform.DOScale(Vector3.one * 0.33f,
                                        0.15f).SetEase(Ease.OutBack);
            goldObj.GetComponent<SpriteRenderer>().sortingOrder = 20;
            float duration = 0.5f;
            goldObj.GetComponent<SpriteRenderer>().transform.DOMove(endPos, duration)
                .SetEase(Ease.InBack)
                .OnComplete(() => {   
                    AddGold(goldAmount);
                    if (goldObj != null)
                    {
                        Destroy(goldObj);
                    }
                });

            
        }

        public IEnumerator PlaySpendGoldAnimation(Vector3 startPos, Vector3 endPos, int goldAmount)
        {
            GameObject[] gold = new GameObject[6];
            int completedAnimations = 0;

            for (int i = 0; i < gold.Length; i++)
            {
                gold[i] = new GameObject("Gold");
                SpriteRenderer goldImage = gold[i].AddComponent<SpriteRenderer>();
                goldImage.sprite = uiData.GoldSprite;
                goldImage.transform.localScale = Vector3.one * 0.33f;
                goldImage.transform.position = startPos;
                goldImage.transform.SetParent(goldParent);
                goldImage.sortingOrder = 20;
                float duration = 0.3f;

                GameObject currentGold = gold[i];
                goldImage.transform.DOMove(endPos, duration)
                    .SetEase(Ease.InBack)
                    .OnComplete(() => {
                        completedAnimations++;
                        if (completedAnimations >= gold.Length)
                        {
                            SubtractGold(goldAmount);

                            foreach (GameObject goldObj in gold)
                            {
                                if (goldObj != null)
                                {
                                    Destroy(goldObj);
                                }
                            }
                        }
                    });

                yield return new WaitForSeconds(0.05f);
            }
            yield return new WaitUntil(() => completedAnimations >= gold.Length);
        }


    }
}
