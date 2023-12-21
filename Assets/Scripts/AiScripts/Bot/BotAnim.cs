using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Unity.PlasticSCM.Editor.WebApi;

public class BotAnim : MonoBehaviour
{
    [SerializeField] private SkeletonAnimation _ska;
    [SerializeField] private AnimationReferenceAsset idle, walk, startJump, idleJump, endJump;
    private Rigidbody2D _rb;
    private Animator _animator;
    private float velocity;
    private string currentAnimation;
    private bool isJumping;
    private bool jumpAnim;
    RaycastHit2D hit;

    private void Awake()
    { 
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }
    private void Start()
    {
        SetCharacterState("idle");
    }
    private void Update()
    {
        velocity = _rb.velocity.x;
        if(Mathf.Abs(velocity) > 0.1f && !jumpAnim)
        {
            SetCharacterState("walk");
        }
        else if(!jumpAnim)
        {
            SetCharacterState("idle");
        }
        hit = Physics2D.Raycast(transform.position, Vector2.down);
        //Debug.Log(hit.collider.gameObject.layer);
        if(hit.distance > 1)
        {
            isJumping = true;
            jumpAnim = true;
            StartJump();
        }
        else EndJump();
    }

    private void StartJump()
    {
        if(isJumping)
        {
            SetCharacterState("startJump");
            Invoke(nameof(idleJump), 0.16f);
        }
    }

    private void IdleJump()
    {
        if (isJumping)
        {
            SetCharacterState("idleJump");
        }
    }

    private void EndJump()
    {
        SetCharacterState("endJump");
        Invoke(nameof(SetBool), 0.43f);
    }

    private void SetBool()
    {
        jumpAnim = false;
    }
    private void SetAnimation(AnimationReferenceAsset animation, bool loop, float timeScale)
    {
        if (animation.name.Equals(currentAnimation)) return;
        _ska.state.SetAnimation(0,animation,loop).TimeScale = timeScale;
        currentAnimation = animation.name;
    }

    private void SetCharacterState(string state)
    {
        if (state.Equals("idle"))
        {
            SetAnimation(idle, true, 1f);
        }
        if (state.Equals("walk"))
        {
            SetAnimation(walk, true, 2.5f);
        }
        if (state.Equals("startJump"))
        {
            SetAnimation(startJump, false, 6f);
        }
        if (state.Equals("endJump"))
        {
            SetAnimation(endJump, false, 3f);
        }
        if (state.Equals("idleJump"))
        {
            SetAnimation(idleJump, true, 1f);
        }
    }
}
