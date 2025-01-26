using System;
using UnityEngine;

public class Impact : MonoBehaviour
{
    [SerializeField] Animator animator;

    private void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (other.gameObject.name.ToLower().StartsWith("player") && Movement.isDashing)
        {
            if (!gameObject.name.ToLower().Contains("small")) return;
            Score.IncrementScore(50);
            animator.SetTrigger("IsBounce");
            GetComponent<HazardVerticalMovement>().ShowSmallFishDashAnimation();
            GameManager.instance.kills++;
            GetComponent<BoxCollider2D>().enabled = false;
            return;
        }
        if (other.gameObject.name.ToLower().Contains("shadow")) return;
        GameManager.instance.isPlaying = false;
        GameManager.instance.ShowGameOverScreen();
    }

    private void OnCollisionEnter(Collision other)
    {
        //print("Collision Entered");
    }
}
