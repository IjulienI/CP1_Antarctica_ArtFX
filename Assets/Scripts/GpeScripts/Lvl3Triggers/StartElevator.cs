using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartElevator : MonoBehaviour
{
    [SerializeField] private GameObject InvisibleWallR;
    [SerializeField] private GameObject InvisibleWallL;
    [SerializeField] private GameObject InvisibleWallT;

    [SerializeField] private Animator ElevatorAnim;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        InvisibleWallR.SetActive(true);
        InvisibleWallL.SetActive(true);
        InvisibleWallT.SetActive(true);

        ElevatorAnim.enabled = true;
    }
}
