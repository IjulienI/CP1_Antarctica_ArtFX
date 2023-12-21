using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticleScrip : MonoBehaviour
{
    private Animator anim;
    [SerializeField] GameObject jump, land, walk;
    bool jumping = true;
    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (anim.GetBool("isJumping") == true)
        {
            if (jumping)
            {
                var jumpDust = Instantiate(jump, transform.position, transform.rotation);
            }
            jumping = false;
        }
        else if (anim.GetBool("isJumping") == false)
        {
            jumping = true;
        }
    }
    public void LandingParticle()
    {
        var landDust = Instantiate(land, transform.position, transform.rotation);
    }
    public void WalkParticle()
    {
        var walkDust = Instantiate(walk, transform.position, transform.rotation);
    }
}