using System;
using UnityEngine;
using UnityEngine.UI;
public class HazardVerticalMovement : MonoBehaviour
{
    private bool showSmallFishKill = false;
    private float smallFishKillAnimTimer = 0;
    
    public void ShowSmallFishDashAnimation()
    {
        smallFishKillAnimTimer = 0;
        showSmallFishKill = true;
    }
    
    void Update()
    {
        if (!GameManager.instance.isPlaying)
            return;
        if (showSmallFishKill)
        {
            UpdateAnimationSmallFishKill();
            return;
        }
        var currentPosition = gameObject.GetComponent<RectTransform>().anchoredPosition.y - Time.deltaTime * 250;
        if (currentPosition <= -1100)
        {
            Destroy(gameObject);
            return;
        }
        gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(gameObject.GetComponent<RectTransform>().anchoredPosition.x, currentPosition);
    }

    private void UpdateAnimationSmallFishKill()
    {
        if (smallFishKillAnimTimer > 1)
        {
            Destroy(gameObject);
            return;
        }
        smallFishKillAnimTimer += Time.deltaTime;
        transform.localScale = smallFishKillAnimTimer < 0.25 ? new Vector3(smallFishKillAnimTimer *12, smallFishKillAnimTimer * 12,3) : new Vector3(3,3,3);
        GetComponent<Image>().color = Color.Lerp(Color.white, Color.clear, smallFishKillAnimTimer);
    }
}
