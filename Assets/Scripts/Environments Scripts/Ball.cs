using UnityEngine;

public class Ball : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("StageBorderTag"))
            LevelManager.Instance.IncreaseCurrentBallCountInsidePool();
    }
}
