using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    //
    public bool IsBallsFallInPool { get; private set; }
    public bool IsPlayerStartParkour { get; private set; }
    public bool IsNewLevelCreated { get; private set; }
    //
    [SerializeField] private GameObject playerGameobject;
    [SerializeField] private GameObject restartButton;
    //
    public static event Action StageStatus_When_BallsFallInPool;
    public static event Action ParkourStatus_When_LevelEnding;

    void Awake()
    {
        if (Instance != null)
            Destroy(this);
        else
            Instance = this;
    }

    void Start()
    {
        SetLevelFirstStart();
    }

    #region Balls Fall In Pool Settings
    public void SetBallsFallInPool()
    {
        StageStatus_When_BallsFallInPool = SetIsBallsFallInPoolTrue;
        StageStatus_When_BallsFallInPool.Invoke();
    }

    public void ContinueMovingAfterBallFallInPool()
    {
        StageStatus_When_BallsFallInPool = SetIsBallsFallInPoolFalse;
        StageStatus_When_BallsFallInPool.Invoke();
    }
    //
    private void SetIsBallsFallInPoolTrue()
    {
        IsBallsFallInPool = true;
    }

    private void SetIsBallsFallInPoolFalse()
    {
        IsBallsFallInPool = false;
    }
    #endregion

    #region Finish Parkour Settings
    public void SetPlayerStartParkour()
    {
        ParkourStatus_When_LevelEnding = SetIsPlayerStartParkourTrue;
        ParkourStatus_When_LevelEnding.Invoke();
        StartCoroutine(SetPlayerFinishPushingParkour());
    }

    private void SetIsPlayerStartParkourTrue()
    {
        IsPlayerStartParkour = true;
    }

    private void SetIsPlayerStartParkourFalse()
    {
        IsPlayerStartParkour = false;
    }

    IEnumerator SetPlayerFinishPushingParkour()
    {
        yield return new WaitForSeconds(.5f);
        ParkourStatus_When_LevelEnding = SetIsPlayerStartParkourFalse;
        ParkourStatus_When_LevelEnding.Invoke();
        LevelManager.Instance.ResetBarCount();
    }
    #endregion

    #region New Level Settings
    public void SetGameNewLevel()
    {
        LevelManager.Instance.IncreaseLevelCount();
        LevelManager.Instance.CreateNextLevel();
        IsNewLevelCreated = true;
        StartCoroutine(SetPlayerPosition());
        StartCoroutine(SetNewLevelCreatingFalse());
    }

    IEnumerator SetPlayerPosition()
    {
        yield return new WaitForSeconds(5f);
        LevelManager.Instance.SetPlayerPositionToStartPoint(playerGameobject);        
        IsNewLevelCreated = false;
    }

    IEnumerator SetNewLevelCreatingFalse()
    {
        yield return new WaitForSeconds(5f);
        IsNewLevelCreated = false;
    }
    #endregion

    #region First Start Level Settings
    public void SetLevelFirstStart()
    {
        LevelManager.Instance.CreateFirstLevel();
        LevelManager.Instance.UpdateLevelText();
    }
    #endregion


    public void SetStageStatusToBallFallInsidePool()
    {
        StartCoroutine(CheckBallInsidePoolSeq());
    }

    IEnumerator CheckBallInsidePoolSeq()
    {
        int timer = 0;
        int timerMax = 6;
        while (timer < timerMax)
        {
            yield return new WaitForSeconds(.5f);
            LevelManager.Instance.UpdateCurrentBallCountTextInsidePool();
            timer++;
            if (timer == timerMax)
            {
                bool isPoolHaveRequiredBall = LevelManager.Instance.CheckPoolHaveRequiredBall();
                if (isPoolHaveRequiredBall)
                {
                    LevelManager.Instance.SetEnableMovingPool();
                    yield return new WaitForSeconds(1.5f);
                    ContinueMovingAfterBallFallInPool();
                    LevelManager.Instance.PassNextStage();
                }
                else
                {
                    restartButton.SetActive(true);
                    Debug.Log("Game Over");
                }

            }
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
}
