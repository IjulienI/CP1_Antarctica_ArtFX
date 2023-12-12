using System;
using System.Net;
using UnityEngine;
using Pathfinding;
using System.Threading;
using UnityEditor.Rendering;
using UnityEngine.UIElements;

public class EnemyAi : MonoBehaviour
{
    [Header("PathFinding")]
    public Transform target;
    public float activateDistance = 50f;
    public float pathUpdateSeconds = 0.5f;

    [Header("Physics")]
    public float speed = 200f;
    public float nextWaypointDistance = 3f;
    public float jumpNodeHeightRequirement = 0.8f;
    public float jumpModifier = 0.3f;
    public float jumpCheckOffset = 0.1f;
    public LayerMask groundLayer;

    [Header("Custom Behavior")]
    public bool followEnabled = true;
    public bool jumpEnabled = true;
    public bool directionLookEnabled = true;

    private Path _path;
    private int currentWaypoint = 0;
    bool isGrounded = false;
    Seeker _seeker;
    Rigidbody2D _rb;

    private void Start()
    {
        _seeker = GetComponent<Seeker>();
        _rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);
    }

    private void Update()
    {
        Debug.DrawRay(this.transform.position, new Vector2(this.transform.position.x - (GetComponent<SpriteRenderer>().bounds.size.x) / 2, this.transform.position.y), Color.red);
        RaycastHit2D left = Physics2D.Raycast(new Vector2(this.transform.position.x - (GetComponent<SpriteRenderer>().bounds.size.x / 2), this.transform.position.y),
            Vector2.up, 2f, groundLayer);
    }

    private void FixedUpdate()
    {
        Debug.DrawRay(this.transform.position, Vector2.down, Color.green);
        RaycastHit2D hitGround = Physics2D.Raycast(this.transform.position, Vector2.down, 1f, groundLayer);
        isGrounded = hitGround.collider;
        
        if (TargetInDistance() && followEnabled)
        {
            PathFollow();
        }
    }

    private void UpdatePath()
    {
        if (followEnabled && TargetInDistance() && _seeker.IsDone())
        {
            _seeker.StartPath(_rb.position, target.position, OnPathComplete);
        }
    }

    private void PathFollow()
    {
        if (_path == null)
        {
            return;
        }

        if (currentWaypoint >= _path.vectorPath.Count)
        {
            return;
        }
        
        //isGrounded = Physics2D.Raycast(transform.position, -Vector3.up, GetComponent<Collider2D>().bounds.extents.y + jumpCheckOffset);        

        Vector2 direction = ((Vector2)_path.vectorPath[currentWaypoint] - _rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        if(jumpEnabled && isGrounded)
        {
            if(direction.y > jumpNodeHeightRequirement)
            {
                _rb.AddForce(Vector2.up * jumpModifier);
            }

            isGrounded = false;
        }
        Debug.Log(force);

        _rb.AddForce(force);

        float distance = Vector2.Distance(_rb.position, _path.vectorPath[currentWaypoint]);
        if(distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        if (directionLookEnabled)
        {
            if (_rb.velocity.y > 0.05f)
            {
                transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if(_rb.velocity.y < -0.05f)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
    }

    private bool TargetInDistance()
    {
        return Vector2.Distance(transform.position, target.transform.position) < activateDistance;
    }

    private void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            _path = p;
            currentWaypoint = 0;
        }
    }  
}
