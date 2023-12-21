using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewZone : MonoBehaviour
{
    [SerializeField] private GameObject targetPos;
    private GameObject ai;
    private GameObject target;

    private void Awake()
    {
        ai = GameObject.FindAnyObjectByType<AiStateMachine>().gameObject;
        target = GameObject.FindGameObjectWithTag("Target");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            ai.GetComponent<AiStateMachine>().SetFree();
            Invoke(nameof(SetTarget),0.1f);
        }
    }

    void SetTarget()
    {
        target.transform.position = targetPos.transform.position;
    }
}
