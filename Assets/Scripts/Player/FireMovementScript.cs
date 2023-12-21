using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireMovementScript : MonoBehaviour
{
    [SerializeField] Vector3 PositionFlame;
    [SerializeField] Animator anim;

    void Start()
    {
        PositionFlame = transform.localPosition;
    }

    void Update()
    {
        if (anim.GetBool("isWalking") == true || anim.GetBool("isClimbing") == true || anim.GetBool("isClimb") == true)
        {
            PositionFlame.x = 0.62f;
            PositionFlame.y = 1.24f;
        }
        else if (anim.GetBool("isWalking") == false && anim.GetBool("isClimbing") == false && anim.GetBool("isClimb") == false)
        {
            PositionFlame.x = 0.45f;
            PositionFlame.y = 1.19f;
        }
        transform.localPosition = PositionFlame;

    }
}
