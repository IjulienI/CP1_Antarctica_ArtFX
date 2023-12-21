using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ElevatorClamp : MonoBehaviour
{
    [SerializeField] GameObject elevator;
    [SerializeField] float location;
    [SerializeField] float speed;
    [SerializeField] Animator animator;

    private bool isInside = false;
    public bool finish = false;
    private GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");    }

    private void Update()
    {
        //if (transform.position.y < location)
        //{
        if (isInside && animator.isActiveAndEnabled == true)
            {
                player.transform.position = new Vector2(player.transform.position.x, elevator.transform.position.y - speed);
                //elevator.transform.position = new Vector2(elevator.transform.position.x + speed * Time.deltaTime, elevator.transform.position.y);
                //player.transform.position = new Vector2(player.transform.position.x + speed * Time.deltaTime, player.transform.position.y);
            }
        //}
        //else
        //{
            //finish = true;
        //}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isInside = true;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isInside = false;
        }
    }
}
