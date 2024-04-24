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
    [SerializeField][Range(-45f, 45f)] private float minNechRotationAngle;
    [SerializeField][Range(-45f, 45f)] private float maxNechRotationAngle;
    [SerializeField][Range(-45f, 45f)] private float minSpineRotationAngle;
    [SerializeField][Range(-45f, 45f)] private float maxSpineRotationAngle;
    [Space(10)]
    [SerializeField] private Transform groundChek;
    [SerializeField] private Transform handTransform;
    [SerializeField] private Transform neckhTransform;
    [SerializeField] private Transform spineTransform;
    [SerializeField] private Transform leftSholderTransform;
    [SerializeField] private Transform rightSholderTransform;
    [SerializeField] private Transform bulletSpawnPoint;
    [Space(10)]
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private Animator mainAnimator;
    [SerializeField] private AudioSource shootingSound;
    [SerializeField] private ParticleSystem muzzleBreak;
    [Space(10)]
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject sleve;
    [Space(10)]
    public LayerMask groundLayer;

    private Vector3 mousePosition;
    private int horizontalInput;
    private bool fireInput;
    private bool isFacingRight = true;

    private void Update()
    {
        GetPlayerInput();
        GetMousePosition();

        Jump();
        BodyFlip();
        NoobAnimations();
        NeckhRotation();
        SpineRotation();
        LeftSholderRotation();
        RightSholderTransform();
    }
    private void FixedUpdate()
    {
        Movement();
    }

    private void GetMousePosition()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    private void GetPlayerInput()
    {
        horizontalInput = Mathf.FloorToInt(Input.GetAxisRaw("Horizontal"));
        fireInput = Input.GetMouseButton(0);
    }
    private void NeckhRotation()
    {
        Vector3 direction = mousePosition - neckhTransform.position;
        float angle = Mathf.Atan2(direction.y * transform.localScale.y, direction.x * transform.localScale.x ) * Mathf.Rad2Deg;
        float clampedAngle = Mathf.Clamp(angle, minNechRotationAngle, maxNechRotationAngle);
        neckhTransform.localRotation = Quaternion.Euler(0, 0, clampedAngle);
    }
    private void SpineRotation()
    {
        Vector3 direction = mousePosition - spineTransform.position;
        float angle = Mathf.Atan2(direction.y * transform.localScale.y, direction.x * transform.localScale.x) * Mathf.Rad2Deg;
        float clampedAngle = Mathf.Clamp(angle, minSpineRotationAngle, maxSpineRotationAngle);
        spineTransform.localRotation = Quaternion.Euler(0, 0, clampedAngle);
    }
    private void LeftSholderRotation()
    {
        Vector3 direction = leftSholderTransform.position - handTransform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle -= 90f;
        leftSholderTransform.rotation = Quaternion.Euler(0, 0, angle);
    }
    private void RightSholderTransform()
    {
        Vector3 direction;
        float angle;

        if (isFacingRight)
        {
            direction = mousePosition - rightSholderTransform.position;
            angle = Mathf.Atan2(direction.y, direction.x) * Mathf .Rad2Deg;
            angle += 60f;
        }
        else
        {
            direction = rightSholderTransform.position - mousePosition;
            angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            angle -= 60f;
        }
       
        rightSholderTransform.rotation = Quaternion.Euler(0, 0, angle);
    }
    private void ShootingAnimation()
    {
        muzzleBreak.Play();
        GameObject bulletObject = Instantiate(bullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Rigidbody2D bulletRigid = bulletObject.GetComponent<Rigidbody2D>();
        bulletSpawnPoint.localRotation = Quaternion.Euler(0, 0, Random.Range(-1.5f, 1.5f));
        bulletRigid.velocity = bulletSpawnPoint.transform.right * 60f * transform.localScale.x ;
    }
    private void NoobAnimations()
    {
        if(horizontalInput > 0 && !isFacingRight)
        {
            mainAnimator.SetBool("LegsBackward", true);
        }
        else if(horizontalInput < 0 && isFacingRight)
        {
            mainAnimator.SetBool("LegsBackward", true);
        }
        else
        {
            mainAnimator.SetBool("LegsBackward", false);
        }

        mainAnimator.SetInteger("Legs", horizontalInput);
        mainAnimator.SetInteger("Face", horizontalInput);
        mainAnimator.SetBool("Hands", fireInput);
        mainAnimator.SetBool("Jump", IsGrounded());
    }
    private void Movement()
    {
        rigidBody.velocity = new Vector2(horizontalInput * movementSpeed, rigidBody.velocity.y);
    }
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundChek.position, 0.2f, groundLayer);
    }
    private void BodyFlip()
    {
        if (isFacingRight && mousePosition.x < transform.position.x || !isFacingRight && mousePosition.x > transform.position.x)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.W) && IsGrounded())
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpPower);
        }
    }
}