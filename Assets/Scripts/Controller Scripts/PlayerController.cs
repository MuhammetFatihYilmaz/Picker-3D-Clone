using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _playerRigidbody;
    //
    [SerializeField] private PlayerScriptable playerData;
    [SerializeField] private GameObject playerBallContainer;
    [SerializeField] private GameObject exitedBallsParent;
    [SerializeField] private GameObject _propellersParent;
    //
    private float _pushingPower;

    void Awake()
    {
        _playerRigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (GameManager.Instance.IsBallsFallInPool)
            return;
        if(GameManager.Instance.IsPlayerStartParkour)
        {
            PushPlayerOnParkourWay();
            return;
        }
        KeyboardMovement();
    }

    private void KeyboardMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 forwardMove = Vector3.forward * playerData.ForwardSpeed * Time.fixedDeltaTime;
        Vector3 horizontalMove = Vector3.right * horizontalInput * playerData.HorizontalSpeed * Time.fixedDeltaTime;
        _playerRigidbody.MovePosition(transform.position + forwardMove + horizontalMove);
    }

    private void PushPlayerOnParkourWay()
    {
        _pushingPower = LevelManager.Instance.GivePushingPlayerAmountWithBarCount();
        _playerRigidbody.AddForce(transform.forward * _pushingPower, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BallTag"))
        {
            other.gameObject.transform.parent = playerBallContainer.transform;
            LevelManager.Instance.FillBarCount();
        }
        if (other.CompareTag("PropellerTag"))
        {
            Destroy(other.gameObject);
            SetActivePropellers();
        }
        //
        if (other.CompareTag("StageBorderTag"))
        {
            SetPasivePropellers();
            Destroy(other.gameObject,3);
            GameManager.Instance.SetBallsFallInPool();
            PushBallsOnPool();
            GameManager.Instance.SetStageStatusToBallFallInsidePool();
            StartCoroutine(RemoveBallsInPool());
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("BallTag"))
            other.gameObject.transform.parent = exitedBallsParent.transform;
        //
        if (other.CompareTag("ParkourStarterTag"))
            GameManager.Instance.SetPlayerStartParkour();
        //
        if (other.CompareTag("SpawnStarterTag"))
        {
            if(!GameManager.Instance.IsNewLevelCreated)
                GameManager.Instance.SetGameNewLevel();
        }
    }

    private void PushBallsOnPool()
    {
        Rigidbody[] ballsRigidbories = playerBallContainer.GetComponentsInChildren<Rigidbody>();
        foreach(Rigidbody rb in ballsRigidbories)
        {
            rb.AddForce(transform.forward * .45f, ForceMode.Impulse);
        }
    }

    IEnumerator RemoveBallsInPool()
    {
        yield return new WaitForSeconds(1.5f);
        foreach (Transform item in exitedBallsParent.transform)
        {
            Destroy(item.gameObject);
        }
    }

    private void SetActivePropellers()
    {
        _propellersParent.SetActive(true);
    }

    private void SetPasivePropellers()
    {
        _propellersParent.SetActive(false);
    }
}
