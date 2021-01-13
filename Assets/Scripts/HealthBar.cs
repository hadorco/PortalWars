using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class HealthBar : MonoBehaviour
{
    [SerializeField] public Slider mSlider;
    private int mMaxHealth;
    private Canvas mCanvas;

    private void Awake()
    {
        mCanvas = GameObject.Find("PingCanvas").GetComponent<Canvas>();
        transform.SetParent(mCanvas.transform, false);
    }

    public void SetMaxHealth(int maxHealth)
    {
        mSlider.maxValue = maxHealth;
        mSlider.value = maxHealth;
    }

    public void SetHealth(int health)
    {
        mSlider.value = health;
    }
}
