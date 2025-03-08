using UnityEngine;

namespace Match3.SO
{
    [CreateAssetMenu(fileName = "NewFruit", menuName = "Create New Fruit")]
    public class FruitDataSO : ScriptableObject
    {
        public string FruitName;
        public Sprite Sprite;
    }
}
