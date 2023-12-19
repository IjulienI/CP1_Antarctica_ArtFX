using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Drawing.Inspector.PropertyDrawers;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [Header("Ai FOV")]

    [Range(5f, 25f)]
    [SerializeField] private float visionRange;

    [Range(45f, 180f)]
    [SerializeField] private float visionConeAngle;

    [Range(5f,25f)]
    [SerializeField] private float forceVisioRange;

    [Header("Ai Sound Detection")]

    [Range(5f, 25f)]
    [SerializeField] private float soundRange;

    [Range(0f, 15f)]
    [SerializeField] private float minRange;

    [Range(0f, 15f)]
    [SerializeField] private float maxRange;

    [Header("Snif")]
    [SerializeField] private int minCycle;
    [SerializeField] private int maxCycle;

    private GameObject player;
    private AiStateMachine _stateMachine;

    private string touchTag;

    bool doOnce;
    bool detected;
    public int cycle;

    private void Start()
    {
        player = GameObject.FindAnyObjectByType<PlayerMovement>().gameObject;
        _stateMachine = GetComponent<AiStateMachine>();
    }

    private void Update()
    {
        if ((IsPlayerInFov() || detected && Vector2.Distance(transform.position, player.transform.position) < forceVisioRange) && !(PlayerMovement.instance.stateOfFire == 1))
        {
            _stateMachine.state = AiStateMachine.State.chase;
            doOnce = true;
            detected = true;
            cycle = 0;
        }
        else if (doOnce)
        {
            doOnce = false;
            detected = false;
            cycle = Random.Range(minCycle, maxCycle);
            _stateMachine.state = AiStateMachine.State.move;
        }       
    }

    bool IsPlayerInFov()
    {
        Vector2 directionToPlayer = (player.transform.position - transform.position)*-1;
        float angleToPlayer = Vector2.Angle(transform.right, directionToPlayer);

        if(angleToPlayer < visionConeAngle / 2)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, (player.transform.position - transform.position),visionRange);
            Debug.DrawRay(transform.position, (player.transform.position - transform.position));
            if (hit.collider != null && hit.collider.tag == "Player")
                {
                return true;
                }
        }
        return false;
    }

    public void ReceiveNoise()
    {
        if(Vector2.Distance(player.transform.position, transform.position) < soundRange && !_stateMachine.goSound)
        {
            _stateMachine.goSound = true;
            float distance = Vector2.Distance(player.transform.position, transform.position) / 4;
            _stateMachine.GetRandomInRange(player.transform,(int)(minRange * distance), (int)(maxRange * distance));
            Debug.Log((int)(minRange * distance)/4);
        }
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

        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(transform.position, soundRange);

        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, visionRange);

        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(transform.position, forceVisioRange);


    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10,10,100,20),touchTag);
    }
}
