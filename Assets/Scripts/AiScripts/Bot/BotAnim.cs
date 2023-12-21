using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotAnim : MonoBehaviour
{
    private AIPathController _controller;
    private Rigidbody2D _rb;
    private Animator _animator;
    private float velocity;

    private void Awake()
    {
        _controller = GetComponent<AIPathController>(); 
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }
    private void Update()
    {
        velocity = _rb.velocity.x;
        if(Mathf.Abs(velocity) > 0.1f )
        {
            _animator.SetBool("isWalking", true);
        }
        else if(_animator.GetBool("isWalking") != false)
        {
            _animator.SetBool("isWalking", false);
        }
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down);
        //Debug.Log(hit.collider.gameObject.layer);
        if(hit.distance > 2)
        {
            _animator.SetBool("isJumping", true);
        }
        else if(_animator.GetBool("isJumping") == true)
        {
            _animator.SetBool("isJumping", false);
        }
    }
}
