using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleScrip : MonoBehaviour
{
    [SerializeField] private float time;
    void Start()
    {
        Invoke("DestroyParticle", time);
    }

    private void DestroyParticle()
    {
        Destroy(gameObject);
    }
}
