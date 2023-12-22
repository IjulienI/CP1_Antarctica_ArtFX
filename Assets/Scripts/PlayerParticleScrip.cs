using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;
using UnityEngine.InputSystem;

public class PlayerParticleScrip : MonoBehaviour
{
    private Animator anim;
    [SerializeField] GameObject jump, land, walk, run;
    bool jumping = true;
    Vector3 localScale;


    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        localScale = transform.localScale * 1.7f;

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
        landDust.transform.localScale = localScale;
    }
    public void WalkParticle()
    {
        var walkDust = Instantiate(walk, transform.position, transform.rotation);
        walkDust.transform.localScale = localScale;
    }

    public void RunParticle()
    {
        var runDust = Instantiate(run, transform.position, transform.rotation);
        runDust.transform.localScale = localScale;
    }
}