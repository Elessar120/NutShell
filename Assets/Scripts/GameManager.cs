using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
   public bool isPlaying;
   public static GameManager instance;
   public RectTransform newBubbleRect;
   public float dashDistance = 5000;
   public float dashingSpeed = 0.1f;
   public GameObject gameOverPage;
   public TextMeshProUGUI gameOverScore;
   public Movement playerMovement;
   public HazardSpawner hazardSpawner;
   public AudioSource gamePlaySound;
   public BackGround backGround;
   public int kills = 0;
   public int dashs = 0;
   public TextMeshProUGUI killText;
   public TextMeshProUGUI dashText;
   public GameObject score;
   public void ShowGameOverScreen()
   {
      gameOverScore.text = Score.score.ToString();
      score.gameObject.SetActive(false);
      killText.text = kills.ToString();
      dashText.text = dashs.ToString();
      //gamePlaySound.Stop();
      gameOverPage.SetActive(true);
   }
public void RePlay()
{
   kills = 0;
   dashs = 0;
   Score.Reset();
   score.gameObject.SetActive(true);
   backGround.Reset();
   gameOverPage.SetActive(false);
   playerMovement.Reset();
   hazardSpawner.Reset();
   newBubbleRect.GetComponent<NewBubble>().Reset();
   //gamePlaySound.time = 0;
   
   isPlaying = true;
   
}
   private void Awake()
   {
      if (instance == null)
      {
         instance = this;
      }
      else
      {
         Destroy(gameObject);
      }
   }
   
}
