using UnityEngine;
using DG.Tweening;

public class MovingPool : MonoBehaviour
{
    private void OnEnable()
    {
        transform.DOMoveY(.25f, .25f, false);
    }
}
