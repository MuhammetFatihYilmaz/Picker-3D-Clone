using System;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    public static event Action StageStatus_When_ContinueNextStage;
    //
    private LevelScriptable[] _levelData;
    private int _levelCount;
    private int _stageCount;
    private int _barCount;
    private int _ballCountInsidePool;
    private Transform _spawnPoint;
    private Transform _startPoint;
    private GameObject[] _movingPool;
    private GameObject[] _floors;
    private TMP_Text[] _ballInsidePoolText;
    //
    [SerializeField] private Transform firstSpawnPoint;
    [SerializeField] private TMP_Text currentLevelText;
    [SerializeField] private TMP_Text nextLevelText;
    [SerializeField] private TMP_Text stageText;
    [SerializeField] private TMP_Text barFillText;
    [SerializeField] private GameObject _levelContainer;
    //
    private string _saveData;

    void Awake()
    {
        if (Instance != null)
            Destroy(this);
        else
            Instance = this;
        //
        _levelData = Resources.LoadAll<LevelScriptable>("Scriptable Objects/Level Scriptibles");
        _ballInsidePoolText = new TMP_Text[3];
        _movingPool = new GameObject[3];
        _floors = new GameObject[3];
        //
        LoadLevelData();
        UpdateLevelText();
        UpdateStageText();
    }


    #region Level Init Settings
    public void CreateNextLevel()
    {
        GameObject level = Instantiate(_levelData[_levelCount-1].LevelPrefab, _spawnPoint.position,transform.rotation);
        level.transform.parent = _levelContainer.transform;
        Destroy(_levelContainer.transform.GetChild(0).gameObject,10f);
    }

    public void CreateFirstLevel()
    {
        GameObject level = Instantiate(_levelData[_levelCount-1].LevelPrefab, firstSpawnPoint.position, transform.rotation);
        level.transform.parent = _levelContainer.transform;
    }

    public void SetStageRequiredBallCountText(TMP_Text[] poolsRequiredBallText)
    {
        for (int i = 0; i < 3; i++)
        {
            poolsRequiredBallText[i].text = "/ " + _levelData[_levelCount-1].PoolsRequiredBallCount[i].ToString();
        }
    }

    public void SetMovingPoolGameObject(GameObject[] movingPool)
    {
        for (int i = 0; i < 3; i++)
        {
            _movingPool[i] = movingPool[i];
        }
    }

    public void SetStageFillBallCountText(TMP_Text[] ballInsidePoolText)
    {
        for (int i = 0; i < 3; i++)
        {
            _ballInsidePoolText[i] = ballInsidePoolText[i];
            _ballInsidePoolText[i].text = "0";
        }
    }

    public void SetFloorColor(GameObject[] floors, Color floorColor)
    {
        for (int i = 0; i < 3; i++)
        {
            _floors[i] = floors[i];
            _floors[i].GetComponent<MeshRenderer>().material.color = floorColor;
        }
    }

    public void SetSpawnPoint(Transform spawnPoint)
    {
        _spawnPoint = spawnPoint;
    }

    public void SetStartPoint(Transform startPoint)
    {
        _startPoint = startPoint;
    }

    public void SetPlayerPositionToStartPoint(GameObject _playerGameObject)
    {
        _playerGameObject.transform.DOMoveX(0f, 2.5f, false);
        _playerGameObject.transform.DOMoveZ(_startPoint.position.z, 2.5f, false);
    }
    #endregion

    #region Level, Stage, Pool Settings
    public void IncreaseCurrentBallCountInsidePool()
    {
        _ballCountInsidePool++;
    }

    public void UpdateCurrentBallCountTextInsidePool()
    {
        _ballInsidePoolText[_stageCount].text = _ballCountInsidePool.ToString();
    }

    public void ResetCurrentBallCountInsidePool()
    {
        _ballCountInsidePool = 0;
    }

    public void SetEnableMovingPool()
    {
        _movingPool[_stageCount].SetActive(true);
        _movingPool[_stageCount].GetComponent<MeshRenderer>().material.color = _floors[0].GetComponent<MeshRenderer>().material.color;
    }

    public void IncreaseStageCount()
    {
        _stageCount++;
        if (_stageCount == 3)
            _stageCount = 0;
        UpdateStageText();
    }

    public void IncreaseLevelCount()
    {
        if(_levelCount<_levelData.Length)
        {
            _levelCount++;
            UpdateLevelText();
            SaveLevelData();
        }
    }

    public void UpdateLevelText()
    {
        currentLevelText.text = _levelCount.ToString();
        nextLevelText.text = (_levelCount + 1).ToString();
    }

    public void UpdateStageText()
    {
        stageText.text = _stageCount.ToString();
    }

    public bool CheckPoolHaveRequiredBall()
    {
        if (_ballCountInsidePool >= _levelData[_levelCount-1].PoolsRequiredBallCount[_stageCount])
        {
            return true;
        }
        else
            return false;
    }

    public void SaveLevelData()
    {
        PlayerPrefs.SetInt(_saveData, _levelCount);
        PlayerPrefs.Save();
    }

    public void LoadLevelData()
    {
        if (!PlayerPrefs.HasKey(_saveData))
            PlayerPrefs.SetInt(_saveData, 1);
        _levelCount = PlayerPrefs.GetInt(_saveData);
    }
    #endregion

    #region Bar Settings
    public void FillBarCount()
    {
        _barCount++;
        barFillText.text = _barCount.ToString();
    }

    public void ResetBarCount()
    {
        _barCount = 0;
        barFillText.text = _barCount.ToString();
    }

    public float GivePushingPlayerAmountWithBarCount()
    {
        float amount = (float)_barCount / 100;
        return amount;
    }
    #endregion
    //
    public void PassNextStage()
    {
        StageStatus_When_ContinueNextStage += IncreaseStageCount;
        StageStatus_When_ContinueNextStage += ResetCurrentBallCountInsidePool;
        StageStatus_When_ContinueNextStage.Invoke();
        StageStatus_When_ContinueNextStage = null;
    }

}
