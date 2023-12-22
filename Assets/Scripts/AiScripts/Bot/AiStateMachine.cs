using System.Collections.Generic;
using UnityEngine;

public class AiStateMachine : MonoBehaviour
{
    private GameObject target;
    private AIPathController _controller;
    private Locomotion locomotion;
    private CircleCollider2D _circleCollider;

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
    public GameObject zone;
    public AudioSource _monsterSource;
    public AudioClip monsterMovements, monsterBite, monsterScream;
    public bool canResream = true;

    private void Awake()
    {
        //_circleCollider = GetComponent<CircleCollider2D>();
        //_circleCollider.enabled = false;
        _monsterSource = GetComponent<AudioSource>();
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
                //_controller.speed = _controller.baseSpeed;
                target.transform.position = player.transform.position;
                goSound = false;
                break;
            case State.free:
                if (_fov.cycle > 0 )
                {
                    GetRandomInRange(transform, 2,5);
                    //_controller.speed = _controller.baseSpeed / 1.5f;
                    _fov.cycle--;
                }
                else
                {
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
        if(PlayerMovement.instance.stateOfFire == 3)
        {
            state = State.chase;
        }
        //if(state == State.chase)
        //{
        //    if (_circleCollider.enabled == false)
        //    {
        //        _circleCollider.enabled = true;
        //    }
        //}
        //else
        //{
        //    if (_circleCollider.enabled == true)
        //    {
        //        _circleCollider.enabled = false;
        //    }
        //}
    }

    private void Initialize()
    {
        state = State.free;
    }

    private void GetRandomTarget()
    {
        List<Node> tempNodes = new List<Node>();
        for (int i = 0; i < _controller.AllNodes.Count; i++)
        {
            if (Vector2.Distance(transform.position, _controller.AllNodes[i].transform.position) <= randomRange)
            {
                tempNodes.Add(_controller.AllNodes[i]);
            }
        }
        if(tempNodes.Count > 0)
        {
            target.transform.position = tempNodes[Random.Range(0, tempNodes.Count)].transform.position;
            tempNodes.Clear();
            state = State.move;
        }
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
        if(tempNodes.Count > 0)
        {
            target.transform.position = tempNodes[Random.Range(0, tempNodes.Count)].transform.position;
            tempNodes.Clear();
            state = State.move;
        }
    }

    private void StopWait()
    {
        if(state == State.wait)
        {
            canResream = true;
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

    public void SetFree()
    {
        state = State.free;
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