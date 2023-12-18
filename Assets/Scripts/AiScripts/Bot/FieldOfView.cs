using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] private float visionRange; 
    [SerializeField] private float visionConeAngle;
    private GameObject player;
    private AiStateMachine _stateMachine;
    private string touchTag;
    bool doOnce;

    private void Start()
    {
        player = GameObject.FindAnyObjectByType<PlayerMovement>().gameObject;
        _stateMachine = GetComponent<AiStateMachine>();
    }

    private void Update()
    {
        if (IsPlayerInFov())
        {
            _stateMachine.state = AiStateMachine.State.chase;
            doOnce = true;
        }
        else if (doOnce)
        {
            doOnce = false;
            _stateMachine.state = AiStateMachine.State.move;
        }
        //Collider2D[] rangeCheck = Physics2D.OverlapCircleAll(transform.position, visionRange, LayerMask.NameToLayer("Player"));
        //if(rangeCheck.Length > 0 )
        //{ 
        //    Transform target = rangeCheck[0].transform;
        //    Vector2 directionToTarget = (target.position - transform.position).normalized;

        //    if(Vector2.Angle(transform.position, directionToTarget) < visionRange / 2)
        //    {
        //        float distanceToTarget = Vector2.Distance(transform.position, directionToTarget);

        //        RaycastHit2D hit = Physics2D.Raycast(transform.position, (transform.position - player.transform.position) * -1);
        //        Debug.DrawRay(transform.position, (transform.position - player.transform.position) * -1);
        //        touchTag = hit.collider.name;
        //        Debug.Log(player.transform.position);
        //        Debug.Log(touchTag);
        //        if (hit.collider != null && hit.collider.tag == "Player" && distanceToTarget <= _stateMachine.maxDistance)
        //        {
        //            _stateMachine.state = AiStateMachine.State.chase;
        //            doOnce = true;
        //        }
        //        else if (doOnce)
        //        {
        //            doOnce = false;
        //            _stateMachine.state = AiStateMachine.State.move;
        //        }
        //    }
        //}
        //if(Vector2.Distance(transform.position, player.transform.position) <= visionRange)
        //{
        //    if (Vector2.Angle(transform.position, player.transform.forward) <= visionConeAngle / 2)
        //    {
        //        float distance = Vector2.Distance(transform.position, player.transform.position);
        //        RaycastHit2D hit = Physics2D.Raycast(transform.position, (transform.position - player.transform.position)*-1);
        //        Debug.DrawRay(transform.position, (transform.position - player.transform.position)*-1);
        //        touchTag = hit.collider.name;
        //        Debug.Log(player.transform.position);
        //        Debug.Log(touchTag);
        //        if (hit.collider != null && hit.collider.tag == "Player" && distance <= _stateMachine.maxDistance)
        //        {
        //            _stateMachine.state = AiStateMachine.State.chase;
        //            doOnce = true;
        //        }
        //        else if(doOnce)
        //        {
        //            doOnce = false;
        //            _stateMachine.state = AiStateMachine.State.move;
        //        }
        //    }
        //}
    }

    bool IsPlayerInFov()
    {
        Vector2 directionToPlayer = player.transform.position - transform.position;
        float angleToPlayer = Vector2.Angle(transform.right, directionToPlayer);

        if(angleToPlayer < visionConeAngle * 0.5f)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, (transform.position - player.transform.position) * -1,visionRange);
            Debug.DrawRay(transform.position, (transform.position - player.transform.position) * -1);
            if (hit.collider != null && hit.collider.tag == "Player")
                {
                return true;
                }
        }
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        float halfFov = visionConeAngle / 2;
        Quaternion upRayRotation = Quaternion.AngleAxis(-halfFov + 180, Vector3.forward);
        Quaternion downRayRotation = Quaternion.AngleAxis(halfFov + 180, Vector3.forward);

        Vector3 upRayDirection = upRayRotation * transform.right * visionRange;
        Vector3 downRayDirection = downRayRotation * transform.right * visionRange;

        Gizmos.DrawRay(transform.position, upRayDirection);
        Gizmos.DrawRay(transform.position, downRayDirection);

        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, visionRange);
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10,10,100,20),touchTag);
    }
}
