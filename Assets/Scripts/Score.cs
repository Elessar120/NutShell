using System;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
  public static int score = 0;
  public TextMeshProUGUI scoreText;
private float timer;
  private void Start()
  {
    scoreText = GetComponent<TextMeshProUGUI>();
  }

  public static void Reset()
  {
    score = 0;
  }

  public static void IncrementScore(int amount)
  {
    score += amount;
  }

  private void Update()
  {
    scoreText.text = score.ToString();
    if (GameManager.instance.isPlaying)
    {
      timer += Time.deltaTime;
        while (timer >= 1)
        {
          timer -= 1;
          score++;
        }
    }
  }
}