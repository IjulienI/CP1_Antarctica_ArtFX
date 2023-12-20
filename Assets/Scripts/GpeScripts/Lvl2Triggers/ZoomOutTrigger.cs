using UnityEngine;

public class ZoomOutTrigger : MonoBehaviour
{
    [SerializeField] Animator camAnimator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        camAnimator.enabled = true;
    }
}
