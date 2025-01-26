using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
   public void LoadGame()
   {
      SceneManager.LoadScene("Game");
   }
}
