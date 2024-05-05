using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using YG;

public class Noobik : MonoBehaviour
{
    [SerializeField] private float jumpPower;
    [SerializeField] private float movementSpeed;
    [Space(10)]
    [SerializeField] private Transform groundChek;
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private Animator mainAnimator;
    [SerializeField] private AudioSource runningAudio;
    [SerializeField] private AudioSource jumpingAudio;

    [HideInInspector] public int jumpsNumber = 2;
    [HideInInspector] public bool isGrounded = true;

    private bool isFacingRight = true;
    private int horizontalInput;


    private void Update()
    {
        PlayerInput();
        Animations();
        BodyFlip();
    }
    private void FixedUpdate()
    {
        Movement();
    }

    private void PlayerInput()
    {
        horizontalInput = Mathf.RoundToInt(Input.GetAxisRaw("Horizontal"));       
        if(Input.GetKeyDown(KeyCode.W)) { DoubleJump(); }
    }
    private void Animations()
    {
        mainAnimator.SetInteger("Face", horizontalInput);
        mainAnimator.SetInteger("Run", horizontalInput);
        mainAnimator.SetBool("Jump", isGrounded);
    }
    private void DoubleJump()
    {
        if (isGrounded)
        {
            jumpsNumber--;
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpPower);
            jumpingAudio.Play();
        }
        else if (jumpsNumber > 0)
        {
            jumpsNumber--;
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpPower);
            jumpingAudio.Play();
        }
    }
    private void Movement()
    {
        rigidBody.velocity = new Vector2(horizontalInput * movementSpeed, rigidBody.velocity.y);       

        if(horizontalInput != 0 && isGrounded)
        {
            if (!runningAudio.isPlaying)
            {
                runningAudio.Play();
            }
        }
        else
        {
            runningAudio.Pause();
        }
    }
    private void BodyFlip()
    {
        if (horizontalInput > 0 && !isFacingRight)
        {
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
            isFacingRight = !isFacingRight; ;
        }
        else if (horizontalInput < 0 && isFacingRight)
        {
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
            isFacingRight = !isFacingRight;
        }     
    }

}