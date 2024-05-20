using UnityEngine;
using YG;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int reboundCount;
    [Space(10)]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D rigidBody2D;
    [SerializeField] private CircleCollider2D circleCollider;
    [SerializeField] private TrailRenderer bulletTrail; 
    [SerializeField] private ParticleSystem hitParticles;
    [SerializeField] private ParticleSystem reboundParticles;
    [SerializeField] private AudioSource audioSource;
    [Space(10)]
    [SerializeField] private AudioClip bulletReboundClip;
    [SerializeField] private AudioClip bulletGroundHitClip;


    private void OnEnable()
    {
        Invoke("Disable", 1f);

        if (!YandexGame.savesData.sounds)
        {
            audioSource.volume = 0.0f;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "ZombiePart":
                transform.parent = collision.gameObject.transform;
                spriteRenderer.enabled = false;
                circleCollider.enabled = false;
                rigidBody2D.simulated = false;
                bulletTrail.enabled = false;
                hitParticles.Play();
                break;

            case "Rebound":
                reboundCount--;
                audioSource.clip = bulletReboundClip;
                audioSource.Play();
                reboundParticles.Play();

                if(reboundCount == 0)
                {
                    transform.parent = collision.gameObject.transform;
                    spriteRenderer.enabled = false;
                    circleCollider.enabled = false;
                    rigidBody2D.simulated = false;
                    bulletTrail.enabled = false;

                    Invoke("Disable", 1);
                }
                break;

            default:
                transform.parent = collision.gameObject.transform;
                spriteRenderer.enabled = false;
                circleCollider.enabled = false;
                rigidBody2D.simulated = false;
                bulletTrail.enabled = false;
                audioSource.clip = bulletGroundHitClip;
                audioSource.Play();
                reboundParticles.Play();
                Invoke("Disable", 1);
                break;
        }
    }


    private void Disable()
    {
        gameObject.SetActive(false);
    }
}