using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;
using static UnityEditor.Experimental.GraphView.GraphView;

public class AiStateMachine : MonoBehaviour
{
    private GameObject target;
    private AIPathController _controller;
    private Locomotion locomotion;
    [Header("Movements")]
    [SerializeField] private int randomRange;
    [SerializeField] private int distanceTreshold;
    [SerializeField] private int minWaitRange;
    [SerializeField] private int maxWaitRange;
    [SerializeField] public int maxDistance;
    [Header("State")]
    public State state;
    [Header("Other")]
    public GameObject player;
    public bool goSound;
    private FieldOfView _fov;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Target");
        player = GameObject.FindAnyObjectByType<PlayerMovement>().gameObject;
        _controller = GetComponent<AIPathController>();
        _fov = GetComponent<FieldOfView>();
        
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
                _controller.speed = _controller.baseSpeed;
                target.transform.position = player.transform.position;
                goSound = false;
                break;
            case State.free:
                if(_fov.cycle > 0 )
                {
                    GetRandomInRange(transform, 2,5);
                    _controller.speed = _controller.baseSpeed / 2;
                    _fov.cycle--;
                }
                else
                {
                    _controller.speed = _controller.baseSpeed;
                    GetRandomTarget();
                }
                break;
            case State.move:
                if (Vector3.Distance(transform.position, target.transform.position) <= distanceTreshold)
                {
                    state = State.wait;
                    goSound = false;
                    Invoke(nameof(StopWait), Random.Range(minWaitRange, maxWaitRange));
                }
                else
                {
                    Vector2 closerNodePos = new Vector2(0.0f, 0.0f);
                    float close = Mathf.Infinity;
                    for (int i = 0; i < _controller.AllNodes.Count; i++)
                    {
                        if (Vector2.Distance(target.transform.position, _controller.AllNodes[i].transform.position) < close)
                        {
                            close = Vector2.Distance(target.transform.position, _controller.AllNodes[i].transform.position);
                            closerNodePos = _controller.AllNodes[i].transform.position;
                        }
                    }
                    target.transform.position = closerNodePos;
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
        List<Node> tempNodes = new List<Node>();
        for(int i = 0; i < _controller.AllNodes.Count; i++)
        {
            if (Vector2.Distance(transform.position, _controller.AllNodes[i].transform.position) <= randomRange)
            {
                tempNodes.Add(_controller.AllNodes[i]);
            }
        }
        target.transform.position = tempNodes[Random.Range(0,tempNodes.Count)].transform.position;
        state = State.move;
    }

    public void GetRandomInRange(Transform targetRandom, int minRange, int maxRange)
    {
        List<Node> tempNodes = new List<Node>();
        for (int i = 0; i < _controller.AllNodes.Count; i++)
        {
            if (Vector2.Distance(targetRandom.position, _controller.AllNodes[i].transform.position) >= minRange && Vector2.Distance(targetRandom.position, _controller.AllNodes[i].transform.position) <= maxRange)
            {
                tempNodes.Add(_controller.AllNodes[i]);
            }
        }
        target.transform.position = tempNodes[Random.Range(0, tempNodes.Count)].transform.position;
        state = State.move;
    }

    private void StopWait()
    {
        if(state == State.wait)
        {
            int rnd = Random.Range(0, 3);
            if (rnd < 2)
            {
                state = State.free;
            }
            else
            {
                GetRandomInRange(player.transform, 3, 8);
            }
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
}