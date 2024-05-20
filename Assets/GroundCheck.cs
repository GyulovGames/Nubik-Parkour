using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] private Noobik noobik;
    [SerializeField] private AudioSource landingAudio;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            landingAudio.Play();
            noobik.jumpsNumber = 2;
        }
    }
}