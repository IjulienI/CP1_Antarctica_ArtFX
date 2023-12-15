using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] private float visionRange; 
    [SerializeField] private float visionConeAngle;
    private GameObject player;
    private AiStateMachine _stateMachine;

    private void Start()
    {
        player = GameObject.FindAnyObjectByType<PlayerMovement>().gameObject;
        _stateMachine = GetComponent<AiStateMachine>();
    }

    private void Update()
    {
        if(Vector2.Distance(transform.position, player.transform.position) <= visionRange)
        {
            if (Vector2.Angle(transform.forward, player.transform.forward) <= visionConeAngle)
            {
                _stateMachine.state = AiStateMachine.State.chase;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        float halfFov = visionConeAngle / 2;
        Quaternion upRayRotation = Quaternion.AngleAxis(-halfFov + 180, Vector3.forward);
        Quaternion downRayRotation = Quaternion.AngleAxis(halfFov + 180, Vector3.forward);

        Vector3 upRayDirection = upRayRotation * transform.right * visionRange;
        Vector3 downRayDirection = downRayRotation * transform.right * visionRange;

        Gizmos.DrawRay(transform.position, upRayDirection);
        Gizmos.DrawRay(transform.position, downRayDirection);
        Gizmos.DrawLine(transform.position + downRayDirection, transform.position + upRayDirection);
    }
}
