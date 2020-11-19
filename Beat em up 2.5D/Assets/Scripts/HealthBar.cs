using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Text txt;

    private int maxHealth;

    public void SetMaxHealth(int health)
    {
        maxHealth = health;
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetHealth(int health)
    {
        slider.value = health;
        if (slider.value <= 0)
        {
            slider.value = maxHealth;
        }
    }

    public void SetText(string text)
    {
        txt.text = text;
    }
}
