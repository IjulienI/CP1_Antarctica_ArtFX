using UnityEngine;

public class ElevatorFallTrigger : MonoBehaviour
{
    [SerializeField] private Animator elevatorAnimator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        elevatorAnimator.SetTrigger("crash");
    }
}
