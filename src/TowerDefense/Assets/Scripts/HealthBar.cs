using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slide;

    /// <summary>
    /// Set maximun health
    /// </summary>
    /// <param name="health">Health value</param>
    public void SetMaxHealth(int health)
    {
        slide.maxValue = health;
        slide.value = health;
    }

    /// <summary>
    /// Set current health
    /// </summary>
    /// <param name="health">Health value</param>
    public void SetHealth(int health) {
        slide.value = health;
    }
}
