using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelData", menuName = "Level Data/Level")]
public class LevelScriptable : ScriptableObject
{
    public GameObject LevelPrefab;
    public int[] PoolsRequiredBallCount;
}
