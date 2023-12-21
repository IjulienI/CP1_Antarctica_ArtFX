using UnityEngine;
using Spine.Unity;

public class BotAnim : MonoBehaviour
{
    [SerializeField] private SkeletonAnimation _ska;
    [SerializeField] private AnimationReferenceAsset idle, walk, startJump, idleJump, endJump, wallJump;
    private Rigidbody2D _rb;
    private float velocity;
    private string currentAnimation;
    private bool isJumping;
    RaycastHit2D hit;
    RaycastHit2D wallHit;

    private void Awake()
    { 
        _rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        SetCharacterState("idle");
    }
    private void Update()
    {
        hit = Physics2D.Raycast(transform.position, Vector2.down);
        wallHit = Physics2D.Raycast(transform.position, Vector2.right);
        Debug.DrawRay(transform.position, Vector2.right);
        velocity = _rb.velocity.x;
        if(Mathf.Abs(velocity) > 0.1f && !isJumping)
        {
            SetCharacterState("walk");
        }
        else if(!isJumping)
        {
            SetCharacterState("idle");
        }
        if(hit.distance > 1)
        {
            isJumping = true;
            StartJump();
        }
        else if(isJumping)
        {
            EndJump();
        }

        if(isJumping && wallHit.distance < 2)
        {
            WallJump();
        }
    }

    private void StartJump()
    {
        SetCharacterState("startJump");
        Invoke(nameof(IdleJump), 0.16f);
    }

    private void IdleJump()
    {
        SetCharacterState("idleJump");
    }

    private void EndJump()
    {
        SetCharacterState("endJump");
        Invoke(nameof(SetBool), 0.43f);
    }

    private void WallJump()
    {
        SetCharacterState("wallJump");
        Invoke(nameof(IdleJump), 0.27f);
    }

    private void SetBool()
    {
        isJumping = false;
    }
    private void SetAnimation(AnimationReferenceAsset animation, bool loop, float timeScale)
    {
        if (animation.name.Equals(currentAnimation)) return;
        _ska.state.SetAnimation(0, animation, loop).TimeScale = timeScale;
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
            SetAnimation(walk, true, 2f);
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
        if (state.Equals("wallJump"))
        {
            SetAnimation(wallJump, true, 3f);
        }
    }
}
