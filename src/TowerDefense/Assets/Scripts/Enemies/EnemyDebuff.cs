using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDebuff : MonoBehaviour
{
    Enemy enemyToDebuff;
    public List<float> appliedSlows;
    public float currentAppliedSlow;

    private void Awake()
    {
        enemyToDebuff = GetComponent<Enemy>();
        appliedSlows = new List<float>();
    }

    /// <summary>
    /// Applies debuff on current target
    /// </summary>
    /// <param name="slowFactor"></param>
    public void ApplyDebuff(float slowFactor)
    {
        //Keeps only the biggest debuff 
        if (slowFactor > currentAppliedSlow)
        {
            currentAppliedSlow = slowFactor;
        }

        float initialSpeed = enemyToDebuff.initialSpeed;
        float newSpeed = initialSpeed * (1 - currentAppliedSlow);
        enemyToDebuff.currentSpeed = newSpeed;

        //Keep a list of slows applied, because we can overlap effects
        appliedSlows.Add(slowFactor);
    }

    /// <summary>
    /// Removes debuff from current target
    /// </summary>
    /// <param name="slowFactor"></param>
    public void RemoveDebuff(float slowFactor)
    {
        appliedSlows.Remove(slowFactor);

        //If still have a slow to apply, else destroy the component
        if (appliedSlows.Count > 0)
        {
            if (slowFactor >= currentAppliedSlow)
            {
                //Only apply debuff if 'slowFactor' is bigger than current slow factor
                currentAppliedSlow = FindHighestSlowFactor();
                float initialSpeed = enemyToDebuff.initialSpeed;
                float newSpeed = initialSpeed * (1 - currentAppliedSlow);
                enemyToDebuff.currentSpeed = newSpeed;
            }
        }
        else
        {
            enemyToDebuff.currentSpeed = enemyToDebuff.initialSpeed;
            Destroy(this);
        }
    }

    /// <summary>
    /// Search for the biggest slow factor
    /// </summary>
    /// <returns>Biggest slow factor</returns>
    private float FindHighestSlowFactor()
    {
        float highest = 0f;
        foreach (var slow in appliedSlows)
        {
            if (slow > highest)
            {
                highest = slow;
            }
        }
        return highest;
    }
}
