using UnityEngine;
using System.Collections.Generic;
using Match3.SO;


[System.Serializable]
public class LevelFruit
{
    public FruitDataSO Fruit;
    public int fruitCount;
    public Transform fruitTransform;
}

namespace Match3.SO
{

    [CreateAssetMenu(fileName = "NewLevel", menuName = "Create New Level Data")]
    public class LevelDataSO : ScriptableObject
    {
        [Header("General")]
        public string LevelName;
        [Space]
        [Header("Level Requirements")]
        public int score;
        public List<LevelFruit> LevelFruits = new List<LevelFruit>();
    }
}