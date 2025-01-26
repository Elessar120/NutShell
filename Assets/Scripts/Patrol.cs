using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Patrol : MonoBehaviour
{
    public float centerPosition; 
    public bool rightDirection;
    public RectTransform rectTransform;
    public float range = 450;

    public float speed;

    public float xPos;
    private void OnEnable()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (!GameManager.instance.isPlaying )
        {
            return;
        }
        Move();
    }

    public void SetDirection(bool direction)
    {
        rightDirection = direction;
    }
    private void Move()
    {
        if (rightDirection)
        {
            xPos += speed * Time.deltaTime;
            if (xPos > 450)
            {
                xPos = 450;
                rightDirection = !rightDirection;
            }

            if (xPos > centerPosition + range)
            {
                xPos = centerPosition + range;
                rightDirection = !rightDirection;
            }
            rectTransform.anchoredPosition = new Vector2(xPos, rectTransform.anchoredPosition.y);
            rectTransform.localScale = Vector3.one;
        }
        else if (!rightDirection)
        {
            xPos -= speed * Time.deltaTime;
            if (xPos < -450)
            {
                xPos = -450;
                rightDirection = !rightDirection;
            }

            if (xPos < centerPosition - range)
            {
                xPos = centerPosition - range;
                rightDirection = !rightDirection;
            }
            rectTransform.anchoredPosition = new Vector2(xPos, rectTransform.anchoredPosition.y);
            rectTransform.localScale = new Vector3(-1, 1,1);

        }
    }
}
