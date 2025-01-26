using System;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    [SerializeField] private RectTransform player;
    
    private void Start()
    {
    }

    void Update()
    {
        if (Movement.isDashing) return;
        GetComponent<RectTransform>().anchoredPosition = player.anchoredPosition;
    }
}
