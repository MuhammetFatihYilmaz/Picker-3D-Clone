using UnityEngine;
using DG.Tweening;

public class LeftPropeller : MonoBehaviour
{
    void Awake()
    {
        transform.DORotate(new Vector3(0, 360, 0), 2f, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);
    }
}
