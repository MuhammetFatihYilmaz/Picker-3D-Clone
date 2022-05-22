using UnityEngine;
using TMPro;

public class Stage : MonoBehaviour
{
    [SerializeField] private TMP_Text[] poolsRequiredBallText;
    [SerializeField] private TMP_Text[] ballInsidePoolText;
    [SerializeField] private GameObject[] movingPool;
    [SerializeField] private GameObject[] floorGameobject;
    [SerializeField] private Color floorColor;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform _startPoint;

    void OnEnable()
    {
        LevelManager.Instance.SetStageRequiredBallCountText(poolsRequiredBallText);
        LevelManager.Instance.SetStageFillBallCountText(ballInsidePoolText);
        LevelManager.Instance.SetMovingPoolGameObject(movingPool);
        LevelManager.Instance.SetFloorColor(floorGameobject,floorColor);
        LevelManager.Instance.SetSpawnPoint(spawnPoint);
        LevelManager.Instance.SetStartPoint(_startPoint);
    }  
}
