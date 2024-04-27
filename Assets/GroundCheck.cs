using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] private Noobik noobik;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        noobik.isGrounded = true;
        noobik.jumpsNumber = 2;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        noobik.isGrounded = false;
    }
}