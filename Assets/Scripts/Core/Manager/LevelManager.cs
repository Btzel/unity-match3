using Match3.SO;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelDataSO[] Levels;
    [SerializeField] private int levelIndex;

    public LevelDataSO currentLevel;

    private void Start()
    {
        currentLevel = Levels[levelIndex];
    }
}
