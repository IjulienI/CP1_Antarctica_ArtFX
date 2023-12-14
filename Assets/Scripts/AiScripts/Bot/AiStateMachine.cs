using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AiStateMachine : MonoBehaviour
{
    private GameObject target;
    public GameObject player;
    private Locomotion locomotion;
    public State state;
    public bool alerted;
    [SerializeField] private int randomRange;
    [SerializeField] private int distanceTreshold;
    [SerializeField] private int minWaitRange;
    [SerializeField] private int maxWaitRange;
    [SerializeField] private int maxDistance;

    private bool drawLine;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Target");
        player = GameObject.FindAnyObjectByType<PlayerMovementEvann>().gameObject;
    }

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        switch (state)
        {
            case State.chase:
                target.transform.position = player.transform.position;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position);
                if(hit.collider != null)
                {
                    float distance = Vector2.Distance(transform.position, player.transform.position);
                    if(distance >= maxDistance)
                    {
                        drawLine = true;
                        if(hit.collider.tag != "Player")
                        {
                            state = State.move;
                        }
                    }
                }
                break;
            case State.free:
                GetRandomTarget();
                break;
            case State.move:
                drawLine = false;
                if(Vector3.Distance(transform.position,target.transform.position) <= distanceTreshold)
                {
                    state = State.wait;
                    Invoke(nameof(StopWait), Random.Range(minWaitRange, maxWaitRange));
                }
                break;
        }
    }

    private void Initialize()
    {
        state = State.free;
    }

    private void GetRandomTarget()
    {
        Node[] nodes = FindObjectsOfType<Node>();
        List<Node> tempNodes = new List<Node>();
        for(int i = 0; i < nodes.Length; i++)
        {
            if (Vector2.Distance(transform.position, nodes[i].transform.position) <= randomRange)
            {
                tempNodes.Add(nodes[i]);
            }
        }
        target.transform.position = tempNodes[Random.Range(0,tempNodes.Count)].transform.position;
        state = State.move;
    }

    private void StopWait()
    {
        if(state == State.wait)
        {
            state = State.free;
        }
    }
    enum Locomotion
    {
        idle,
        jump,
        walk,
        snif
    }

    public enum State
    {
        chase,
        free,
        investigation,
        move,
        wait
    }

    private void OnDrawGizmosSelected()
    {
        if(drawLine)
        {
            Gizmos.DrawRay(transform.position, player.transform.position);
        }
    }
}