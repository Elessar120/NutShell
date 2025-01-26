using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    [SerializeField] float verticalSpeed = 1.25f;
    [SerializeField] float horizontalSpeed = 1.25f;
    private float yPos = 50;
    private float xPos;
    [SerializeField] private Transform mainBubble;
    private Animator animator;
    private RectTransform playerRect => GetComponent<RectTransform>();
    public static bool isDashing = false;
    public AudioSource sfx;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Dash();
        ShowDashingAnimation();
        Move();
        if (yPos > 500 && yPos < 510)
        {
            //yPos = 500;
            return;
        }else if (yPos > 510)
        {
            //yPos -= verticalSpeed * Time.deltaTime * 100;
            //playerRect.anchoredPosition = new Vector2(xPos, yPos);
        }
        else
        {
            yPos += verticalSpeed * Time.deltaTime * 100;
            playerRect.anchoredPosition = new Vector2(xPos, yPos);
        }
        
    }

    private void Dash()
    {
        if (!GameManager.instance.isPlaying || isDashing)
        {
            return;
        }
        if (!Input.GetKeyDown(KeyCode.Space))
        {
            return;
        }

        var distanceToNewBubble = Vector2.Distance(playerRect.anchoredPosition,
            GameManager.instance.newBubbleRect.anchoredPosition);
        if ( distanceToNewBubble > GameManager.instance.dashDistance)
        {
            GameManager.instance.isPlaying = false;
            GameManager.instance.ShowGameOverScreen();
        }
        else
        {
            isDashing = true;
            mainBubble.GetComponent<Animator>().Play("BubblePop");
            sfx.time = 0;
            sfx.Play();
            dashingTimer = 0;
            GameManager.instance.dashs++;
            animator.Play("Dashing");
            mainBubble.GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<AfterImage>().Show();
            Debug.Log("Dashing Started");
        }
    }

    private float dashingTimer=0;
    private void ShowDashingAnimation()
    {
        if (!isDashing) return;
        dashingTimer += Time.deltaTime * 3;
        playerRect.anchoredPosition +=  dashingTimer * 
                                        (GameManager.instance.newBubbleRect.anchoredPosition - playerRect.anchoredPosition);
        xPos = playerRect.anchoredPosition.x;
        yPos = playerRect.anchoredPosition.y;
//        Debug.Log(distanceToNewBubble);
        if (dashingTimer >= 1) //Check whether dashing finished
        {
            isDashing = false;
            playerRect.anchoredPosition = GameManager.instance.newBubbleRect.anchoredPosition;
            xPos = playerRect.anchoredPosition.x;
            yPos = playerRect.anchoredPosition.y;
            GameManager.instance.newBubbleRect.GetComponent<NewBubble>().Reset();
            mainBubble.GetComponent<Animator>().Play("BubbleIdle");
            Debug.Log("Dashing Finished");
            animator.Play("Idle");
            mainBubble.GetComponent<BoxCollider2D>().enabled = true;
            mainBubble.GetComponent<RectTransform>().anchoredPosition = playerRect.anchoredPosition;
            GetComponent<AfterImage>().Hide();
            return;
        }
        //Dashing Not finished

    }

    public void Reset()
    {
        xPos = 0;
        yPos = 50;
        playerRect.anchoredPosition = new Vector2(0,50);
    }
    private void Move()
    {
        if (!GameManager.instance.isPlaying || isDashing)
            return;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
                
            xPos -= horizontalSpeed * 100 * Time.deltaTime;
            if (xPos < -450)
            {
                xPos = -450;
                GameManager.instance.isPlaying = false;
                GameManager.instance.ShowGameOverScreen();
                
            }


        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            xPos += horizontalSpeed * 100 * Time.deltaTime;
            if (xPos > 450)
            {
                xPos = 450;
                GameManager.instance.isPlaying = false;
                GameManager.instance.ShowGameOverScreen();
            }
        }
        GetComponent<RectTransform>().anchoredPosition = new Vector2(xPos, yPos);

    }
}
