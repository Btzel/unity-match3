using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Fruit
{
    public string FruitName;
    public int FruitPoint;
}


namespace Match3.SO
{
    [CreateAssetMenu(fileName = "NewEconomy", menuName = "Create New Economy Data")]
    public class EconomyDataSO : ScriptableObject
    {
        public List<Fruit> Fruits = new List<Fruit>();
        public int DestroyCost;
        public int SwapCost;
    }
}