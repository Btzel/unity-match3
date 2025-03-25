using Match3.Manager;
using Match3.SO;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelDataSO[] Levels;
    [SerializeField] private int levelIndex;

    public LevelDataSO currentLevel;
        public int score;
        public List<LevelFruit> LevelFruits;
    }

    public CurrentLevel currentLevel;
    private void Start()
    {
        currentLevel = new CurrentLevel();
        currentLevel.LevelName = Levels[levelIndex].LevelName;

        currentLevel.LevelFruits = new List<LevelFruit>();
        foreach (var levelFruit in Levels[levelIndex].LevelFruits)
        {
            currentLevel.LevelFruits.Add(new LevelFruit
            {
                Fruit = levelFruit.Fruit,
                fruitCount = levelFruit.fruitCount,
                fruitTransform = levelFruit.fruitTransform
            });
        }
        currentLevel.score = 0;
    }
}
