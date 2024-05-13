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
    [SerializeField] private AudioSource portalAudio;
    [SerializeField] private AudioSource runningAudio;
    [SerializeField] private AudioSource jumpingAudio;
    [SerializeField] private ParticleSystem runParticles;
    [HideInInspector] public int jumpsNumber = 2;
    [HideInInspector] public bool isGrounded = true;

    private Vector2 startPosition = Vector2.zero;
    private bool stopPlayerInput = false;
    private bool isFacingRight = true;
    private int horizontalInput;

    public static UnityEvent DeadEvent = new UnityEvent();
    public static UnityEvent AliveEvent = new UnityEvent();

    private void Start()
    {
        GetStartPosition();
    }
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

    private void GetStartPosition()
    {
        startPosition = transform.position;
    }
    private void PlayerInput()
    {
        horizontalInput = Mathf.RoundToInt(Input.GetAxisRaw("Horizontal"));
        if (Input.GetKeyDown(KeyCode.W)) { DoubleJump(); }
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

        if (!stopPlayerInput)
        {
            rigidBody.velocity = new Vector2(horizontalInput * movementSpeed, rigidBody.velocity.y);

            if (horizontalInput != 0 && isGrounded)
            {
                if (!runningAudio.isPlaying)
                {
                    runningAudio.Play();
                }

                if (!runParticles.isPlaying)
                {
                    runParticles.Play();
                }
            }
            else
            {
                runningAudio.Pause();
                runParticles.Stop();
            }
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
    private void ReplaseNoob()
    {
        transform.position = startPosition;
        AliveEvent.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Dead")
        {
            DeadEvent.Invoke();
            Invoke("ReplaseNoob", 1f);
        }
        else if (collision.gameObject.tag == "Teleport")
        {
            stopPlayerInput = true;
            rigidBody.velocity = Vector3.zero;
            portalAudio.Play();
            mainAnimator.SetTrigger("Teleportation");
            runningAudio.Stop();
            runParticles.Stop();
        }
    }
}