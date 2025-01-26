using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class NewBubble : MonoBehaviour
{
    private float currentPositionY = 2100;
    private float currentPositionX = 60;
    private bool rightDirection = true;

    public void Reset()
    {
        currentPositionY = 1920;
        currentPositionX = Random.Range(-450, 450);
        rightDirection = Random.Range(0, 2) == 0;
    }
    private void Start()
    {
        gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(currentPositionX, currentPositionY);
    }

    void Update()
    {
        if (!GameManager.instance.isPlaying)
            return;
        currentPositionY -= Time.deltaTime * 350;
        currentPositionX += (rightDirection ? 250 : -250) * Time.deltaTime;
        if (currentPositionX >= 450)
        {
            currentPositionX = 440;
            rightDirection = false;
        }else if (currentPositionX <= -450)
        {
            currentPositionX = -440;
            rightDirection = true;
        }
        
        if (currentPositionY <= -200)
        {
            Reset();
        }
        gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(currentPositionX, currentPositionY);
    }
}
