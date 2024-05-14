using UnityEngine;

public class FallSand : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Animator animator = GetComponent<Animator>();
            animator.enabled = true;
        }
    }
}