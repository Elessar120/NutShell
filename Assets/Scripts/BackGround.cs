using UnityEngine;

public class BackGround : MonoBehaviour
{
    private float currentPosition = 0;
    void Update()
    {
        if (!GameManager.instance.isPlaying)
            return;
        currentPosition -= Time.deltaTime * 750;
        if (currentPosition <= -3840)
        {
            currentPosition = 0;
        }
        gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, currentPosition);
    }

    public void Reset()
    {
        currentPosition = 0;
        gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, currentPosition);
    }
}
